using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using UnityEditor;


[CreateAssetMenu(fileName = "Tower Stats", menuName = "Custom/Tower")]
public class TowerStats : ScriptableObject
{
	[Header("Summary information")]
	[Tooltip("Tower name")]
	[SerializeField]
	string towerName;
	/// <summary>
	/// Название
	/// </summary>
	public string Name => towerName;

	[TextArea]
	[Tooltip("Description of tower")]
	[SerializeField]
	string description;
	/// <summary>
	/// Описание
	/// </summary>
	public string Description => description;

	[SerializeField]
	TowerType towerType;



	[Header("Base stats")]
	[SerializeField]
	int maxHP;
	/// <summary>
	/// Максимальное количество здоровья
	/// </summary>
	public int MaxHP => maxHP;

	[Range(1, 3)]
	[SerializeField]
	int level;
	/// <summary>
	/// Уровень
	/// </summary>
	public int Level => level;

	[SerializeField]
	float cost;
	/// <summary>
	/// Стоимость
	/// </summary>
	public float Cost => cost;

	[Range(0f, 4f)]
	[SerializeField]
	float attackDelay;
	/// <summary>
	/// Задержка перед атакой
	/// </summary>
	public float AttackDelay => attackDelay;

	[Range(10f, 40f)]
	[SerializeField]
	float range;
	/// <summary>
	/// Дальность стрельбы
	/// </summary>
	public float Range => range;


	[Header("Projectile stats")]
	[SerializeField]
	ProjectileStats projectileStats;
	/// <summary>
	/// Статы снаряда
	/// </summary>
	public ProjectileStats Projectile => projectileStats;

	private void Reset()
	{
		towerName = "Tower";
		description = "Big tower";
		level = 1;
		cost = 1000f;

		maxHP = 1500;
		attackDelay = 2;
		range = 20f;

		projectileStats = new ProjectileStats(800, 1, 4f);
	}
}
