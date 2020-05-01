using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tower : MonoBehaviour
{

    public GameObject SpawnPos;

    /// <summary>
    /// Скорость летящего снаряда
    /// </summary>
    [SerializeField]
    public float ProjectileSpeed;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    GameObject ProjectilePrefab;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    float AttackDelay = 2f;

    [SerializeField]
    float Damage = 200f;

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
    /// 
    /// </summary>
    /// <param name="Hp">Здоровье</param>
    /// <param name="Dmg">Урон за атаку</param>
    /// <param name="DmgRate">Количество ударов в секудну</param>
    public void Initialize(float Hp, float Dmg, float DmgRate)
    {
        HP = Hp;
        Damage = Dmg;
        AttackDelay = DmgRate;
        spawnPoint = transform.Find("SpawnPoint");
    }

    Goose FindGoose()
    {
        float minDistance = 9999;
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
    }

    public void StopDamage()
    {
        StopCoroutine("Attack");
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            Goose aim = FindGoose();

            // добавляю скрипт на префаб
            var projectile = GameObject.Instantiate(ProjectilePrefab);
            Projectile proj = projectile.GetComponent<Projectile>();
            proj.Loauch(spawnPoint.position, SpawnPos.transform.position, ProjectileSpeed);
            //proj.Loauch(spawnPoint.position, aim.transform.position);

            yield return new WaitForSeconds(AttackDelay);
            //if (aim.Hp>0)
            //    aim.getDamage(Damage);
            yield return new WaitForEndOfFrame();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeDamage();
    }
}
