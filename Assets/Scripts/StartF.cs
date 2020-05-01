using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartF : MonoBehaviour
{
    public List<GameObject> goose_prefabs;          //префабы гусей
    public Transform target;
    GooseFabric fabric;
    // Start is called before the first frame update
    void Start()
    {
        fabric = new GooseFabric();
        StartCoroutine(fabric.SpawnGeese(3,goose_prefabs));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            fabric.OnAttack(2,target.position,40);
        }
    }
}
