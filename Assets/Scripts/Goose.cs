using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GooseState
{
	stay = 0,                       //стоит
	walk,                           //идет
	run,                            //бежит
	atack,                          //атакует
	death                            //умирает
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
	public int Level;
	public int Damage;            //урон гуся
	public float Speed = 1.5f;           //скорость гуся
	public float SpeedMultiplier;       //множитель ускорения
	public float AttackSpeed;       //множитель ускорения

	public GooseState state;                //состояние гуся
	public Animator animator;             //аниматор
	public GooseKind typeGoose;

	public Vector3 Movement;
	Target aim;
	AudioSource audio;

	void Start()
	{
		animator = transform.GetComponentInChildren<Animator>();
		audio = GetComponentInChildren<AudioSource>();
	}

	public void Initialize(int lvl)
	{
		Level = lvl;
		state = GooseState.stay;

		//int tmp = (int)((gooseLvl / 25f) / Mathf.Sqrt(1 + Mathf.Pow(gooseLvl / 25f, 2)) * 50);

		int gooseGrade = Level / 10;
		int kindSpread = Random.Range(1, 10) * (Level % 10);
		switch (gooseGrade)
		{
			case 0:
				typeGoose = kindSpread < 50 ? GooseKind.White : GooseKind.Gray;
				break;

			case 1:
				typeGoose = kindSpread < 50 ? GooseKind.Gray : GooseKind.Brown;
				break;

			case 2:
				typeGoose = kindSpread < 50 ? GooseKind.Brown : GooseKind.Black;
				break;
			default:
				typeGoose = GooseKind.Boss;
				break;
		}

		maxHP = (int)((Level / 5f + 1f) * 250f);
		HP = maxHP;
		Damage = (int)(maxHP / 2.5);

		SpeedMultiplier = 1f + Level / 25f;

		//Тут надо попроавить:
		AttackSpeed = 2f - SpeedMultiplier / 2f;

		if (typeGoose == GooseKind.Boss)
		{
			maxHP = 150000;
			HP = maxHP;
			Damage = 1000001;
			SpeedMultiplier = 1f + Level / 45;
			AttackSpeed = 3f - SpeedMultiplier / 2f;
		}
		findTarget(null);
	}


	public void startAttack(Target target)
	{
		StartCoroutine(Attack(target));
	}

	IEnumerator Attack(Target target)
	{
		state = GooseState.atack;
		while (true)
		{
			if (target == null || target.isDestroyed) break;
			audio.Play();
			//Небольшой разброс дамага
			int tmpGooseDamage = Damage + (int)(Random.Range(-0.1f * Damage, 0.1f * Damage));

			//TowerFabric.Instance.TryDamageTower(TowerNumber, goose_damage);

			//Воспроизведение анимации атаки
			animator.SetInteger("GooseState", 3);

			yield return new WaitForSeconds(AttackSpeed / 2);
			target.GetDamage(tmpGooseDamage);  //Нанесение урона в середине анимации
			yield return new WaitForSeconds(AttackSpeed / 2);

			animator.SetInteger("GooseState", 0);
		}
		state = GooseState.walk;
	}

	public IEnumerator TakeBell(BossBell bell)
	{
		state = GooseState.atack;
		//TODO: выяснить как сократить задержку
		//Воспроизведение анимации атаки
		animator.SetInteger("GooseState", 3);
		yield return new WaitForSeconds(AttackSpeed / 2);
		bell.GetDamage(Damage);
		yield return new WaitForSeconds(AttackSpeed / 2);

		animator.SetInteger("GooseState", 1);
		animator.SetBool("WithBell", true);
		state = GooseState.walk;
	}

	void findTarget(Target target)
	{
		// если ранее цель была установлена
		if (aim != null)
			aim.Destroyed -= findTarget;

		//нахожу новую цель
		if (target == null || target.isDestroyed)
			aim = TowerFabric.Instance.findNearTarget(transform.position);
		else
			aim = target;
		aim.Destroyed += findTarget;
	}

	void FixedUpdate()
	{
		if ( aim == null )
			return;
		var position = aim.transform.position - new Vector3(0, 0, 0.5f);
		var direction = (position - transform.position);
		if (direction.magnitude > 0.1 && state != GooseState.atack && state != GooseState.death)
		{
			// поворот гуся

			transform.rotation = Quaternion.Euler(0, direction.x < 0 ? 0 : 180, 0);

			Movement = direction.normalized * Speed * SpeedMultiplier;

			Movement.z = -3f + Mathf.Abs(Movement.y / 10);
			state = GooseState.walk;
			//воспроизведение анимации ходьбы        
			animator.SetInteger("GooseState", 1);
			animator.speed = SpeedMultiplier;
			if (typeGoose == GooseKind.Boss)
			{
				animator.speed = 0.5f;
				Movement.z = -3f + Mathf.Abs(Movement.y / 10) - 4;
			}
			transform.position += direction.normalized * Speed * SpeedMultiplier * Time.deltaTime;
		}
		else
		{
			Movement = Vector3.zero;

			//state = GooseState.stay;
			//воспроизведение idle
			animator.SetInteger("GooseState", 0);
		}
	}

	IEnumerator SlowDown(float coefSlow = 1, float timeSlow = 0)
	{
		SpeedMultiplier = (1 + Level / 25) * coefSlow;
		AttackSpeed = 2 - SpeedMultiplier / 2;
		yield return new WaitForSeconds(timeSlow);
		SpeedMultiplier = 1 + Level / 25;
		//Тут надо попроавить:
		AttackSpeed = 2 - SpeedMultiplier / 2;
	}

	//Наносит урон гусю
	public bool GetDamage(float dmg, float coefSlow = 1, float timeSlow = 0)
	{
		base.GetDamage(dmg);
		if (timeSlow != 0)
		{
			StopCoroutine(SlowDown());
			StartCoroutine(SlowDown(coefSlow, timeSlow));
		}
		if (isDestroyed && state != GooseState.death)
		{
			state = GooseState.death;
			StopAllCoroutines();
			//воспроизведение анимации
			StartCoroutine(OnDeath());
		}
		return isDestroyed;
	}

	IEnumerator OnDeath()
	{
		animator.speed = 1;
		if ( aim != null )
			aim.Destroyed -= findTarget;
		GooseFabric.Instance.geese.Remove(this);
		animator.SetInteger("GooseState", 4);   //death
		state = GooseState.death;
		yield return new WaitForSeconds(0.9f / SpeedMultiplier);
		Destroy(this.gameObject);
	}
}
