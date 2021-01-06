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
	public TargetType Type;

	/// <summary>
	/// максимальное хп
	/// </summary>
	public int MaxHP;

	[SerializeField]
	float _hp = 100f;


	public bool IsDestroyed => HP <= 0;

	public bool IsAlive => !IsDestroyed;

	/// <summary>
	/// Хп Башни, вызывает событие уничтожения уничтожение
	/// </summary>
	public float HP
	{
		get => _hp;
		protected set
		{
			if ( value >= _hp )
			{
				_hp = value;
				return;
			}
			_hp = value;
			Damaged?.Invoke(this);
			if ( _hp <= 0 )
			{
				_hp = 0;
				Destroyed?.Invoke(this);
			}
		}
	}

	/// <summary>
	/// Наносит урон цели
	/// </summary>
	/// <param name="dmg"></param>
	/// <returns></returns>
	public virtual bool GetDamage (float dmg)
	{
		if ( HP <= 0 )
			return false;
		HP -= dmg;
		return IsDestroyed;
	}

	public virtual void DestroySelf ()
	{
		this.enabled = false;
		GameObject.Destroy(gameObject);
	}

	public virtual void OnCollided (Collider collider)
	{

	}

	private void OnTriggerEnter (Collider other)
	{
		OnCollided(other);
	}
}
