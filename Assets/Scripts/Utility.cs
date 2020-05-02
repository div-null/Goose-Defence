using UnityEngine;
using System.Collections;

/// <summary>
/// Тип снаряда
/// </summary>
enum ProjectileType
{
    //Капуста
    Cabbage = 0
}

/// <summary>
/// Уровень башни
/// </summary>
enum Level
{
    T1 = 0,
    T2,
    T3
}


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
    public TowerStats(int Hp, float range, float attackDelay, float deployTime, float cost, ProjectileStats projectile)
    {
        HP = Hp;
        Range = range;
        AttackDelay = attackDelay;
        DeployTime = deployTime;
        Projectile = projectile;
        Cost = cost;
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

    /// <summary>
    /// Стоимость постройки башни
    /// </summary>
    float Cost;
}

