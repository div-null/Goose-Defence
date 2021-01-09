using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
	public Vector3 Position => _spawnPoint.position;
	public int Order { get; protected set; }
	public bool IsFree;
	Transform _spawnPoint;


	/// <summary>
	/// Установка площадки
	/// </summary>
	/// <param name="order"></param>
	/// <param name="status"></param>
	public void Initialize(int order, bool status, Vector3 position)
	{
		transform.position = position;
		Order = order;
		IsFree = status;
	}
	void Awake()
	{
		_spawnPoint = transform.Find("SpawnPoint");
	}

}
