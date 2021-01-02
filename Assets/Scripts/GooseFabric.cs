using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GooseFabric : Singleton<GooseFabric>
{
	public event StatUpdate UpdateGooseLvl;

	Transform BattleField;

	[SerializeField]
	Vector3 UpSpawnPoint;

	[SerializeField]
	Vector3 DownSpawnPoint;

	/// <summary>
	/// Линии нумеруются сверху вниз
	/// </summary>
	[SerializeField]
	GooseTypeStats[] GooseTypes;

	/// <summary>
	/// Массив гусей
	/// </summary>
	[SerializeField]
	public List<Target> geese;

	Goose GooseBoss;

	[SerializeField]
	public List<GameObject> goose_prefabs;

	[SerializeField]
	public int gooseLvl;
	bool canSpawnBoss = true;

	public int GooseLvl { get { return gooseLvl; } protected set { gooseLvl = value; UpdateGooseLvl?.Invoke(gooseLvl); } }



	void Awake()
	{
		geese = new List<Target>();
		BattleField = GameObject.Find("BattleField").GetComponent<Transform>();
		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		var depth = 0;

		UpSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
		DownSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, depth));

		float length = UpSpawnPoint.y - DownSpawnPoint.y;
		float step = length / 6;
		float start = UpSpawnPoint.y;
	}

	//Изменение текущего уровня
	public void ChangeGooseLvl(int gooseLvl)
	{
		GooseLvl = gooseLvl;
	}

	public Goose FindGoose(Vector3 pos, float range)
	{
		float minDistance = range;
		Goose temp = null;
		foreach (var goose in geese)
		{
			float distance = (goose.transform.position - pos).magnitude;
			if (distance < minDistance && goose.HP > 0)
			{
				minDistance = distance;
				temp = (Goose)goose;
			}
		}
		return temp;
	}

	public void StartSpawning()
	{
		StartCoroutine("SpawnGeese");
	}

	public void loanchBoss()
	{
		StartCoroutine(LoanchBoss());
	}

	Vector3 getSpawnPosition()
	{
		float length = Mathf.Abs(UpSpawnPoint.y - DownSpawnPoint.y);
		float x = DownSpawnPoint.x;
		float z = Random.Range(-1f, 0f);
		float y = UpSpawnPoint.y + z * length;
		return new Vector3(x, y, z);
	}

	GameObject placeGoose(Vector3 position, float gooseSize)
	{
		// создаю объект с компонентами
		GameObject gooseCollider = new GameObject("Collider", typeof(SphereCollider));
		gooseCollider.transform.position = position;
		gooseCollider.transform.rotation = Quaternion.identity;

		//Добавил Никита
		//gooseCollider.AddComponent<SphereCollider>();
		SphereCollider collider = gooseCollider.GetComponent<SphereCollider>();
		collider.radius = gooseSize;
		collider.isTrigger = true;

		GameObject gooseObject = new GameObject("Goose");
		gooseObject.transform.tag = "Goose";
		gooseObject.transform.position = position;
		gooseObject.transform.rotation = Quaternion.identity;
		gooseObject.transform.SetParent(BattleField);
		//увеличиваю гуся в зависимости от его уровня
		gooseObject.transform.localScale = new Vector3(gooseSize, gooseSize);

		// помещаю коллайдер под гуся
		gooseCollider.transform.SetParent(gooseObject.transform);
		return gooseObject;
	}

	public IEnumerator SpawnGeese()
	{
		GooseLvl = 1;
		int spawnedGooseCount = 0;
		while (true)
		{
			spawnedGooseCount++;
			// вычисляю количество новых гусей на уровне
			int countGooseOnLvl = (int)((GooseLvl / 25f) / Mathf.Sqrt(1 + Mathf.Pow(GooseLvl / 25f, 2)) * 50);

			Vector3 spawnPosition = getSpawnPosition();
			var gooseObject = placeGoose(spawnPosition, (1f + GooseLvl / 25f));
			var goose = gooseObject.AddComponent<Goose>();
			goose.Initialize(GooseLvl);
			goose.Destroyed += onGooseDead;

			//TODO: вынести в метод Initialize
			GameObject.Instantiate(goose_prefabs[(int)goose.typeGoose], goose.transform, false);
			geese.Add(goose);

			if (countGooseOnLvl == spawnedGooseCount)
			{
				spawnedGooseCount = 0;
				GooseLvl++;
			}
			if (GooseLvl == 30)
			{
				loanchBoss();
				break;
			}
			yield return new WaitForSeconds(20f / countGooseOnLvl);
		}
	}

	IEnumerator LoanchBoss()
	{
		if (canSpawnBoss)
		{
			canSpawnBoss = false;

			StopCoroutine("SpawnGeese");
			yield return new WaitForSeconds(1.5f);

			Vector3 spawnPosition = getSpawnPosition();
			var gooseObject = placeGoose(spawnPosition, 4f);
			gooseObject.name = "Goose Boss";
			GooseBoss = gooseObject.AddComponent<Goose>();
			GooseBoss.Destroyed += onBossDead;

			GooseBoss.Initialize(40);
			GameObject.Instantiate(goose_prefabs[(int)GooseBoss.typeGoose], GooseBoss.transform, false);
			geese.Add(GooseBoss);
		}
	}

	/// <summary>
	/// Вызывается при смерте босса
	/// </summary>
	/// <param name="goose"></param>
	void onGooseDead(Target goose)
	{
		Game.Instance.increaseScore(goose.maxHP / 10);
		goose.Destroyed -= onGooseDead;
		geese.Remove(goose);
	}

	/// <summary>
	/// Вызывается при смерте босса
	/// </summary>
	/// <param name="goose"></param>
	void onBossDead(Target goose)
	{
		Game.Instance.WinGame?.Invoke(true, Game.Instance.Score);
		onGooseDead(goose);
	}

	public void GoAwayAll()
	{
		//GameObject.Find("Goose Boss").GetComponentInChildren<Animator>().SetTrigger("WithBell");
		// foreach (var item in geese)
		// {
		// 	item.transform.rotation = Quaternion.Euler(0, 180, 0);
		// }		
	}

	public void Clear()
	{
		Stopspawning();
		geese = new List<Target>();
		GooseLvl = 1;
	}

	public void Stopspawning()
	{
		StopAllCoroutines();
	}

	//TODO: вынести в класс Tower
	public void OnAttack(float radius, Vector2 target, int damage, float coefSlow = 1, float timeSlow = 0)
	{
		//находим побитых гусей
		RaycastHit2D[] hits = Physics2D.CircleCastAll(target, radius, Vector2.down, 5);

		foreach (var hit in hits)
		{
			//увидели гуся
			var parent = hit.transform.parent;
			if (parent == null)
				continue;
			var goose = parent.gameObject.GetComponent<Goose>();
			if (goose && goose.isAlive)
				goose.GetDamage(damage, coefSlow, timeSlow);        //Бьём гуся
		}
	}

}
