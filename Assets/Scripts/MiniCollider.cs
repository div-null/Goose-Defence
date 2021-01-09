using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniCollider : MonoBehaviour
{
	Target parent;

	void Start()
	{
		parent = transform.parent.GetComponent<Target>();
	}

	private void OnTriggerEnter(Collider other)
	{
		parent.OnCollided(other);
	}
}
