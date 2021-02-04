#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Bullet MainCalibrePrefab;
    [SerializeField]
    protected Bullet SubCalibrePrefab;
    [SerializeField]
    protected Transform ShootPos;
    [SerializeField]
    protected float ShootSpeed;
    protected float _shootElapsed;

    public virtual void Shoot(Bullet _calibre, Transform _target)
    {
        var bullet = Instantiate(_calibre);
        bullet.Shoot(ShootPos.position, Vector3.Normalize((ShootPos.position - _target.position) * (-1)));
    }

    /// <summary>
    /// По дефолту пуляет подкалиберными
    /// </summary>
    /// <param name="_target">в кого шмалять-с</param>
    public virtual void Shoot(Transform _target)
    {
        var bullet = Instantiate(SubCalibrePrefab);
        bullet.Shoot(ShootPos.position, Vector3.Normalize((ShootPos.position - _target.position) * (-1)));
    }
}