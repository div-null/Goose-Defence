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
    public int Damage;

    /// <summary>
    /// Радиус дамага
    /// </summary>
    public float ExplosionRange;

    /// <summary>
    /// Скорость полёта снаряда
    /// </summary>
    public float Velocity;
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


/// <summary>
/// Тропа(линия) от min до max
/// </summary>
public struct WalkLine
{
    public WalkLine(float minY, float maxY)
    {
        MinY = minY;
        MaxY = maxY;
    }
    public float MinY;
    public float MaxY;
}