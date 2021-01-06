using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Tower : Target
{
	public TowerStats Stats => _stats;

	public bool IsAvailable { get; protected set; }

	public int TowerOrder;

	/// <summary>
	/// Точка спавна снарядов
	/// </summary>
	[SerializeField]
	protected List<Transform> _spawnPoints;


	/// <summary>
	/// Префаб снаряда
	/// </summary>
	[SerializeField]
	protected GameObject _projectilePrefab;

	[SerializeField]
	protected TowerStats _stats;

	[SerializeField]
	protected Transform _battlefield;

	[SerializeField]
	protected AudioSource _audioSource;

	[SerializeField]
	protected Animator _animator;

	private Coroutine _attackRoutine;


	public void Initialize (TowerStats stats, GameObject projectilePref, int order)
	{
		_animator = GetComponent<Animator>();
		_spawnPoints = new List<Transform>();
		this._stats = stats;
		TowerOrder = order;
		Type = TargetType.Human;
		MaxHP = stats.MaxHP;
		HP = stats.MaxHP;

		_projectilePrefab = projectilePref;

		// FIXME: КОСТЫЛЬ
		Tower tower = GetComponentInChildren<Tower>();

		foreach ( var child in tower.GetComponentsInChildren<Transform>() )
			if ( child.CompareTag("FirePoint") )
				_spawnPoints.Add(child);
	}

	public void MakeDamage ()
	{
		IsAvailable = true;
		_attackRoutine = StartCoroutine(_attack());
	}

	public void StopDamage ()
	{
		this.StopRoutine(_attackRoutine);
		IsAvailable = false;
	}

	public override void OnCollided (Collider collider)
	{
		var goose = collider.gameObject.GetComponentInParent<Goose>();
		if ( goose == null )
			return;

		if ( goose.State != GooseState.Attack )
			goose.StartAttack(this);
	}

	public override void DestroySelf ()
	{
		HP = 0;
		this.StopRoutine(_attackRoutine);
		base.DestroySelf();
	}

	private void Start ()
	{
		_audioSource = GetComponent<AudioSource>();
		_animator = GetComponent<Animator>();
		_battlefield = GameObject.Find("BattleField").transform;
	}

	protected virtual IEnumerator _attack ()
	{
		while ( IsAvailable )
		{
			Goose aim = GooseFabric.Instance.FindGoose(transform.position, Stats.Range);
			// null или далеко
			if ( aim == null || _spawnPoints == null )
			{
				yield return new WaitForSeconds(0.1f);
				continue;
			}

			_animator.SetTrigger("Shoot");
			var shotNumber = _spawnPoints.Count();
			foreach ( var spawnPoint in _spawnPoints )
			{
				yield return new WaitForSeconds(0.05f);
				aim = GooseFabric.Instance.FindGoose(transform.position, Stats.Range);
				if ( aim == null )
					break;

				// добавляю скрипт на префаб
				var projectile = GameObject.Instantiate(_projectilePrefab, spawnPoint.position, Quaternion.identity, _battlefield);
				Projectile proj = projectile.GetComponent<Projectile>();

				_audioSource.Play();
				proj.Loauch(spawnPoint.position, _predictTargetPosition(spawnPoint.position, aim), Stats.Projectile);
				yield return new WaitForSeconds(Stats.AttackDelay / shotNumber);
			}
			// может быть не нужен
			yield return new WaitForEndOfFrame();
		}

	}

	protected virtual Vector3 _predictTargetPosition (Vector3 from, Goose goose)
	{
		Vector3 target = goose.transform.position;
		float distance = Vector3.Distance(from, target);
		float u1 = Math.Abs(Stats.Projectile.Velocity);
		float u2 = Math.Abs(goose.Speed);
		float angle = Mathf.Deg2Rad * ( Vector3.Angle(from - target, goose.Movement) );
		float time = Mathf.Abs(( Mathf.Sqrt(2) * Mathf.Sqrt(2 * distance * distance * u1 * u1 + distance * distance * u2 * u2 * Mathf.Cos(2 * angle) - distance * distance * u2 * u2) - 2 * distance * u2 * Mathf.Cos(angle) ) / ( 2 * ( u1 * u1 - u2 * u2 ) ));

		Vector3 prediction = goose.Movement * time + new Vector3(-0.15f, 0.15f, 0);
		return target + prediction;
	}

}
