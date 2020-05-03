using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartF : MonoBehaviour
{
    public List<GameObject> goose_prefabs;          //префабы гусей
    public Transform target;
   // GooseFabric fabric;
    ////public GameObject tower;
    // Start is called before the first frame update
    void Start()
    {
        TowerFabric.Instance.placeTower(0, new TowerStatsList.TowerTomatoT1());
        TowerFabric.Instance.placeTower(2, new TowerStatsList.TowerCabbageT1());
        TowerFabric.Instance.placeTower(1, new TowerStatsList.TowerTomatoT2());

        Game.Instance.startGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //tower.SetActive(true);
            //fabric.OnAttack(2,target.position,40);
        }
    }
}
