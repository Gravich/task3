using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Creature : MonoBehaviour
{

    [HideInInspector]
    public Vector3 RespawnPoint;
    protected NavMeshAgent Agent;

    [Header("Base config")]
    [SerializeField]
    private ParticleSystem ParticleDeathlyBlast;
    [SerializeField]
    private ParticleSystem ParticleShieldAttacked;
    [SerializeField]
    private ParticleSystem ParticleShieldDestroy;

    [SerializeField]
    private float ShieldDefault;
    [SerializeField]
    private float ShieldRegenSpeed;
    private float ShieldCurrent;


    [SerializeField]
    private float HPDefault;
    private float HPCurrent;

    protected virtual void Start()
    {
        RespawnPoint = transform.position;
        HPCurrent = HPDefault;
        ShieldCurrent = ShieldDefault;
        Agent = GetComponent<NavMeshAgent>();
    }


    private void RegenerateShield()
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
                Die();
            }
        }
        else
        {
            ShieldCurrent -= DMG;
            if (ParticleShieldAttacked && ParticleShieldDestroy) 
            {
                Instantiate(ParticleShieldAttacked, transform);
                if (ShieldCurrent <= 0)
                {
                    Instantiate(ParticleShieldDestroy, transform.position, new Quaternion());
                }
            }
        }
    }


    protected void Die()
    {
        Instantiate(ParticleDeathlyBlast, transform.position, new Quaternion());
        SpawnManager.Instanse.Respawn(this);
    }


    public virtual object RespawnData { get; set; }
}
