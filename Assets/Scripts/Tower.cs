using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class Tower : Target
{

	[SerializeField]
	public Animator Anim;

	AudioSource bangSound;

	[SerializeField]
	public GameObject SpawnPos;

	/// <summary>
	/// Точка спавна снарядов
	/// </summary>
	[SerializeField]
	List<Transform> spawnPoints;
	/// <summary>
	/// Префаб снаряда
	/// </summary>
	[SerializeField]
	GameObject ProjectilePrefab;

	[SerializeField]
	public TowerStatsList info = new TowerStatsList.TowerCabbageT1();

	public bool isAvailable { get; set; }

	public int TowerOrder;

	[SerializeField]
	private Transform Battlefield;

	private void Start ()
	{
		bangSound = GetComponent<AudioSource>();
		Anim = GetComponent<Animator>();
		Battlefield = GameObject.Find("BattleField").transform;
	}

	public void Initialize (TowerStatsList info, GameObject projectilePref, int order)
	{
		type = TargetType.Human;
		Anim = GetComponent<Animator>();
		spawnPoints = new List<Transform>();
		TowerOrder = order;
		this.info = info;
		maxHP = info.MaxHP;
		HP = info.MaxHP;

		ProjectilePrefab = projectilePref;

		// КОСТЫЛь
		Tower tower = GetComponentInChildren<Tower>();

		foreach ( var child in tower.GetComponentsInChildren<Transform>() )
			if ( child.tag == "FirePoint" )
				spawnPoints.Add(child);
	}

	public void MakeDamage ()
	{
		isAvailable = true;
		StartCoroutine("Attack");
	}

	public void StopDamage ()
	{
		StopCoroutine("Attack");
		isAvailable = false;
	}

	public IEnumerator Attack ()
	{
		while ( isAvailable )
		{
			Goose aim = GooseFabric.Instance.FindGoose(transform.position, info.Range);
			// null или далеко
			if ( aim == null || spawnPoints == null )
			{
				yield return new WaitForSeconds(0.1f);
				continue;
			}

			Anim.SetTrigger("Shoot");
			var shotNumber = spawnPoints.Count();
			foreach ( var spawnPoint in spawnPoints )
			{
				yield return new WaitForSeconds(0.05f);
				aim = GooseFabric.Instance.FindGoose(transform.position, info.Range);
				if ( aim == null )
					break;

				// добавляю скрипт на префаб
				var projectile = GameObject.Instantiate(ProjectilePrefab, spawnPoint.position, Quaternion.identity, Battlefield);
				Projectile proj = projectile.GetComponent<Projectile>();

				bangSound.Play();
				proj.Loauch(spawnPoint.position, predictTargetPosition(spawnPoint.position, aim), info.Projectile);
				yield return new WaitForSeconds(info.AttackDelay / shotNumber);
			}
			// может быть не нужен
			yield return new WaitForEndOfFrame();
		}

	}

	Vector3 predictTargetPosition (Vector3 from, Goose goose)
	{
		Vector3 target = goose.transform.position;
		float distance = Vector3.Distance(from, target);
		float u1 = Math.Abs(info.Projectile.Velocity);
		float u2 = Math.Abs(goose.Speed);
		float angle = Mathf.Deg2Rad * ( Vector3.Angle(from - target, goose.Movement) );
		float time = Mathf.Abs(( Mathf.Sqrt(2) * Mathf.Sqrt(2 * distance * distance * u1 * u1 + distance * distance * u2 * u2 * Mathf.Cos(2 * angle) - distance * distance * u2 * u2) - 2 * distance * u2 * Mathf.Cos(angle) ) / ( 2 * ( u1 * u1 - u2 * u2 ) ));

		Vector3 prediction = goose.Movement * time + new Vector3(-0.15f, 0.15f, 0);
		return target + prediction;
	}

	public override void OnCollided (Collider collider)
	{
		var goose = collider.gameObject.GetComponentInParent<Goose>();
		if ( goose == null )
			return;

		if ( goose.state != GooseState.atack )
			goose.startAttack(this);
	}

	public override void DestroySelf ()
	{
		HP = 0;
		StopCoroutine("Attack");
		base.DestroySelf();
	}
}