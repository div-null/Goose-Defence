using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ProjectileStats
{
    public ProjectileStats(int Damage, float Range, float Speed)
    {
        this.Damage = Damage;
        ExplosionRange = Range;
        Velocity = Speed;
    }

    /// <summary>
    /// Скорость снаряда
    /// </summary>
    int Damage;

    /// <summary>
    /// Радиус дамага
    /// </summary>
    float ExplosionRange;

    /// <summary>
    /// Скорость полёта снаряда
    /// </summary>
    float Velocity;
}


struct TowerStats
{
    public TowerStats(int Hp, float range, float attackDelay, float deployTime, ProjectileStats projectile)
    {
        HP = Hp;
        Range = range;
        AttackDelay = attackDelay;
        DeployTime = deployTime;
        Projectile = projectile;
    }

    /// <summary>
    /// Статы Снаряда
    /// </summary>
    ProjectileStats Projectile;

    /// <summary>
    /// Здоровье башни
    /// </summary>
    int HP;

    /// <summary>
    /// Время между атаками
    /// </summary>
    float AttackDelay;

    /// <summary>
    /// Дальность стрельбы
    /// </summary>
    float Range;

    /// <summary>
    /// Время установки
    /// </summary>
    float DeployTime;
}


public class Game : Singleton<Game>
{
    TowerStats[] TowersTypes;

    ProjectileStats[] ProjectilesTypes;

    [SerializeField]
    int WallHp = 15000;

    void Awake()
    {
        ///
        /// Статы Башен и снарядов
        ///
        ProjectilesTypes = new ProjectileStats[3]
        {
            //                  Hp  Range   Speed
            new ProjectileStats(300, 0.5f, 2f),
            new ProjectileStats(460, 0.8f, 1.5f),
            new ProjectileStats(550, 1.2f, 1.3f),
        };

        TowersTypes = new TowerStats[3]
        {
            //             Hp   Range Delay Deploy  Projectile
            new TowerStats(4000,    6,  2,   4,     ProjectilesTypes[0]),
            //            +1200
            new TowerStats(5200,    5, 1.7f, 8,     ProjectilesTypes[0]),
            //            +1600
            new TowerStats(6800,    4, 2.4f, 12,    ProjectilesTypes[0]),
        };
    }


}
