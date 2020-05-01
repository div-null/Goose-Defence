using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartF : MonoBehaviour
{
    public List<GameObject> goose_prefabs;          //префабы гусей

    // Start is called before the first frame update
    void Start()
    {
        GooseFabric fabric = new GooseFabric();
        StartCoroutine(fabric.SpawnGeese(3,goose_prefabs));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
