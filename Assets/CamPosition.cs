using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosition : MonoBehaviour
{
	Vector3 Medial;
	Transform Anchor;
	void Start()
	{
		//Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		//Anchor = GameObject.Find("Anchor").transform;
		//Medial = camera.ScreenToWorldPoint(new Vector3(0, Screen.height/2, 0));

		//var dist = Anchor.position - Medial;
		//camera.transform.position += dist;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
