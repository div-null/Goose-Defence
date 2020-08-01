using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
	public void Awake()
	{
		spawnPoint = transform.Find("SpawnPoint");
	}

	/// <summary>
	/// Устанавливаем 
	/// </summary>
	/// <param name="order"></param>
	/// <param name="status"></param>
	public void Initialize(int order, bool status, Vector3 position)
	{
		transform.position = position;
		Order = order;
		isFree = status;
	}

	public Transform spawnPoint;
	public int Order;
	public bool isFree;
}
