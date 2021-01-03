using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BossBell : Target
{
	public void Initialize(int _maxHp)
	{
		type = TargetType.Human;
		maxHP = _maxHp;
		HP = maxHP;
	}

	public override void OnCollided(Collider collider)
	{
		base.OnCollided(collider);
		var goose = collider.gameObject.GetComponentInParent<Goose>();
		if (goose == null)
			return;

		//если босс коснулся колокола
		if (goose.typeGoose == GooseKind.Boss)
		{
			//то воспроизводим ваншот атаку
			goose.StartCoroutine(goose.TakeBell(this));
		}
		else
		{
			if (goose.state != GooseState.atack)
				goose.startAttack(this);
		}
	}

	public override bool GetDamage(float dmg)
	{
		// если дамаг наносится малый, то игнор
		if (dmg < 10000)
			return true;
		// иначе ваншот
		base.GetDamage(dmg * 100);
		return isDestroyed;
	}
}
