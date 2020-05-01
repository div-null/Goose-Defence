using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseFabric : MonoBehaviour
{
    public bool fabricActivity;                     //активность фабрики
    public List<Goose> geese;                       //ГУУУУСИИИИ

    public IEnumerator SpawnGeese(int level, List<GameObject> prefabs)
    {
        int count = 5 + level * 2;                  //формуля для вычисления кол-ва гусей
        for(int i = 0; i < count; i++)
        {
            GameObject goose = GameObject.Instantiate(
                prefabs[Random.Range(0,3)],
                new Vector2(Random.Range(-5,5),Random.Range(-5,5)),
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
        fabricActivity = true;
        geese = new List<Goose>();
    }
}
