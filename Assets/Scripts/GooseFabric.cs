using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GooseFabric : Singleton<GooseFabric>
{
    [SerializeField]
    Transform UpSpawnPoint;

    [SerializeField]
    Transform DownSpawnPoint;

    /// <summary>
    /// Линии нумеруются сверху вниз
    /// </summary>
    [SerializeField]
    WalkLine[] Lines;
    GooseTypeStats[] GooseTypes;
    public bool fabric_activity;                     //активность фабрики
    [SerializeField]
    public List<Goose> geese;                       //ГУУУУСИИИИ
    [SerializeField]
    public List<GameObject> goose_prefabs;

    void Awake()
    {
        geese = new List<Goose>();

		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		var depth = 0;
		UpSpawnPoint.position = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
		DownSpawnPoint.position = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, depth));

		float length = UpSpawnPoint.position.y - DownSpawnPoint.position.y;
        float step = length / 6;
        float start = UpSpawnPoint.position.y;
        Lines = new WalkLine[6];
        for (int i = 0; i < 6; i++)
            Lines[i] = new WalkLine(start - i * step, start - (i + 1) * step);
    }

    int calcLineNumber(float pos)
    {
        for (int i = 0; i < 6; i++)
        {
            if (Lines[i].MinY < pos && Lines[i].MaxY > pos)
                return i;
        }
        return 5;
    }

	//Изменение текущего уровня
	public void ChangeGooseLvl(int gooseLvl)
	{
		this.gooseLvl = gooseLvl;
	}

	int gooseLvl = 1;

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
			int countGooseOnLvl = (gooseLvl / 25) / (int)Mathf.Sqrt(1 + (int)Mathf.Pow(gooseLvl / 25, 2)) * 50;

			

			float length = Mathf.Abs(UpSpawnPoint.position.y - DownSpawnPoint.position.y);
			float x = DownSpawnPoint.position.x;			
			float z = Random.Range(-1f, 0f);
			float y = UpSpawnPoint.position.y + z * length;
			
			

			//добавление гуся
			Goose tmpGoose = new Goose();
			tmpGoose.Initialize(gooseLvl);

			GameObject tmpGM = new GameObject(
				"Goose")
				;

			tmpGM.transform.position = new Vector3(x, y, z);
			tmpGM.transform.rotation = Quaternion.identity;
			var tmpG = tmpGM.AddComponent<Goose>();
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
        RaycastHit2D[] hits = Physics2D.CircleCastAll(target,radius,Vector2.down,5);                  //находим побитых гусей
		
        foreach(var hit in hits)                                                    //увидели гуся
        {
            hit.transform.gameObject.GetComponent<Goose>().OnDamage(damage, coefSlow, timeSlow);        //Бьём гуся
        }
    }
     


}
