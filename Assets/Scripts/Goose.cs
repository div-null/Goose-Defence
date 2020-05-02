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

    /// <summary>
    /// Хп гуся в зависимости от lvl
    /// </summary>
    static int[] GooseLvlHp = new int[3] { 250, 400, 650 };

    public int max_hp = 250;                  //кол-во стартового (максимального) здоровья у гуся
    int cur_hp = 250;                  //текущее значение показателя здоровья

    public int goose_damage = 100;            //урон гуся
    public float goose_speed = 10f;           //скорость гуся


    public GooseState state;                  //состояние гуся

    //инициализатор гуся (уровень слож-ти, спрайт, трансформ-спавн)
    public void Initialize(int Dmg, float SpdMul, int maxHp)
    {
        max_hp = maxHp;
        cur_hp = max_hp;
        goose_damage = Dmg;
        goose_speed *= SpdMul;
    }

    /// <summary>
    /// Инициализация через статы(ХП, Дамаг, Множитель скорости)
    /// </summary>
    /// <param name="stats"></param>
    public void Initialize(GooseTypeStats stats)
    {
        max_hp = stats.Hp;
        cur_hp = max_hp;
        goose_damage = stats.Damage;
        goose_speed *= stats.SpeedMultiplier;
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
