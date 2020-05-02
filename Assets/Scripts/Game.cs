using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game>
{
    public bool isGameStarted { get; protected set; }

    /// <summary>
    /// Хп стены
    /// </summary>
    [SerializeField]
    int WallHp = 50000;

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
            // остановка
            if (!isGameStarted)
                StopCoroutine("EarnMoney");
        }
    }

    IEnumerator BeginGame()
    {
        yield return new WaitForSeconds(3f);
        isGameStarted = true;
        StartCoroutine("EarnMoney");
    }

    IEnumerator EndGame()
    {
        isGameStarted = false;
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
        int Lvl = (int)Level.T3 + 1;
        int Proj = (int)ProjectileType.Cabbage + 1;

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
