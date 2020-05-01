using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseFabric : MonoBehaviour
{
    public bool fabricActivity;                     //активность фабрики
    public List<GameObject> goose_prefabs;          //префабы гусей
    public List<Goose> geese;                       //ГУУУУСИИИИ

    public IEnumerator SpawnGeese(int level, List<GameObject> prefabs)
    {
        int count = 5 + level * 2;                  //формуля для вычисления кол-ва гусей
        for(int i = 0; i < count; i++)
        {
            var goose = GameObject.Instantiate(prefabs[Random.Range(0, 2)]);    //случайный префаб

            //добавление гуся
            geese.Add(new Goose(
                level                                      //левел
            ));
            goose.AddComponent<Goose>();

            yield return null;
        }
        
    }

    public GooseFabric()
    {
        fabricActivity = true;
        goose_prefabs = new List<GameObject>();
        geese = new List<Goose>();
    }
}
