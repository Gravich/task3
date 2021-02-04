#pragma warning disable 649
#pragma warning disable 618
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnimmortallObject : MonoBehaviour
{
    public static ActorController Actor;
    [HideInInspector]
    public Vector3 RespawnPoint;

    [SerializeField]
    private ParticleSystem blast;
    [SerializeField]
    private ParticleSystem ShieldAttacked;
    [SerializeField]
    private ParticleSystem ShieldDestroy;
    [SerializeField]
    private Respawner RespawnManager;
    [SerializeField]

    private float ShieldDefault;
    [SerializeField]
    private float ShieldCurrent;
    [SerializeField]
    private float ShieldRegenSpeed;
    [SerializeField]
    private float HPDefault;
    [SerializeField]
    private float HPCurrent;

    protected virtual void Start()
    {
        RespawnPoint = transform.position;
        HPCurrent = HPDefault;
        ShieldCurrent = ShieldDefault;
    }


    protected void RegenerateShield()
    {
        if (ShieldCurrent < ShieldDefault)
        {
            ShieldCurrent += ShieldRegenSpeed*Time.deltaTime;
        }
    }


    protected virtual void Update()
    {
        RegenerateShield();
    }


    public virtual void TakeDamage(float DMG)
    {
        if (ShieldCurrent <=0)
        {
            HPCurrent -= DMG;
            if (HPCurrent<0)
            {
                ToDeath();
            }
        }
        else
        {
            ShieldCurrent -= DMG;
            var shieldEffect = Instantiate(ShieldAttacked, transform.position, new Quaternion());
            shieldEffect.startColor = new Color(1, ShieldCurrent / ShieldDefault, ShieldCurrent / ShieldDefault);
            shieldEffect.subEmitters.GetSubEmitterSystem(0).startColor = new Color(1, ShieldCurrent / ShieldDefault, ShieldCurrent / ShieldDefault);
            if (ShieldCurrent <= 0)
            {
                Instantiate(ShieldDestroy, transform.position, new Quaternion());
            }
        }
    }


    protected void ToDeath()
    {
        Instantiate(blast, transform.position, new Quaternion());
        Respawner.Instanse.Respawn(this);
    }
}
