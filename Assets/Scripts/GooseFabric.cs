using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GooseFabric : Singleton<GooseFabric>
{
	/// <summary>
	/// Начало новой волны
	/// </summary>
	public event StatUpdate UpdateGooseLvl;

	/// <summary>
	/// Номер волны
	/// </summary>
	public int GooseLvl { get => _gooseLvl; protected set { _gooseLvl = value; UpdateGooseLvl?.Invoke(_gooseLvl); } }

	/// <summary>
	/// Префабы гусей
	/// </summary>
	[SerializeField]
	private List<GameObject> _goosePrefabs = default;

	/// <summary>
	/// Массив живых гусей
	/// </summary>
	[SerializeField]
	private List<Target> _geese;

	[SerializeField]
	private int _gooseLvl;

	/// <summary>
	/// Верхняя граница спавна гусей
	/// </summary>
	[SerializeField]
	private Vector3 _upSpawnPoint;

	/// <summary>
	/// Нижняя граница спавна гусей
	/// </summary>
	[SerializeField]
	private Vector3 _downSpawnPoint;

	/// <summary>
	/// Босс на уровне
	/// </summary>
	private Goose _gooseBoss;

	private bool canSpawnBoss = true;

	private Transform _battleField;

	private Coroutine _spawnGeeseRoutine;

	public void StartSpawning ()
	{
		canSpawnBoss = true;
		_spawnGeeseRoutine = StartCoroutine(_spawnGeese());
	}

	public void Stopspawning ()
	{
		StopAllCoroutines();
	}

	public void LoanchBoss ()
	{
		StartCoroutine(_loanchBoss());
	}

	public void Clear ()
	{
		Stopspawning();
		_geese = new List<Target>();
		GooseLvl = 1;
	}

	//Изменение текущего уровня
	public void ChangeGooseLvl (int gooseLvl)
	{
		GooseLvl = gooseLvl;
	}

	public Goose FindGoose (Vector3 pos, float range)
	{
		float minDistance = range;
		Goose temp = null;
		foreach ( var goose in _geese )
		{
			float distance = ( goose.transform.position - pos ).magnitude;
			if ( distance < minDistance && goose.HP > 0 )
			{
				minDistance = distance;
				temp = (Goose)goose;
			}
		}
		return temp;
	}



	private void Awake ()
	{
		_geese = new List<Target>();
		_battleField = GameObject.Find("BattleField").GetComponent<Transform>();
		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		var depth = 0;

		_upSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth)) + Vector3.right;
		_downSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, depth)) + Vector3.right;
	}

	private Vector3 _getSpawnPosition ()
	{
		float length = Mathf.Abs(_upSpawnPoint.y - _downSpawnPoint.y);
		float x = _downSpawnPoint.x;
		float z = Random.Range(-1f, 0f);
		float y = _upSpawnPoint.y + z * length;
		return new Vector3(x, y, z);
	}

	private GameObject _placeGoose (Vector3 position, float gooseSize)
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
		gooseObject.transform.SetParent(_battleField);
		//увеличиваю гуся в зависимости от его уровня
		gooseObject.transform.localScale = new Vector3(gooseSize, gooseSize);

		// помещаю коллайдер под гуся
		gooseCollider.transform.SetParent(gooseObject.transform);
		return gooseObject;
	}

	/// <summary>
	/// Вызывается при смерте босса
	/// </summary>
	/// <param name="goose"></param>
	private void onGooseDead (Target goose)
	{
		Game.Instance.IncreaseScore(goose.MaxHP / 10);
		goose.Destroyed -= onGooseDead;
		_geese.Remove(goose);
	}

	/// <summary>
	/// Вызывается при смерте босса
	/// </summary>
	/// <param name="goose"></param>
	private void onBossDead (Target goose)
	{
		Game.Instance.WinGame?.Invoke(true, Game.Instance.Score);
		onGooseDead(goose);
	}

	private IEnumerator _spawnGeese ()
	{
		GooseLvl = 1;
		int spawnedGooseCount = 0;
		while ( true )
		{
			spawnedGooseCount++;
			// вычисляю количество новых гусей на уровне
			int countGooseOnLvl = (int)( ( GooseLvl / 25f ) / Mathf.Sqrt(1 + Mathf.Pow(GooseLvl / 25f, 2)) * 50 );

			Vector3 spawnPosition = _getSpawnPosition();
			var gooseObject = _placeGoose(spawnPosition, ( 1f + GooseLvl / 25f ));
			var goose = gooseObject.AddComponent<Goose>();
			goose.Initialize(GooseLvl);
			goose.Destroyed += onGooseDead;

			//TODO: вынести в метод Initialize
			GameObject.Instantiate(_goosePrefabs[(int)goose.GooseType], goose.transform, false);
			_geese.Add(goose);

			if ( countGooseOnLvl == spawnedGooseCount )
			{
				spawnedGooseCount = 0;
				GooseLvl++;
			}
			if ( GooseLvl == 30 )
			{
				LoanchBoss();
				break;
			}
			yield return new WaitForSeconds(20f / countGooseOnLvl);
		}
	}

	private IEnumerator _loanchBoss ()
	{
		if ( canSpawnBoss )
		{
			canSpawnBoss = false;

			this.StopRoutine(_spawnGeeseRoutine);
			yield return new WaitForSeconds(1.5f);

			Vector3 spawnPosition = _getSpawnPosition();
			var gooseObject = _placeGoose(spawnPosition, 4f);
			gooseObject.name = "Goose Boss";
			_gooseBoss = gooseObject.AddComponent<BossGoose>();
			_gooseBoss.Destroyed += onBossDead;

			_gooseBoss.Initialize(40);
			GameObject.Instantiate(_goosePrefabs[(int)_gooseBoss.GooseType], _gooseBoss.transform, false);
			_geese.Add(_gooseBoss);
		}
	}
}
