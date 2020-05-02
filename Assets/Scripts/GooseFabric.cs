using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GooseFabric : Singleton<GooseFabric>
{
    GooseTypeStats[] GooseTypes;

    public bool fabric_activity;                     //активность фабрики
    [SerializeField]
    public List<Goose> geese;                       //ГУУУУСИИИИ
    [SerializeField]
    public List<GameObject> goose_prefabs;

    public GooseFabric()
    {
        fabric_activity = true;
        geese = new List<Goose>();
        GooseTypes = new GooseTypeStats[3];
        GooseTypes[0] = new GooseTypeStats(250, 100, 1);
        GooseTypes[1] = new GooseTypeStats(400, 200, 1.2f);
        GooseTypes[2] = new GooseTypeStats(600, 300, 1.4f);
    }

    public IEnumerator SpawnGeese(int gooseLvl, int gooseCount)
    {
        GooseTypeStats stats = GooseTypes[gooseLvl - 1];

        for (int i = 0; i < gooseCount; i++)
        {
            float x = Random.Range(-5f, 5f);
            float z = Random.Range(-1f, 0f);
            float y = (z+0.5f)*10;
            GameObject goose = GameObject.Instantiate(
                goose_prefabs[gooseLvl-1],
                new Vector3(x,y,z),
                Quaternion.identity
            );    //случайный префаб


            //добавление гуся
            Goose g = goose.AddComponent<Goose>();
            goose.GetComponent<Goose>().Initialize(stats);
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
