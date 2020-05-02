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
    public int TowerCount { get { return placeAwailable.Count(place => place); } }

    public GameObject[] TowerPrefabs;
    public List<Tower> Towers;
    public bool[] placeAwailable;
    public GameObject[] place;



    void Awake()
    {
        Towers = new List<Tower>();
        placeAwailable = new bool[MaxTowerCount];
        for (int i = 0; i < MaxTowerCount; i++)
        {
            Towers.Add(null);
            placeAwailable[i] = false;
        }
    }

    public void placeTower(int order, TowerStats stats, TowerType type)
    {
        // НЕ ДЕЛАЮ БАЩНЮ ДОЧЕРНЕЙ 
        GameObject tower = GameObject.Instantiate(TowerPrefabs[(int)type], place[order].transform.position + new Vector3(0,2f), Quaternion.identity);
        Towers[order] = tower.AddComponent<Tower>();
        Towers[order].Initialize(stats);
        Towers[order].MakeDamage();
        Towers[order].TowerDestroyed += deleteTower;
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
        placeAwailable[order] = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
