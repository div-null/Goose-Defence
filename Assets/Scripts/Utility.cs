using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Тип снаряда/башни
/// </summary>
public enum TowerType
{
	//Капуста
	Tomate = 0,
	Cabbage,
	Peas
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
