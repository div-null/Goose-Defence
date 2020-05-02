using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Радиус дамага
    /// </summary>
    [SerializeField]
    public float Radius = 2f;

    [SerializeField]
    public int Damage;

    /// <summary>
    /// Скорость снаряда
    /// </summary>
    [SerializeField]
    public float Velocity;

    /// <summary>
    /// Время до уничтожения лежащего объекта
    /// </summary>
    [SerializeField]
    public float DestroyTime;

    Vector3 Direction;

    float RemainTime;

    public void Loauch(Vector3 tower, Vector3 point, ProjectileStats stats)
    {
        Damage = stats.Damage;
        this.Velocity = stats.Velocity;
        transform.position = tower;
        Direction = (point - tower).normalized;
        RemainTime = (Vector3.Distance(tower, point) / stats.Velocity);
        Radius = stats.ExplosionRange;
        //TODO: добавить поворот
        transform.rotation = Quaternion.LookRotation(Vector3.back);
    }

    void MakeDamage()
    {
        Vector2 pos = transform.position;
        // TODO: Вызов метода дамага гусей
        GooseFabric.Instance.OnAttack(Radius, pos, Damage);
        Debug.Log("Destroy ball");
        GameObject.Destroy(gameObject, DestroyTime);
        this.enabled = false;
    }

    
    void FixedUpdate()
    {
        if (RemainTime > 0)
        {
            transform.Rotate(0, 0, 10f * Time.deltaTime);                       //вращение снаряда        
            transform.position += Direction * Velocity * Time.deltaTime;
            RemainTime -= Time.deltaTime;
        }
        else
            MakeDamage();
    }
}
