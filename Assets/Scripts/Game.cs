using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void EndGame(bool result, int score);

public class Game : Singleton<Game>
{

    // События победы и поражения
    event EndGame LooseGame;
    event EndGame WinGame;

    /// <summary>
    /// Игра идёт
    /// </summary>
    public bool isGameStarted { get; protected set; } = false;

    /// <summary>
    /// УРОВЕНЬ УГРОЗЫ
    /// </summary>
    public int WarnLevel { get; protected set; } = 1;
    
    int gooseCount { get { return Mathf.RoundToInt((WarnLevel / 25) / Mathf.Sqrt(1 + WarnLevel ^ 2) * 50); } }

    /// <summary>
    /// Хп стены
    /// </summary>
    [SerializeField]
    int wallHp = 50000;

    public int WallHp { get { return wallHp; } set {
            wallHp = value;
            if (wallHp <= 0)
                LooseGame?.Invoke(false, Score);
                } }

    [SerializeField]
    public int baseMoney = 5000;

    /// <summary>
    /// Ожидание между получением денег
    /// </summary>
    [SerializeField]
    float moneyBackDelay = 5f;

    /// <summary>
    /// Количество денег получаемое за ожидание
    /// </summary>
    int moneyPerDelay = 450;

    [SerializeField]
    int money;
    public int Money { get { return money; } protected set { money = value; } }

    [SerializeField]
    int score;
    public int Score { get { return score; } protected set { score = value; } }

    public void increaseScore(int ammount)
    {
        Score += ammount;
    }

    public void increaseMoney(int ammount)
    {
        Money += ammount;
    }

    public void decreaseMoney(int ammount)
    {
        Money -= ammount;
    }


    /// <summary>
    /// КОРУТИНА СПАВНА ГУСЕЙ И УВЕЛИЧЕНИЯ СЛОЖНОСТИ
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnGooses()
    {
        while (true)
        {
            GooseFabric.Instance.spawnGeeseOfType(1, gooseCount);
            yield return new WaitForSeconds(15);
            WarnLevel++;
        }
    }

    public void startGame()
    {
        StartCoroutine("BeginGame");
    }

    public void finishGame()
    {
        StartCoroutine("EndGame");
    }

    /// <summary>
    /// Регулярное получение денег
    /// </summary>
    /// <returns></returns>
    IEnumerator EarnMoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(moneyBackDelay);
            Money += moneyPerDelay;
        }
    }

    IEnumerator BeginGame()
    {
        yield return new WaitForSeconds(3f);
        isGameStarted = true;
        StartCoroutine("EarnMoney");
        StartCoroutine("SpawnGooses");
    }

    IEnumerator EndGame()
    {
        isGameStarted = false;
        StopCoroutine("EarnMoney");
        StopCoroutine("SpawnGooses");
        yield return new WaitForSeconds(4f);
    }

    [SerializeField]
    TowerStats[,] TowersTypes;

    [SerializeField]
    ProjectileStats[,] ProjectilesTypes;


    void Awake()
    {
        ///
        /// Статы Башен и снарядов
        ///
        int Lvl = (int)TowerLevel.T3 + 1;
        int Proj = (int)TowerType.Cabbage + 1;

        ProjectilesTypes = new ProjectileStats[Lvl, Proj];
        TowersTypes = new TowerStats[Lvl, Proj];

        //                                          Dmg  Range Speed
        ProjectilesTypes[0, 0] = new ProjectileStats(600, 0.3f, 1.5f);
        ProjectilesTypes[1, 0] = new ProjectileStats(2160, 0.3f, 1.5f);
        ProjectilesTypes[2, 0] = new ProjectileStats(2700, 0.3f, 1.5f);


        //                                 Hp  Range Delay   Deploy Cost  Projectile
        TowersTypes[0, 0] = new TowerStats(10000, 6, 15/12f, 4,     2500, ProjectilesTypes[0, 0]);
        TowersTypes[1, 0] = new TowerStats(20000, 6, 15/15f, 8,     3000, ProjectilesTypes[1, 0]);
        TowersTypes[2, 0] = new TowerStats(30000, 6, 15/18f, 12,    4500, ProjectilesTypes[2, 0]);

    }


}
