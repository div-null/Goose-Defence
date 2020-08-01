﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerFabric : Singleton<TowerFabric>
{
	public const int MaxTowerCount = 6;

	/// <summary>
	/// Количество не пустых мест
	/// </summary>
	public int TowerCount { get { return Places.Count(place => place.isFree); } }

	public GameObject PlacePrefab;

	[SerializeField]
	public GameObject[] ProjectilePrefabs;

	[SerializeField]
	public GameObject[] TowerPrefabs;

	public List<Tower> Towers;
	List<Target> Targets;
	Wall wall;
	BossBell bell;

	public GameObject[] place;
	public Place[] Places;

	/// <summary>
	/// Префаб полоски здоровья
	/// </summary>
	[SerializeField]
	GameObject hpBarPrefab;

	public int TowerSelectedOrder;

	[SerializeField]
	Vector3 UpSpawnPoint;

	[SerializeField]
	Vector3 DownSpawnPoint;


	public void spawnLocation()
	{
		Towers = new List<Tower>();
		Targets = new List<Target>();
		Places = new Place[MaxTowerCount];
		for (int i = 0; i < MaxTowerCount; i++)
		{
			Towers.Add(null);
			Vector3 pos = place[i].transform.position;
			pos.z = -3 + pos.y / 10f + 1.5f;
			var obj = GameObject.Instantiate(PlacePrefab, pos, Quaternion.identity);
			Places[i] = obj.GetComponent<Place>();
			Places[i].Initialize(i, true, pos);
		}

		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		UpSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		DownSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));


		//////////////////////////
		/// СПАВН стены
		var wallObj = GameObject.Instantiate(TowerPrefabs[9], new Vector3(-13.4f, 0.77f, 0), Quaternion.identity);
		wallObj.transform.tag = "Tower";

		Vector3 pos1 = wallObj.transform.position;
		pos1.z = Places[0].transform.position.z + 0.001f;
		wallObj.transform.position = pos1;

		wall = wallObj.GetComponent<Wall>();
		wall.Destroyed += destroyWall;
		wall.Initialize(new TowerStatsList.Wall().MaxHP);
		Targets.Add(wall);


		//////////////////////////
		/// СПАВН колокола
		var bellObj = GameObject.Instantiate(TowerPrefabs[10], new Vector3(-19.95f, -0.7f, 0), Quaternion.identity);
		bellObj.transform.tag = "Tower";

		pos1 = bellObj.transform.position;
		pos1.z = -3 + Mathf.Abs(pos1.y / 10) + 0.1f;
		bellObj.transform.position = pos1;

		bell = bellObj.GetComponent<BossBell>();
		bell.Destroyed += destroyBell;
		bell.Initialize(new TowerStatsList.Colocol().MaxHP);
		Targets.Add(bell);

		//////////////////////////
		/// СПАВН пустой башни
		var emptytower = GameObject.Instantiate(TowerPrefabs[10], new Vector3(1000f, -1.38f, 0), Quaternion.identity);
		bellObj.transform.tag = "Tower";

		pos1 = emptytower.transform.position;
		pos1.z = -3 + Mathf.Abs(pos1.y / 10) + 4f;
		emptytower.transform.position = pos1;

		var blank3 = emptytower.AddComponent<Tower>();
		blank3.Initialize(new TowerStatsList.Colocol(), ProjectilePrefabs[0], 10);
		Targets.Add(blank3);
	}


	public TowerStatsList GetStatsByOrder(int order)
	{
		if (Towers[order] != null)
			return Towers[order].info;
		return null;
	}

	public void placeTower(int order, TowerStatsList stats)
	{
		// НЕ ДЕЛАЮ БАЩНЮ ДОЧЕРНЕЙ 
		// + new Vector3(0,2f)
		Vector3 pos = Places[order].spawnPoint.position;
		GameObject tower = GameObject.Instantiate(TowerPrefabs[stats.PrefabId], pos, Quaternion.identity);


		var component = tower.AddComponent<Tower>();
		component.Initialize(stats, ProjectilePrefabs[stats.PrefabId / 3], order);
		component.MakeDamage();
		component.Destroyed += deleteTower;

		Targets.Add(component);
		Towers[order] = component;
		Places[order].isFree = false;
	}

	/// <summary>
	/// Обновляет башню на позиции
	/// </summary>
	/// <param name="order"></param>
	public void upgradeTower(int order)
	{
		TowerStatsList stats = GetInfoByOrder(order);
		var newPref = stats.PrefabId + 1;
		var newStats = TowerStatsList.GetStatsByPrefabId(newPref);
		destroyTower(order);
		placeTower(order, newStats);
	}

	/// <summary>
	/// УНИЧТОЖАЕТ ЗАДАННУЮ БАШНЮ
	/// </summary>
	/// <param name="order"></param>
	public void destroyTower(int order)
	{
		Tower tower = Towers[order];
		tower.Destroyed -= deleteTower;
		Targets.Remove(tower);
		tower.DestroySelf();
		Towers[order] = null;
		Places[order].isFree = true;
	}

	/// <summary>
	/// Обработчик уничтожения башни
	/// </summary>
	/// <param name="tower"></param>
	void deleteTower(Target tower)
	{
		int index = Towers.IndexOf((Tower)tower);
		destroyTower(index);
	}

	IEnumerator GooseBossAway()
	{
		GooseFabric.Instance.GoAwayAll();
		yield return new WaitForSeconds(2f);
		Game.Instance.LooseGame?.Invoke(false, Game.Instance.Score);
	}

	/// <summary>
	/// УНИЧТОЖЕН КОЛОКОЛ
	/// </summary>
	/// <param name="bossBell"></param>
	void destroyBell(Target bossBell)
	{
		Debug.Log("Bell Destroyed");
		Targets.Remove(bossBell);
		bossBell.DestroySelf();
		StartCoroutine(GooseBossAway());
	}

	/// <summary>
	/// УНИЧТОЖЕНА СТЕНА
	/// </summary>
	void destroyWall(Target wall)
	{
		for (int i = 0; i < 3; i++)
			Destroy(wall.transform.Find("wall " + i.ToString())
				 .GetComponent<BoxCollider>());
		wall.DestroySelf();
		Targets.Remove(wall);
		GooseFabric.Instance.loanchBoss();
		Debug.Log("Wall crushed");
	}

	public TowerStatsList GetInfoByOrder(int order)
	{
		return Towers[order].info;
	}

	public Target findNearTarget(Vector3 pos)
	{
		Vector3 temp = Vector3.down * 1000;
		float distance = 9999;
		Target result = null;
		foreach (var target in Targets)
		{
			if (target == null || target.HP <= 0)
				continue;

			Vector3 towerPos = target.transform.position;
			var dist = Vector3.Distance(towerPos, pos);
			if (dist < distance)
			{
				distance = dist;
				temp = towerPos;
				result = target;
			}
		}

		return result;
		//return (temp == Vector3.down * 1000) ? new Vector3(-10, 0, 0) : temp;
	}

}
