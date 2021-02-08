using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CutoffType {Auto, Single};
public class ActorWeapon : Weapon
{
    [SerializeField]
    private KeyCode ShootInput;
    [SerializeField]
    private CutoffType ShootType;


    protected override void Update()
    {
        base.Update();
        switch (ShootType)
        {
            case CutoffType.Auto:
                if (Input.GetKey(ShootInput))
                {
                    Shoot(CamOrto.SharedLookPoint);
                }
                break;

            case CutoffType.Single:
                if (Input.GetKeyDown(ShootInput))
                {
                    Shoot(CamOrto.SharedLookPoint);
                }
                break;
        }
    }
}
