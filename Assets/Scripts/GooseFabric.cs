using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GooseFabric : Singleton<GooseFabric>
{
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
	public int gooseLvl = 1;

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
            if (distance < minDistance)
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
		this.gooseLvl = gooseLvl;
	}



	public void StartSpawning()
	{
		StartCoroutine("SpawnGeese");
	}

	public void Stopspawning()
	{
		StopCoroutine("SpawnGeese");
	}

	public IEnumerator SpawnGeese()
    {
		int spawnedGooseCount = 0;
		while (true)
		{
			spawnedGooseCount++;
			int countGooseOnLvl = (int)((gooseLvl / 25f) / Mathf.Sqrt(1 + Mathf.Pow(gooseLvl / 25f, 2)) * 50);

			float length = Mathf.Abs(UpSpawnPoint.y - DownSpawnPoint.y);
			float x = DownSpawnPoint.x;			
			float z = Random.Range(-1f, 0f);
			float y = UpSpawnPoint.y + z * length;

			GameObject tmpGM = new GameObject("Goose");
			tmpGM.transform.position = new Vector3(x, y, z);
			tmpGM.transform.rotation = Quaternion.identity;
			var tmpG = tmpGM.AddComponent<Goose>();
            //Добавил Никита
            var col = tmpGM.AddComponent<SphereCollider>();
            tmpGM.GetComponent<SphereCollider>().radius = 1;
            //
			tmpG.Initialize(gooseLvl);

			GameObject.Instantiate(goose_prefabs[tmpG.typeGoose], tmpG.transform, false);

			geese.Add(tmpG);
			if(countGooseOnLvl == spawnedGooseCount)
			{
				spawnedGooseCount = 0;
				gooseLvl++;
			}
			yield return new WaitForSeconds(15f / countGooseOnLvl);
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
            if (goose)
                goose.OnDamage(damage, coefSlow, timeSlow);        //Бьём гуся
        }
    }
     


}
