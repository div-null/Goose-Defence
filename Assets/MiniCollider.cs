using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniCollider : MonoBehaviour
{
	Tower parent;

    void Start()
    {
		parent = transform.parent.GetComponent<Tower>();        
    }

	private void OnTriggerEnter(Collider other)
	{
		parent.OnTriggerEnter(other);
	}
}
