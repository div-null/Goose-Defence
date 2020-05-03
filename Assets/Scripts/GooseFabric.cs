using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GooseFabric : Singleton<GooseFabric>
{
	public event StatUpdate UpdateGooseLvl;

	[SerializeField]
    Vector3 UpSpawnPoint;

    [SerializeField]
    Vector3 DownSpawnPoint;

    /// <summary>
    /// Линии нумеруются сверху вниз
    /// </summary>
    [SerializeField]
    GooseTypeStats[] GooseTypes;
    public bool fabric_activity;                     //активность фабрики
    [SerializeField]
    public List<Goose> geese;                       //ГУУУУСИИИИ
	[SerializeField]
	public List<GameObject> goose_prefabs;
	[SerializeField]
	public int gooseLvl;

	public int GooseLvl { get { return gooseLvl; } protected set { gooseLvl = value; UpdateGooseLvl?.Invoke(gooseLvl); } }


	void Awake()
    {
        geese = new List<Goose>();

		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		var depth = 0;

		UpSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
		DownSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, depth));

		float length = UpSpawnPoint.y - DownSpawnPoint.y;
        float step = length / 6;
        float start = UpSpawnPoint.y;
    }

    public Goose FindGoose(Vector3 pos, float range)
    {
        float minDistance = range;
        Goose temp = null;
        foreach (var goose in geese)
        {
            float distance = (goose.transform.position - pos).magnitude;
            if (distance < minDistance && goose.cur_hp > 0)
            {
                minDistance = distance;
                temp = goose;
            }
        }
        return temp;
    }

	//Изменение текущего уровня
	public void ChangeGooseLvl(int gooseLvl)
	{
		GooseLvl = gooseLvl;
	}

    void incScore(Goose goose)
    {
        Game.Instance.increaseScore(1);
    }

    /// <summary>
    /// Вызывается при смерте босса
    /// </summary>
    /// <param name="goose"></param>
    void bossDead(Goose goose)
    {
        Game.Instance.WinGame?.Invoke(true, Game.Instance.Score);
    }

    public void loanchGoose()
    {
        StartCoroutine(LoanchBoss());
    }

    public void loanchBoss()
    {
        StartCoroutine(LoanchBoss());
    }

    IEnumerator LoanchBoss()
    {
        StopCoroutine("SpawnGeese");
        yield return new WaitForSeconds(1.5f);

        float length = Mathf.Abs(UpSpawnPoint.y - DownSpawnPoint.y);
        float x = DownSpawnPoint.x;
        float z = Random.Range(-1f, 0f);
        float y = UpSpawnPoint.y + z * length;

        GameObject tmpCol = new GameObject("Collider");
        tmpCol.transform.position = new Vector3(x, y, z);
        tmpCol.transform.rotation = Quaternion.identity;

        GameObject tmpGM = new GameObject("Goose Boss");
        tmpGM.transform.tag = "Goose";
        tmpGM.transform.position = new Vector3(x, y, z);
        tmpGM.transform.rotation = Quaternion.identity;
        var tmpG = tmpGM.AddComponent<Goose>();
		tmpGM.transform.localScale = new Vector3(4, 4);
        tmpG.GooseDied += bossDead;

		//Добавил Никита
		var col = tmpCol.AddComponent<SphereCollider>();
        tmpCol.GetComponent<SphereCollider>().radius = 1;
        tmpCol.GetComponent<SphereCollider>().isTrigger = true;
        //
        tmpG.Initialize(40);
        GameObject.Instantiate(goose_prefabs[tmpG.typeGoose], tmpG.transform, false);
        tmpCol.transform.SetParent(tmpGM.transform);
        geese.Add(tmpG);
    }

	public void Clear()
	{
		Stopspawning();
		geese = new List<Goose>();
		GooseLvl = 1;
	}

	public void StartSpawning()
	{		
		StartCoroutine("SpawnGeese");
	}

	public void Stopspawning()
	{
        StopAllCoroutines();
	}

	public IEnumerator SpawnGeese()
    {
		GooseLvl = 1;
		int spawnedGooseCount = 0;
		while (true)
		{
			spawnedGooseCount++;
			int countGooseOnLvl = (int)((gooseLvl / 25f) / Mathf.Sqrt(1 + Mathf.Pow(gooseLvl / 25f, 2)) * 50);

			float length = Mathf.Abs(UpSpawnPoint.y - DownSpawnPoint.y);
			float x = DownSpawnPoint.x;			
			float z = Random.Range(-1f, 0f);
			float y = UpSpawnPoint.y + z * length;

            GameObject tmpCol = new GameObject("Collider");
            tmpCol.transform.position = new Vector3(x, y, z);
            tmpCol.transform.rotation = Quaternion.identity;

            GameObject tmpGM = new GameObject("Goose");
            tmpGM.transform.tag = "Goose";
            tmpGM.transform.position = new Vector3(x, y, z);
			tmpGM.transform.rotation = Quaternion.identity;
			var tmpG = tmpGM.AddComponent<Goose>();
            tmpG.GooseDied += incScore;
			tmpGM.transform.localScale = new Vector3((1f + gooseLvl / 25f), (1f + gooseLvl / 25f));
            //Добавил Никита
            var col = tmpCol.AddComponent<SphereCollider>();
            tmpCol.GetComponent<SphereCollider>().radius = 1;
            tmpCol.GetComponent<SphereCollider>().isTrigger = true;
            //
			tmpG.Initialize(gooseLvl);

			GameObject.Instantiate(goose_prefabs[tmpG.typeGoose], tmpG.transform, false);
            tmpCol.transform.SetParent(tmpGM.transform);
            geese.Add(tmpG);
			if(countGooseOnLvl == spawnedGooseCount)
			{
				spawnedGooseCount = 0;
				GooseLvl++;
			}
			if (gooseLvl == 30)
			{
				StartCoroutine("LoanchBoss");
				break;
			}
			yield return new WaitForSeconds(20f / countGooseOnLvl);
		}
	}

    public void OnAttack(float radius, Vector2 target, int damage, float coefSlow = 1, float timeSlow = 0)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(target, radius, Vector2.down, 5);                  //находим побитых гусей
		
        foreach(var hit in hits)                                                    //увидели гуся
        {
            var parent = hit.transform.parent;
            if (parent == null)
                continue;
            var goose = parent.gameObject.GetComponent<Goose>();
            if (goose && goose.cur_hp>0)
                goose.OnDamage(damage, coefSlow, timeSlow);        //Бьём гуся
        }
    }
     


}
