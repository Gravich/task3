using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Bullet Ammo;

    [SerializeField]
    protected Transform ShootPos;

    [SerializeField]
    protected float ShootSpeed;
    private float _shootElapsed;


    protected virtual void Update()
    {
        if (_shootElapsed > 0)
        {
            _shootElapsed -= Time.deltaTime;
        }
    }


    public virtual void Shoot(Vector3 _target)
    {
        if (_shootElapsed <= 0 && Ammo && ShootPos)
        {
            var bullet = Instantiate(Ammo);
            bullet.Fly(ShootPos.position, Vector3.Normalize((ShootPos.position - _target) * (-1)));
            _shootElapsed = 60 / ShootSpeed;
        }
    }
}