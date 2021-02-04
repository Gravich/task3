#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorWeapon : Weapon
{
    private Ray LookAtPoint;
    private RaycastHit _hit;

    void Update()
    {
        LookAtPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

            if (Physics.Raycast(LookAtPoint, out _hit))
            {
                var bot = _hit.collider.gameObject.GetComponent<AI>();
                if (bot)
                {
                    Shoot(MainCalibrePrefab, bot.transform);
                }
                else
                {
                    Shoot(MainCalibrePrefab, CamOrto.SharedLookPoint) ;
                }
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_shootElapsed <= 0)
            {
                if (Physics.Raycast(LookAtPoint, out _hit))
                {
                    var bot = _hit.collider.gameObject.GetComponent<AI>();
                    if (bot)
                    {
                        Shoot(SubCalibrePrefab, bot.transform);
                    }
                    else
                    {
                        Shoot(SubCalibrePrefab, CamOrto.SharedLookPoint);
                    }
                }
                _shootElapsed = 60 / ShootSpeed;
            }
            else
            {
                _shootElapsed -= Time.deltaTime;
            }
        }
    }
}
