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
        fabric_activity = true;
        geese = new List<Goose>();
        GooseTypes = new GooseTypeStats[3];
        GooseTypes[0] = new GooseTypeStats(250, 100, 1);
        GooseTypes[1] = new GooseTypeStats(400, 200, 1.2f);
        GooseTypes[2] = new GooseTypeStats(600, 300, 1.4f);

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

    public IEnumerator SpawnGeese(int gooseLvl, int gooseCount)
    {
        GooseTypeStats stats = GooseTypes[gooseLvl - 1];
        float upPosition = UpSpawnPoint.position.y;
        float length = UpSpawnPoint.position.y - DownSpawnPoint.position.y;
        float x = DownSpawnPoint.position.x;
        for (int i = 0; i < gooseCount; i++)
        {
            float z = Random.Range(-1f, 0f);
            float y = upPosition + z * length;

            GameObject goose = GameObject.Instantiate(
                goose_prefabs[gooseLvl-1],
                new Vector3(x,y,z),
                Quaternion.identity
            );

            //добавление гуся
            Goose g = goose.AddComponent<Goose>();
            int lineNum = calcLineNumber(y);
            goose.GetComponent<Goose>().Initialize(stats, lineNum);
            geese.Add(g);

            yield return new WaitForSeconds(0.3f);
        }
        
    }

    public void spawnGeeseOfType(int gooseLvl, int gooseCount)
    {
        StartCoroutine(SpawnGeese(gooseLvl,gooseCount));
    }



    public void OnAttack(float radius, Vector2 target, int damage)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(target,radius,Vector2.down,5);                  //находим побитых гусей

        foreach(var hit in hits)                                                    //увидели гуся
        {
            hit.transform.gameObject.GetComponent<Goose>().OnDamage(damage);        //Бьём гуся
        }
    }
     


}
