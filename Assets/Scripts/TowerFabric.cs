using System;
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

    public GameObject[] ProjectilePrefabs;
    public GameObject[] TowerPrefabs;
    public List<Tower> Towers;

    public GameObject[] place;
    public Place[] Places;


    public int TowerSelectedOrder;

    [SerializeField]
    Vector3 UpSpawnPoint;

    [SerializeField]
    Vector3 DownSpawnPoint;


    public void spawnLocation()
    {
        Towers = new List<Tower>();
        Places = new Place[MaxTowerCount];
        for (int i = 0; i < MaxTowerCount; i++)
        {
            Towers.Add(null);
            Vector3 pos = place[i].transform.position;
            pos.z = -3 + pos.y / 10f+1.5f;
            var obj = GameObject.Instantiate(PlacePrefab, pos, Quaternion.identity);
            Places[i] = obj.GetComponent<Place>();
            Places[i].Initialize(i, true, pos);
        }

        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        UpSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        DownSpawnPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));


        //////////////////////////
        /// СПАВН стены
        var wall = GameObject.Instantiate(TowerPrefabs[9], new Vector3(-13.4f, 0.77f, 0), Quaternion.identity);
        wall.transform.tag = "Tower";

        Vector3 pos1 = wall.transform.position;
        pos1.z = Places[0].transform.position.z + 0.001f;
        wall.transform.position = pos1;

        var blank1 = wall.AddComponent<Tower>();
        blank1.TowerDestroyed += destroyWall;
        blank1.Initialize(new TowerStatsList.Wall(), ProjectilePrefabs[0], 10);
        Towers.Add(blank1);


        //////////////////////////
        /// СПАВН колокола
        var colocol = GameObject.Instantiate(TowerPrefabs[10], new Vector3(-19.95f, -0.7f, 0), Quaternion.identity);
        colocol.transform.tag = "Tower";

        pos1 = colocol.transform.position;
        pos1.z = -3 + Mathf.Abs(pos1.y / 10) + 0.1f;
        colocol.transform.position = pos1;

        var blank2 = colocol.AddComponent<Tower>();
        blank2.TowerDestroyed += destroyColocol;
        blank2.Initialize(new TowerStatsList.Colocol(), ProjectilePrefabs[0], 10);
        Towers.Add(blank2);

		//////////////////////////
		/// СПАВН пустой башни
		var emptytower = GameObject.Instantiate(TowerPrefabs[10], new Vector3(1000f, -1.38f, 0), Quaternion.identity);
		colocol.transform.tag = "Tower";

		pos1 = emptytower.transform.position;
		pos1.z = -3 + Mathf.Abs(pos1.y / 10) + 4f;
		emptytower.transform.position = pos1;

		var blank3 = emptytower.AddComponent<Tower>();
		blank3.TowerDestroyed += destroyColocol;
		blank3.Initialize(new TowerStatsList.Colocol(), ProjectilePrefabs[0], 10);
		Towers.Add(blank3);
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
        component.TowerDestroyed += deleteTower;

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
        Towers[order].RemoveTower();
        Towers[order] = null;
        Places[order].isFree = true;
    }

    /// <summary>
    /// Обработчик уничтожения башни
    /// </summary>
    /// <param name="tower"></param>
    void deleteTower(Tower tower)
    {

        int index = Towers.IndexOf(tower);
        destroyTower(index);

    }

	IEnumerator GooseBossAway()
	{
		yield return new WaitForSeconds(4);
		Game.Instance.LooseGame?.Invoke(false, Game.Instance.Score);
	}

	/// <summary>
	/// УНИЧТОЖЕН КОЛОКОЛ
	/// </summary>
	/// <param name="tower"></param>
	void destroyColocol(Tower tower)
    {
        Towers.Remove(tower);
        tower.RemoveTower();
		GooseFabric.Instance.GoAwayAll();
		StartCoroutine("GooseBossAway");
	}

    /// <summary>
    /// УНИЧТОЖЕНА СТЕНА
    /// </summary>
    /// <param name="tower"></param>
    void destroyWall(Tower tower)
    {
        for (int i = 0; i < 3; i++)
            Destroy(tower.gameObject.transform.Find("wall " + i.ToString())
                 .gameObject.GetComponent<BoxCollider>());
        
        Towers.Remove(tower);
        
        //tower.RemoveTower();
        GooseFabric.Instance.loanchBoss();
        Debug.Log("Wall crushed");
    }
    public TowerStatsList GetInfoByOrder(int order)
	{
		return Towers[order].info;
	}

	public Vector3 FindNearTower(Vector3 pos)
    {
        Vector3 temp = Vector3.down*1000;
        float distance = 9999;
        foreach (var tower in Towers)
        {
            if (tower == null || tower.HP <= 0)
                continue;
            Vector3 towerPos = tower.transform.position;
            var dist = Vector3.Distance(towerPos, pos);
            if ( dist<distance)
            {
                distance = dist;
                temp = towerPos;
            }
        }
        return (temp == Vector3.down * 1000) ? new Vector3(-10, 0, 0) : temp;
    }

}
