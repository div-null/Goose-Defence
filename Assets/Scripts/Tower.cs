using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SocialPlatforms;

public delegate void TowerEvent(Tower tower);

public class Tower : MonoBehaviour
{
    public event TowerEvent TowerDestroyed;
    
    [SerializeField]
    public Animator Anim;

    [SerializeField]
    public GameObject SpawnPos;

    /// <summary>
    /// Уровень башни
    /// </summary>
    public TowerLevel Level;

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

    ProjectileStats projectileStats;

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

    float selfHp = 100f;
    /// <summary>
    /// Хп Башни, вызывает событие уничтожения уничтожение
    /// </summary>
    float HP
    {
        get
        {
            return selfHp;
        }
        set
        {
            if (value <= 0)
            {
                selfHp = 0;
                TowerDestroyed?.Invoke(this);
            }
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

    public void Initialize(TowerStats stats, GameObject projectilePref)
    {
        HP = stats.HP;
        Damage = stats.Projectile.Damage;
        AttackDelay = stats.AttackDelay;
        AttackRange = stats.Range;
        projectileStats = stats.Projectile;
        spawnPoint = transform.Find("SpawnPoint");
        ProjectilePrefab = projectilePref;
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


    private void Awake()
    {
        spawnPoint = transform.Find("spawn_point");
    }
    public IEnumerator Attack()
    {
        while (true)
        {
            Goose aim = GooseFabric.Instance.FindGoose(transform.position, AttackRange);
            // null или далеко
            if (aim == null)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            // добавляю скрипт на префаб
            var projectile = GameObject.Instantiate(ProjectilePrefab);
            Projectile proj = projectile.GetComponent<Projectile>();
            proj.Loauch(spawnPoint.position, aim.transform.position, projectileStats);

            yield return new WaitForSeconds(AttackDelay);
            // может быть не нужен
            yield return new WaitForEndOfFrame();
        }
    }

    public void RemoveTower()
    {
        StopCoroutine("Attack");
        this.enabled = false;
        GameObject.Destroy(this);
    }
}
