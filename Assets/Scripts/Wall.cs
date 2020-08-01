using UnityEngine;
using System.Collections.Generic;

public class Wall : Target
{
	/// <summary>
	/// Хп стены
	/// </summary>
	public ProgressBar hpBar;

	/// <summary>
	/// Спрайты стены
	/// </summary>
	public List<Sprite> WallSprites;

	/// <summary>
	/// Префаб полоски здоровья
	/// </summary>
	[SerializeField]
	GameObject hpBarPrefab;

	public void Initialize(int _maxHp)
	{
		maxHP = _maxHp;
		HP = maxHP;
		var hpBarObj = GameObject.Instantiate(hpBarPrefab, new Vector3(-2f, 10f, -6f), Quaternion.identity);
		hpBarObj.transform.SetParent(transform);
		hpBar = hpBarObj.GetComponent<ProgressBar>();
		hpBar.Initialize(HP);
		Damaged += getDamage;

		hpBar.Hp = HP;
	}

	private void getDamage(Target obj)
	{
		hpBar.Hp = HP;
		if (HP == maxHP)
		{
			GetComponentInChildren<SpriteRenderer>().sprite = WallSprites[0];
		}
		else if (HP < maxHP / 2)
		{
			GetComponentInChildren<SpriteRenderer>().sprite = WallSprites[1];
		}
		if (HP <= 0)
		{
			GetComponentInChildren<SpriteRenderer>().sprite = WallSprites[2];
			hpBar.Destroy();
		}
	}

	public override bool GetDamage(float dmg)
	{
		return base.GetDamage(dmg);
	}

	public override void DestroySelf()
	{
		Destroy(hpBar.gameObject);
		this.enabled = false;
	}

	public override void OnCollided(Collider collider)
	{
		base.OnCollided(collider);
		var goose = collider.gameObject.GetComponentInParent<Goose>();
		if (goose == null)
			return;

		if (goose.state != GooseState.atack)
			goose.startAttack(this);
	}
}
