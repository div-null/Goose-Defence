using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BossBell : Target
{
	public void Initialize (int _maxHp)
	{
		Type = TargetType.Human;
		MaxHP = _maxHp;
		HP = MaxHP;
	}

	public override void OnCollided (Collider collider)
	{
		base.OnCollided(collider);
		var goose = collider.gameObject.GetComponentInParent<Goose>();
		if ( goose == null )
			return;

		//если гусь может забрать колокол
		if ( goose is IBellHunter hunter )
		{
			hunter.TakeBell(this);
			return;
		}
		if ( goose.State != GooseState.Attack )
			goose.StartAttack(this);

	}

	// FIXME: исправить логику ваншота
	public override bool GetDamage (float dmg)
	{
		// если дамаг наносится малый, то игнор
		if ( dmg < 10000 )
			return true;
		// иначе ваншот
		base.GetDamage(dmg * 100);
		return IsDestroyed;
	}
}
