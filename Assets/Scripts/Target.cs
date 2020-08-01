using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum TargetType
{
	Goose,
	Human
}

public abstract class Target : MonoBehaviour
{
	/// <summary>
	/// Вызывается при получении урона
	/// </summary>
	public event Action<Target> Damaged;

	/// <summary>
	/// Вызывается при уничтожении
	/// </summary>
	public event Action<Target> Destroyed;

	/// <summary>
	/// Тип объекта
	/// </summary>
	public TargetType type;

	/// <summary>
	/// максимальное хп
	/// </summary>
	[SerializeField]
	public int maxHP;

	[SerializeField]
	float hp = 100f;


	public bool isDestroyed
	{
		get
		{
			return HP <= 0;
		}
	}

	public bool isAlive
	{
		get
		{
			return !isDestroyed;
		}
	}

	/// <summary>
	/// Хп Башни, вызывает событие уничтожения уничтожение
	/// </summary>
	[SerializeField]
	public float HP
	{
		get
		{
			return hp;
		}
		protected set
		{
			if (value >= hp)
			{
				hp = value;
				return;
			}
			hp = value;
			Damaged?.Invoke(this);
			if (hp <= 0)
			{
				hp = 0;
				Destroyed?.Invoke(this);
			}
		}
	}

	/// <summary>
	/// Наносит урон цели
	/// </summary>
	/// <param name="dmg"></param>
	/// <returns></returns>
	public virtual bool GetDamage(float dmg)
	{
		if (HP <= 0)
			return false;
		HP -= dmg;
		return isDestroyed;
	}

	public virtual void DestroySelf()
	{
		this.enabled = false;
		GameObject.Destroy(gameObject);
	}

	public virtual void OnCollided(Collider collider)
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		OnCollided(other);
	}
}
