using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GooseFabric : Singleton<GooseFabric>
{
    

    public bool fabric_activity;                     //активность фабрики
    [SerializeField]
    public List<Goose> geese;                       //ГУУУУСИИИИ

    public IEnumerator SpawnGeese(int level, List<GameObject> prefabs)
    {
        int count = 5 + level * 2;                  //формуля для вычисления кол-ва гусей
        for(int i = 0; i < count; i++)
        {
            float x = Random.Range(-5f, 5f);
            float z = Random.Range(-1f, 0f);
            float y = (z+0.5f)*10;
            GameObject goose = GameObject.Instantiate(
                prefabs[0],
                new Vector3(x,y,z),
                Quaternion.identity
            );    //случайный префаб

            //добавление гуся
            Goose g = goose.AddComponent<Goose>();
            goose.GetComponent<Goose>().Initialize(level);
            geese.Add(g);

            yield return null;
        }
        
    }


    public GooseFabric()
    {
        fabric_activity = true;
        geese = new List<Goose>();
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
