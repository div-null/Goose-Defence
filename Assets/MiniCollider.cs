using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		parent = transform.parent.GetComponent<Tower>();        
    }
	Tower parent;


	private void OnTriggerEnter(Collider other)
	{
		parent.OnTriggerEnter(other);
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
