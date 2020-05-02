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

    public int max_hp = 250;                  //кол-во стартового (максимального) здоровья у гуся
    int cur_hp = 250;                  //текущее значение показателя здоровья

    public int goose_damage = 100;            //урон гуся
    public float goose_speed = 3f;           //скорость гуся
    public float speed_multiplier = 1;
    /// <summary>
    /// Номер башни, которую атакует гусь
    /// </summary>
    public int TowerNumber;
    public GooseState state;                  //состояние гуся
    public Animator animator;                   //аниматор

    //инициализатор гуся (уровень слож-ти, спрайт, трансформ-спавн)
    public void Initialize(int Dmg, float SpdMul, int maxHp)
    {
        max_hp = maxHp;
        cur_hp = max_hp;
        goose_damage = Dmg;
        goose_speed *= SpdMul;
        state = GooseState.stay;
    }
   
    /// <summary>
    /// Инициализация через статы(ХП, Дамаг, Множитель скорости)
    /// </summary>
    /// <param name="stats"></param>
    public void Initialize(GooseTypeStats stats, int towerNumber)
    {
        max_hp = stats.Hp;
        cur_hp = max_hp;
        goose_damage = stats.Damage;
        goose_speed *= stats.SpeedMultiplier;
        this.speed_multiplier = stats.SpeedMultiplier;
    }

    IEnumerator Attack()
    {
        state = GooseState.atack;
        while (true)
        {
            TowerFabric.Instance.TryDamageTower(TowerNumber, goose_damage);
            // <- ВЫЗОВ АНИМАЦИИ
            yield return new WaitForSeconds(2f / goose_speed);
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

    void OnCollisionEnter(Collision collision)
    {
        if (state != GooseState.atack) 
            StartCoroutine("Attack");
    }

    //расчет урона
    public void OnDamage(int damage)
    {
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


    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            state = 0;
            animator.SetInteger("GooseState", 0);
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.SetInteger("GooseState", 1);

        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            animator.SetInteger("GooseState", 3);

        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            animator.SetInteger("GooseState", 3);
            animator.SetInteger("GooseState", 4);
        }
    }
}
