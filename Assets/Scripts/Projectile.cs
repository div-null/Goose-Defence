using System.Collections;
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
	public float CoefSlow;

	/// <summary>
	/// Время замедления
	/// </summary>
	public float TimeSlow;

	/// <summary>
	/// Направление полёта снаряда
	/// </summary>
	private Vector3 _direction;

	/// <summary>
	/// Снаряд закончил падение
	/// </summary>
	private bool _isLanded;

	/// <summary>
	/// Оставшееся время до падения
	/// </summary>
	private float _remainTime;

	public void Loauch (Vector3 tower, Vector3 point, ProjectileStats stats)
	{
		_isLanded = false;
		Damage = stats.Damage;
		this.Velocity = stats.Velocity;
		transform.position = tower;
		_direction = ( point - tower ).normalized;
		_remainTime = Vector3.Distance(tower, point) / stats.Velocity;
		Radius = stats.ExplosionRange;

		this.CoefSlow = stats.SlowMultiplier;
		this.TimeSlow = stats.SlowTime;
		transform.rotation = Quaternion.LookRotation(Vector3.back);
	}
	private void Start ()
	{
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate ()
	{
		if ( _remainTime <= 0 && !_isLanded )
		{
			_isLanded = true;
			_makeDamage();
		}

		if ( _remainTime > 0 )
		{
			// transform.Rotate(0, 0, 10f * Time.deltaTime);                       //вращение снаряда
			Vector3 newpos = transform.position + _direction * Velocity * Time.deltaTime;
			_remainTime -= Time.deltaTime;
			newpos.z = -3 + Mathf.Abs(newpos.y / 10);
			transform.position = newpos;
		}

	}

	private void _makeDamage ()
	{
		Vector2 pos = transform.position;
		_attack(pos);
		StartCoroutine(_destroy());
	}

	private void _attack (Vector2 target)
	{
		//находим побитых гусей
		RaycastHit2D[] hits = Physics2D.CircleCastAll(target, Radius, Vector2.down, 5);

		foreach ( var hit in hits )
		{
			//увидели гуся
			var parent = hit.transform.parent;
			if ( parent == null )
				continue;
			var goose = parent.gameObject.GetComponent<Goose>();
			if ( goose && goose.IsAlive )
				goose.GetDamage(Damage, CoefSlow, TimeSlow);
		}
	}

	private IEnumerator _destroy ()
	{
		animator.SetTrigger("Destroy");
		yield return new WaitForSeconds(1f);
		GameObject.Destroy(gameObject, DestroyTime);
		this.enabled = false;

	}
}
