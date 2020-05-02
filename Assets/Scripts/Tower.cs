using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SocialPlatforms;

public class Tower : MonoBehaviour
{

    public GameObject SpawnPos;

    /// <summary>
    /// Скорость летящего снаряда
    /// </summary>
    [SerializeField]
    public float ProjectileSpeed;

    /// <summary>
    /// Точка спавна снарядов
    /// </summary>
    [SerializeField]
    Transform spawnPoint;
    /// <summary>
    /// Префаб снаряда
    /// </summary>
    [SerializeField]
    GameObject ProjectilePrefab;

    /// <summary>
    /// Время перезарядки
    /// </summary>
    [SerializeField]
    float AttackDelay = 2f;

    /// <summary>
    /// Урон снаряда
    /// </summary>
    [SerializeField]
    int Damage = 200;


    /// <summary>
    /// Радиус действия башни
    /// </summary>
    [SerializeField]
    public float AttackRange;

    public bool isAvailable { get; set; }
    public bool Destroyed
    {
        get
        {
            return HP > 0;
        }
    }

    float selfHp;
    float HP
    {
        get
        {
            return selfHp;
        }
        set
        {
            if (value < 0)
                selfHp = 0;
            else
                selfHp = value;
        }
    }

    public bool GetDamage(float dmg)
    {
        if (HP <= 0)
            return false;
        HP -= dmg;
        return Destroyed;
    }

    /// <summary>
    /// Создание башни, МГНОВЕННО начинает стрелять
    /// </summary>
    /// <param name="Hp">Здоровье</param>
    /// <param name="Dmg">Урон за атаку</param>
    /// <param name="DmgRate">Количество ударов в секудну</param>
    public void Initialize(float Hp, int Dmg, float DmgDelay)
    {
        HP = Hp;
        Damage = Dmg;
        AttackDelay = DmgDelay;
        spawnPoint = transform.Find("SpawnPoint");
    }

    Goose FindGoose()
    {
        float minDistance = AttackRange;
        Goose temp = null;
        foreach (var goose in GooseFabric.Instance.geese)
        {
            float distance = (goose.transform.position - transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                temp = goose;
            }
        }
        return temp;
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

    public IEnumerator Attack()
    {
        while (true)
        {
            Goose aim = FindGoose();

            // добавляю скрипт на префаб
            var projectile = GameObject.Instantiate(ProjectilePrefab);
            Projectile proj = projectile.GetComponent<Projectile>();
            proj.Loauch(spawnPoint.position, aim.transform.position, ProjectileSpeed, Damage);

            yield return new WaitForSeconds(AttackDelay);
            // может быть не нужен
            yield return new WaitForEndOfFrame();
        }
    }


    void Start()
    {
        // УБРАТЬ В ДАЛЬНЕЙШЕМ
        MakeDamage();
    }
}
