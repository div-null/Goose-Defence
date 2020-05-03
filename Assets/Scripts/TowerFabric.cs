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
    public int TowerCount { get { return freePlace.Count(place => place); } }

    public GameObject[] ProjectilePrefabs;
    public GameObject[] TowerPrefabs;
    public List<Tower> Towers;
    public bool[] freePlace;
    public GameObject[] place;

    void Awake()
    {
        Towers = new List<Tower>();
        freePlace = new bool[MaxTowerCount];
        for (int i = 0; i < MaxTowerCount; i++)
        {
            Towers.Add(null);
            freePlace[i] = true;
        }

        //////////////////////////
        /// СПАВН ПСЕВДО СТЕНЫ
        var wall = new GameObject("wall");
        wall.transform.position = new Vector3(-10000, 0, 0);
        var blank = wall.AddComponent<Tower>();
        Towers.Add(blank);
    }

    public void placeTower(int order, TowerStatsList stats)
    {
        // НЕ ДЕЛАЮ БАЩНЮ ДОЧЕРНЕЙ 
        // + new Vector3(0,2f)
        GameObject tower = GameObject.Instantiate(TowerPrefabs[stats.PrefabId % 3], place[order].transform.position, Quaternion.identity);
        Towers[order] = tower.AddComponent<Tower>();
        Towers[order].Initialize(stats, ProjectilePrefabs[stats.PrefabId / 3]);
        Towers[order].MakeDamage();
        Towers[order].TowerDestroyed += deleteTower;
        freePlace[order] = false;
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
            if (tower == null)
                continue;
            Vector3 towerPos = tower.transform.position;
            var dist = Vector3.Distance(towerPos, pos);
            if ( dist<distance)
            {
                distance = dist;
                temp = towerPos;
            }
        }
        return (temp == Vector3.down * 1000) ? Towers[6].transform.position : temp;
    }

    public void TryDamageTower(int order, int dmg)
    {
        if (Towers[order])
            Towers[order].GetDamage(dmg);
        else
            Game.Instance.DamageWall(dmg);
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

    /// <summary>
    /// УНИЧТОЖАЕТ ЗАДАННУЮ БАШНЮ
    /// </summary>
    /// <param name="order"></param>
    public void destroyTower(int order)
    {
        Towers[order].RemoveTower();
        Towers[order] = null;
        freePlace[order] = true;
    }
}
