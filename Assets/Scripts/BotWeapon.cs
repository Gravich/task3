#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotWeapon : Weapon
{
    public override void Shoot(Transform _target)
    {
        if (_shootElapsed <= 0)
        {
            base.Shoot(SubCalibrePrefab, _target);
            _shootElapsed = 60 / ShootSpeed;
        }
        else
        {
            _shootElapsed -= Time.deltaTime;
        }
    }
}
