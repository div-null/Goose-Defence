using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GooseState
{
    stay = 0,                       //стоит
    walk,                           //идет
    run,                            //бежит
    atack,                          //атакует
    back                            //убегает
}

public class Goose : MonoBehaviour
{

    public int max_hp = 300;                  //кол-во стартового (максимального) здоровья у гуся
    public int cur_hp = 300;                  //текущее значение показателя здоровья

    public int goose_damage = 150;            //урон гуся
    public float goose_speed = 10f;           //скорость гуся

    public GooseState state;                  //состояние гуся

    //инициализатор гуся (уровень слож-ти, спрайт, трансформ-спавн)
    public void Initialize(int level)
    {
        max_hp = 300 * level;
        cur_hp = max_hp;
        goose_damage = 150 * level;
        goose_speed = 10f + level*1f;
    }

    //расчет характеристик в следствие эффектов
    public void OnEffect()
    {

    }

    //расчет урона
    public void OnDamage(int damage)
    {
        cur_hp -= damage;
        if (cur_hp < 0)
        {
            cur_hp = 0;
            Destroy(this.gameObject);
            GooseFabric.Instance.geese.Remove(this);
            //пополнение очков
        }
    }


    
}
