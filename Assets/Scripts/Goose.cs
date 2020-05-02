using System.Collections;
using System.Collections.Generic;
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
    int cur_hp;                  //текущее значение показателя здоровья

	public int gooseLvl;
    public int goose_damage;            //урон гуся
    public float goose_speed = 3;           //скорость гуся
	public float speed_multiplier;		//множитель ускорения
	public float attack_speed;		//множитель ускорения
	
	public GooseState state;                //состояние гуся
    public Animator animator;                   //аниматор
	public int typeGoose;
    
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

    IEnumerator Attack()
    {
        state = GooseState.atack;
        while (true)
        {
			//Небольшой разброс дамага
			int tmpGooseDamage = goose_damage + (int)(Random.Range(-0.1f * goose_damage, 0.1f * goose_damage));

            //TowerFabric.Instance.TryDamageTower(TowerNumber, goose_damage);
            // <- ВЫЗОВ АНИМАЦИИ
            yield return new WaitForSeconds(attack_speed);
        }
    }

    void FixedUpdate()
    {
        var position = TowerFabric.Instance.FindNearTower(transform.position);
        var direction = (position - transform.position);
        if (direction != Vector3.zero)
        {
            state = GooseState.walk;
            transform.position += direction.normalized * goose_speed * Time.deltaTime;
        }
        else
            state = GooseState.atack;
    }

    //расчет характеристик в следствие эффектов
    public void OnEffect()
    {
        
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

    void OnCollisionEnter(Collision collision)
    {
        if (state != GooseState.atack) 
            StartCoroutine("Attack");
    }

    //Наносит урон гусю
    public void OnDamage(int damage, float coefSlow = 1, float timeSlow = 0)
    {
		if (timeSlow != 0)
			StartCoroutine(SlowDown(coefSlow, timeSlow));
        cur_hp -= damage;
        if (cur_hp < 0)
        {
            cur_hp = 0;
            state = GooseState.death;
            //Destroy(this.gameObject);
            GooseFabric.Instance.geese.Remove(this);
        }
    }

    void Start()
    {
        //state = GooseState.walk;
        animator = GetComponent<Animator>();
    }

}
