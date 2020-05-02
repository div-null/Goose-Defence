﻿using System.Collections;
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
        TowerFabric.Instance.placeTower(0, new TowerStatsList.TowerPeasT3(), TowerType.Cabbage);
        TowerFabric.Instance.placeTower(1, new TowerStatsList.TowerPeasT3(), TowerType.Cabbage);

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
