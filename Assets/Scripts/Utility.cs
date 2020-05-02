using UnityEngine;
using System.Collections;

/// <summary>
/// Тип снаряда/башни
/// </summary>
public enum TowerType
{
    //Капуста
    Cabbage = 0
}

/// <summary>
/// Уровень башни
/// </summary>
public enum TowerLevel
{
    T1 = 0,
    T2,
    T3
}

/// <summary>
/// Статы снаряда
/// </summary>
public struct ProjectileStats
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


/// <summary>
/// Статы башни
/// </summary>
public struct TowerStats
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

/// <summary>
/// Статы гуся определённого ТИПА
/// </summary>
public struct GooseTypeStats
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hp">Хп</param>
    /// <param name="dmg">Дамаг</param>
    /// <param name="speedMul">Множитель скорости</param>
    public GooseTypeStats(int hp, int dmg, float speedMul)
    {
        Damage = dmg;
        Hp = hp;
        SpeedMultiplier = speedMul;
    }

    public int Damage;
    public int Hp;
    public float SpeedMultiplier;
}
