using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GooseState
{
	Stay = 0,
	Walk,
	Run,
	Attack,
	Dead
}

public enum GooseKind
{
	White,
	Gray,
	Brown,
	Black,
	Boss
}

public class Goose : Target
{
	public int Level { get; protected set; }

	/// <summary>
	/// Урон гуся
	/// </summary>
	public int Damage { get; protected set; }

	/// <summary>
	/// Скорость
	/// </summary>
	public float Speed { get => _speed; protected set => _speed = value; }

	/// <summary>
	/// Вектор скорости
	/// </summary>
	public Vector3 Movement { get; protected set; }

	/// <summary>
	/// Множитель скорости
	/// </summary>
	public float SpeedMultiplier { get => _speedMultiplier; protected set => _speedMultiplier = value; }

	/// <summary>
	/// Множитель частоты атаки
	/// </summary>
	public float AttackSpeed { get => _attackSpeed; protected set => _attackSpeed = value; }

	/// <summary>
	/// Состояние гуся
	/// </summary>
	public GooseState State;
	public GooseKind GooseType;


	[SerializeField]
	protected float _speed = 1.5f;

	[SerializeField]
	protected float _speedMultiplier;

	[SerializeField]
	protected float _attackSpeed;

	[SerializeField]
	protected float _basePositionLayer = -3f;

	protected Target _aim;
	protected AudioSource _audioSource;
	protected Animator _animator;
	protected Coroutine _slowdownRoutine;



	//FIXME: вынести реализацию статистик в фабрику гусей
	public virtual void Initialize (int lvl)
	{
		Level = lvl;
		State = GooseState.Stay;

		int gooseGrade = Level / 10;
		bool advancedKind = Random.Range(1, 10) * ( Level % 10 ) > 50;
		switch ( gooseGrade )
		{
			case 0:
			GooseType = advancedKind ? GooseKind.Gray : GooseKind.White;
			break;

			case 1:
			GooseType = advancedKind ? GooseKind.Brown : GooseKind.Gray;
			break;

			case 2:
			GooseType = advancedKind ? GooseKind.Black : GooseKind.Brown;
			break;
			default:
			GooseType = GooseKind.Boss;
			break;
		}
		MaxHP = (int)( ( Level / 5f + 1f ) * 250f );
		HP = MaxHP;
		Damage = (int)( MaxHP / 2.5 );

		SpeedMultiplier = 1f + Level / 25f;

		//Тут надо попроавить:
		AttackSpeed = 2f - SpeedMultiplier / 2f;
		_findTarget(null);
	}

	public void StartAttack (Target target)
	{
		StartCoroutine(_attack(target));
	}

	public virtual Vector3 Walk (Vector3 direction)
	{
		//FIXME: скорректировать перемещение гусей по оси z
		var velocity = direction.normalized * Speed * SpeedMultiplier;
		//_basePositionLayer
		//velocity.z = -3f + Mathf.Abs(velocity.y / 10);
		return velocity;
	}

	/// <summary>
	/// Наносит урон гусю
	/// TODO: переработать замедление с помощью системы эффектов
	/// </summary>
	/// <param name="damage">Урон</param>
	/// <param name="coefSlow">Коэффициент замедления</param>
	/// <param name="timeSlow">Время замедления</param>
	/// <returns></returns>
	public bool GetDamage (float damage, float coefSlow = 1, float timeSlow = 0)
	{
		base.GetDamage(damage);
		if ( timeSlow != 0 )
		{
			this.StopRoutine(_slowdownRoutine);
			_slowdownRoutine = StartCoroutine(_slowDown(coefSlow, timeSlow));
		}
		if ( IsDestroyed && State != GooseState.Dead )
		{
			State = GooseState.Dead;
			StopAllCoroutines();
			//воспроизведение анимации
			StartCoroutine(_onDeath());
		}
		return IsDestroyed;
	}

	void Start ()
	{
		_animator = transform.GetComponentInChildren<Animator>();
		_audioSource = GetComponentInChildren<AudioSource>();
	}

	void FixedUpdate ()
	{
		if ( _aim == null )
			return;
		var position = _aim.transform.position - new Vector3(0, 0, 0.5f);
		var direction = ( position - transform.position );
		if ( direction.magnitude > 0.1 && State != GooseState.Attack && State != GooseState.Dead )
		{
			//воспроизведение анимации ходьбы
			State = GooseState.Walk;
			_animator.SetInteger("GooseState", (int)State);
			_animator.speed = SpeedMultiplier;

			Movement = Walk(direction);

			// поворот и перемещени гуся
			transform.rotation = Quaternion.Euler(0, Movement.x < 0 ? 0 : 180, 0);
			transform.position += Movement * Time.deltaTime;
		}
		else
		{
			Movement = Vector3.zero;
			_animator.SetInteger("GooseState", (int)GooseState.Stay);
		}
	}

	void _findTarget (Target target)
	{
		// если ранее цель была установлена
		if ( _aim != null )
			_aim.Destroyed -= _findTarget;

		//нахожу новую цель
		if ( target == null || target.IsDestroyed )
			_aim = TowerFabric.Instance.findNearTarget(transform.position);
		else
			_aim = target;
		_aim.Destroyed += _findTarget;
	}

	IEnumerator _attack (Target target)
	{
		State = GooseState.Attack;
		while ( true )
		{
			if ( target == null || target.IsDestroyed )
				break;
			_audioSource.Play();
			//Небольшой разброс дамага
			int tmpGooseDamage = Damage + (int)( Random.Range(-0.1f * Damage, 0.1f * Damage) );

			//TowerFabric.Instance.TryDamageTower(TowerNumber, goose_damage);

			//Воспроизведение анимации атаки
			_animator.SetInteger("GooseState", (int)GooseState.Attack);

			yield return new WaitForSeconds(AttackSpeed / 2);
			target.GetDamage(tmpGooseDamage);  //Нанесение урона в середине анимации
			yield return new WaitForSeconds(AttackSpeed / 2);

			_animator.SetInteger("GooseState", (int)GooseState.Stay);

		}
		State = GooseState.Walk;
	}

	IEnumerator _slowDown (float coefSlow = 1, float timeSlow = 0)
	{
		SpeedMultiplier = ( 1 + Level / 25 ) * coefSlow;
		AttackSpeed = 2 - SpeedMultiplier / 2;
		yield return new WaitForSeconds(timeSlow);
		SpeedMultiplier = 1 + Level / 25;
		//Тут надо поправить:
		AttackSpeed = 2 - SpeedMultiplier / 2;
	}

	IEnumerator _onDeath ()
	{
		_animator.speed = 1;
		if ( _aim != null )
			_aim.Destroyed -= _findTarget;
		State = GooseState.Dead;
		_animator.SetInteger("GooseState", (int)State);   //death
		yield return new WaitForSeconds(0.9f / SpeedMultiplier);
		Destroy(this.gameObject);
	}
}
