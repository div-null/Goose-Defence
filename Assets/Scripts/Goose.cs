﻿using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

public enum GooseState
{
    stay = 0,                       //стоит
    walk,                           //идет
    run,                            //бежит
    atack,                          //атакует
    death                            //умирает
}

public class Goose : MonoBehaviour
{

    /// <summary>
    /// Хп гуся в зависимости от lvl
    /// </summary>
    static int[] GooseLvlHp = new int[3] { 250, 400, 650 };

    public int max_hp;                  //кол-во стартового (максимального) здоровья у гуся
    [SerializeField]
    int cur_hp;                  //текущее значение показателя здоровья

	public int gooseLvl;
    public int goose_damage;            //урон гуся
    public float goose_speed = 3;           //скорость гуся
	public float speed_multiplier;		//множитель ускорения
	public float attack_speed;		//множитель ускорения
	
	public GooseState state;                //состояние гуся
    public Animator animator;                   //аниматор
	public int typeGoose;

    public Vector3 Movement;

	public void Initialize(int lvl)
	{
		gooseLvl = lvl;
		state = GooseState.stay;

		int tmp = (int)((gooseLvl / 25f) / Mathf.Sqrt(1 + Mathf.Pow(gooseLvl / 25f, 2)) * 50);
		int typeTmp = gooseLvl / 10;
		if (typeTmp==0)
		{
			typeTmp = Random.Range(1, 10) * (gooseLvl % 10);
			typeTmp = typeTmp < 50 ? 0 : 1;
		}
		else if(typeTmp == 1) {
			typeTmp = Random.Range(1, 10) * (gooseLvl % 10);
			typeTmp = typeTmp < 50 ? 1 : 2;
		}
		else if(typeTmp == 2) {
			typeTmp = Random.Range(1, 10) * (gooseLvl % 10);
			typeTmp = typeTmp < 50 ? 2 : 3;
		}
		else {
			typeTmp = 4;
		}
		typeGoose = typeTmp;	
		
		max_hp = tmp * 250;
		if (typeTmp == 4)
			max_hp = tmp * 250 * 10;

		cur_hp = max_hp;
		goose_damage = (int)(max_hp / 2.5);

		speed_multiplier = 1 + gooseLvl / 25;
		//Тут надо попроавить:
		attack_speed = 2-speed_multiplier/2;

	}

    public void startAttack(Tower tower)
    {
        StartCoroutine(Attack(tower));
    }

    IEnumerator Attack(Tower tower)
    {
        state = GooseState.atack;
        
       
        while (true)
        {
			//Небольшой разброс дамага
			int tmpGooseDamage = goose_damage + (int)(Random.Range(-0.1f * goose_damage, 0.1f * goose_damage));

            //TowerFabric.Instance.TryDamageTower(TowerNumber, goose_damage);

            //Воспроизведение анимации атаки
            animator.SetInteger("GooseState", 3);   

            //Нанесение урона
            tower.GetDamage(tmpGooseDamage);
            yield return new WaitForSeconds(attack_speed);
            animator.SetInteger("GooseState", 0);       //attack
                                                        // animator.SetTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        var position = TowerFabric.Instance.FindNearTower(transform.position) - new Vector3(0, 0,0.5f);
        var direction = (position - transform.position);
        if (direction.magnitude > 0.1 && state !=GooseState.atack && state != GooseState.death)
        {
            Movement = direction.normalized * goose_speed * speed_multiplier;

            Movement.z = -3f+Mathf.Abs(Movement.y / 10);
            state = GooseState.walk;
            transform.position += direction.normalized * goose_speed  * speed_multiplier * Time.deltaTime;
            //воспроизведение анимации ходьбы
            animator.SetInteger("GooseState", 1);
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
		speed_multiplier = (1 + gooseLvl / 25) * coefSlow;
		attack_speed = 2 - speed_multiplier / 2;
		yield return new WaitForSeconds(timeSlow);
		speed_multiplier = 1 + gooseLvl / 25;
		//Тут надо попроавить:
		attack_speed = 2 - speed_multiplier / 2;
	}

    private void OnCollisionEnter(Collision other)
    {
        if (state != GooseState.atack)
            StartCoroutine("Attack");
    }

    //Наносит урон гусю
    public void OnDamage(int damage, float coefSlow = 1, float timeSlow = 0)
    {
		if (timeSlow != 0)
		{
			StopCoroutine("SlowDown");
			StartCoroutine(SlowDown(coefSlow, timeSlow));
		}
			
        cur_hp -= damage;
        if (cur_hp < 0)
        {
            cur_hp = 0;
            state = GooseState.death;
            //воспроизведение анимации
            StartCoroutine("OnDeath");
           
        }
    }
    IEnumerator OnDeath()
    {
        animator.SetInteger("GooseState", 4);   //death
        yield return new WaitForSeconds(1.3f);
        Destroy(this.gameObject);
        GooseFabric.Instance.geese.Remove(this);
    }
    void Start()
    {
        //state = GooseState.walk;
        animator = transform.GetComponentInChildren<Animator>();
    }

}