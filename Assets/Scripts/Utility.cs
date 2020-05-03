using UnityEngine;
using System.Collections;

/// <summary>
/// Тип снаряда/башни
/// </summary>
public enum TowerType
{
    //Капуста
    Tomate =0,
    Cabbage,
    Peas
}


/// <summary>
/// Статы снаряда
/// </summary>
public struct ProjectileStats
{
    public ProjectileStats(int Damage, float Range, float Speed, float coefSlow = 1, float timeSlow = 0)
    {
        this.Damage = Damage;
        ExplosionRange = Range;
        Velocity = Speed;
		this.coefSlow = coefSlow;
		this.timeSlow = timeSlow;

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
	/// <summary>
	/// Коэффициент замедения
	/// </summary>
	public float coefSlow;

	/// <summary>
	/// Время замедления
	/// </summary>
	public float timeSlow;
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
