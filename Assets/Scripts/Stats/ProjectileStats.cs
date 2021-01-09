using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Статы снаряда
/// </summary>
[Serializable]
public class ProjectileStats
{
	public ProjectileStats(int Damage, float Range, float Speed, float coefSlow = 1, float timeSlow = 0)
	{
		this.Damage = Damage;
		ExplosionRange = Range;
		Velocity = Speed;
		this.SlowMultiplier = coefSlow;
		this.SlowTime = timeSlow;
	}

	/// <summary>
	/// Скорость снаряда
	/// </summary>
	[Range(50, 8000)]
	[SerializeField]
	public int Damage;

	/// <summary>
	/// Радиус дамага
	/// </summary>
	[Range(0.01f, 12f)]
	[SerializeField]
	public float ExplosionRange;

	/// <summary>
	/// Скорость полёта снаряда
	/// </summary>
	[Range(1, 50)]
	[SerializeField]
	public float Velocity;

	/// <summary>
	/// Коэффициент замедения
	/// </summary>
	[Range(0, 1)]
	[SerializeField]
	public float SlowMultiplier;

	/// <summary>
	/// Время замедления
	/// </summary>
	[Range(0, 14)]
	[SerializeField]
	public float SlowTime;
}
