using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGoose : Goose, IBellHunter
{

	public override void Initialize (int lvl)
	{
		base.Initialize(lvl);
		GooseType = GooseKind.Boss;
		MaxHP = 150000;
		HP = MaxHP;
		Damage = 1000001;
		SpeedMultiplier = 1f + Level / 45;
		AttackSpeed = 3f - SpeedMultiplier / 2f;
		SpeedMultiplier = 0.5f;
	}

	public override Vector3 Walk (Vector3 direction)
	{
		//FIXME: босс должен находится над всеми гусями
		var velocity = base.Walk(direction);
		//velocity.z = -3f + Mathf.Abs(velocity.y / 10) - 4;
		return velocity;
	}

	public void TakeBell (BossBell bell)
	{
		StartCoroutine(_takeBell(bell));
	}

	IEnumerator _takeBell (BossBell bell)
	{
		State = GooseState.Attack;
		//Воспроизведение анимации атаки
		_animator.SetInteger("GooseState", (int)State);
		yield return new WaitForSeconds(AttackSpeed / 2);
		bell.GetDamage(Damage);
		yield return new WaitForSeconds(AttackSpeed / 2);

		State = GooseState.Walk;
		_animator.SetInteger("GooseState", (int)State);
		_animator.SetBool("WithBell", true);
	}
}
