using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Projectile : MonoBehaviour
{
	public Animator animator;
	/// <summary>
	/// Радиус дамага
	/// </summary>
	[SerializeField]
	public float Radius = 2f;

	[SerializeField]
	public int Damage;

	/// <summary>
	/// Скорость снаряда
	/// </summary>
	[SerializeField]
	public float Velocity;

	/// <summary>
	/// Время до уничтожения лежащего объекта
	/// </summary>
	[SerializeField]
	public float DestroyTime;

	/// <summary>
	/// Коэффициент замедения
	/// </summary>
	public float coefSlow;

	/// <summary>
	/// Время замедления
	/// </summary>
	public float timeSlow;

	/// <summary>
	/// Направление полёта снаряда
	/// </summary>
	Vector3 Direction;

	/// <summary>
	/// Снаряд закончил падение
	/// </summary>
	bool isLanded;

	/// <summary>
	/// Оставшееся время до падения
	/// </summary>
	float RemainTime;

	public void Loauch (Vector3 tower, Vector3 point, ProjectileStats stats)
	{
		isLanded = false;
		Damage = stats.Damage;
		this.Velocity = stats.Velocity;
		transform.position = tower;
		Direction = ( point - tower ).normalized;
		RemainTime = Vector3.Distance(tower, point) / stats.Velocity;
		Radius = stats.ExplosionRange;

		this.coefSlow = stats.SlowMultiplier;
		this.timeSlow = stats.SlowTime;
		//TODO: добавить поворот
		transform.rotation = Quaternion.LookRotation(Vector3.back);
	}

	void MakeDamage ()
	{
		Vector2 pos = transform.position;
		// TODO: Вызов метода дамага гусей
		GooseFabric.Instance.OnAttack(Radius, pos, Damage, coefSlow, timeSlow);
		StartCoroutine(Destroy());
	}


	private void Start ()
	{
		animator = GetComponent<Animator>();
	}

	void FixedUpdate ()
	{
		if ( RemainTime <= 0 && !isLanded )
		{
			isLanded = true;
			MakeDamage();
		}

		if ( RemainTime > 0 )
		{
			// transform.Rotate(0, 0, 10f * Time.deltaTime);                       //вращение снаряда     
			Vector3 newpos = transform.position + Direction * Velocity * Time.deltaTime;
			RemainTime -= Time.deltaTime;
			newpos.z = -3 + Mathf.Abs(newpos.y / 10);
			transform.position = newpos;
		}

	}

	IEnumerator Destroy ()
	{
		animator.SetTrigger("Destroy");
		yield return new WaitForSeconds(1f);
		GameObject.Destroy(gameObject, DestroyTime);
		this.enabled = false;

	}
}
