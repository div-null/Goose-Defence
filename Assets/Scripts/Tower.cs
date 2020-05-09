﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SocialPlatforms;
using System;

public delegate void TowerEvent(Tower tower);

public class Tower : MonoBehaviour
{
    /// <summary>
    /// Возникает при уничтожении
    /// </summary>
    public event TowerEvent TowerDestroyed;

    /// <summary>
    /// Возникает при получаении урона
    /// </summary>
    public event TowerEvent TowerDamaged;

    [SerializeField]
    public Animator Anim;

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
    //максимальное хм
    public int maxHP;
    

    public bool Destroyed
    {
        get
        {
            return HP > 0;
        }
    }

	[SerializeField]
	float selfHp = 100f;
	/// <summary>
	/// Хп Башни, вызывает событие уничтожения уничтожение
	/// </summary>
	[SerializeField]
	public float HP
    {
        get
        {
            return selfHp;
        }
        set
        {
            selfHp = value;
            TowerDamaged?.Invoke(this);
            if (selfHp <= 0)
            {
                selfHp = 0;
                TowerDestroyed?.Invoke(this);
            }
        }
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public bool GetDamage(float dmg)
    {
        if (HP <= 0)
            return false;
        HP -= dmg;
        return Destroyed;
    }

    public void Initialize(TowerStatsList info, GameObject projectilePref, int order)
    {
        Anim = GetComponent<Animator>();
        spawnPoints = new List<Transform>();
        TowerOrder = order;
        this.info = info;
	    maxHP = info.MaxHP;
		HP = (info.MaxHP);
        
        ProjectilePrefab = projectilePref;

        // КОСТЫЛь
        Tower tower = GetComponentInChildren<Tower>();

        foreach (var child in tower.GetComponentsInChildren<Transform>())
            if (child.tag == "FirePoint")
                spawnPoints.Add(child);
    }

    public void MakeDamage()
    {
        StartCoroutine("Attack");
        isAvailable = true;
    }

    public void StopDamage()
    {
        StopCoroutine("Attack");
        isAvailable = false;
    }
    AudioSource bangSound;
    public IEnumerator Attack()
    {

        while (true)
        {
            Goose aim = GooseFabric.Instance.FindGoose(transform.position, info.Range);
            // null или далеко
            if (aim == null || spawnPoints == null)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            foreach (var spawnPoint in spawnPoints)
            {
                Anim.SetTrigger("Shoot");
                yield return new WaitForSeconds(0.05f);
                // добавляю скрипт на префаб
                var projectile = GameObject.Instantiate(ProjectilePrefab, spawnPoint.position, Quaternion.identity);
                Projectile proj = projectile.GetComponent<Projectile>();

                float distance = Vector3.Distance(spawnPoint.position, aim.transform.position);
                float u1 = Math.Abs(info.Projectile.Velocity);
                float u2 = Math.Abs(aim.goose_speed);
			    float angle = Mathf.Deg2Rad * (Vector3.Angle(spawnPoint.position - aim.transform.position, aim.Movement));
			    float time = Mathf.Abs((Mathf.Sqrt(2) * Mathf.Sqrt(2 * distance * distance * u1 * u1 + distance * distance * u2 * u2 * Mathf.Cos(2 * angle) - distance * distance * u2 * u2) - 2 * distance * u2 * Mathf.Cos(angle)) / (2 * (u1 * u1 - u2 * u2)));

                Vector3 prediction = aim.Movement * time + new Vector3(-0.15f, 0.15f, 0);
                //Debug.Log($"Angle = {angle} Time = {time}");
                bangSound = GetComponent<AudioSource>();
                bangSound.Play();
                proj.Loauch(spawnPoint.position, aim.transform.position + prediction, info.Projectile);
            }
            yield return new WaitForSeconds(info.AttackDelay);
            // может быть не нужен
            yield return new WaitForEndOfFrame();
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        var goose = other.gameObject.GetComponentInParent<Goose>();
		if (goose == null) return;
        //если  коснулся босс колокола
        if (goose.typeGoose == 0 && gameObject.CompareTag("Bell"))
        {
            //то воспроизводим атаку
            goose.StartCoroutine(goose.BellAttack(this));
            goose.animator.SetBool("WithBell", true);
            goose.animator.SetInteger("GooseState", 1);
            goose.transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            if (goose == null)
                return;

            if (goose.state != GooseState.atack)
                goose.startAttack(this);
        }
    }

    public void RemoveTower()
    {
        StopCoroutine("Attack");
        this.enabled = false;
        GameObject.Destroy(gameObject);
    }
}
