using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ParticleDestroyBlast;
    [SerializeField]
    private TrailRenderer TracerEffect;
    

    [SerializeField]
    private float Speed;
    [SerializeField]
    private float DamageMin;
    [SerializeField]
    private float DamageMax;

    [SerializeField]
    private float MaxLifeTime;
    private float _lifeTime;

    private RaycastHit RayHit;
    private Vector3 OldPosition;
    private Vector3 Velocity;


    void Start()
    {
        _lifeTime = MaxLifeTime;
    }


    public void Fly(Vector3 shootPosition, Vector3 shootDestination)
    {
        Velocity = shootDestination * Speed;
        transform.position = shootPosition;
    }


    void Update()
    {
        if (_lifeTime > 0)
        {
            OldPosition = transform.position;
            transform.position += Velocity * Time.deltaTime;
            if (Physics.Linecast(OldPosition, transform.position, out RayHit))
            {
                var target = RayHit.collider.gameObject.GetComponent<Creature>();
                if (target)
                {
                    target.TakeDamage(Random.Range(DamageMin, DamageMax));
                }
                Destroy();
            }
            Debug.DrawLine(OldPosition, transform.position, Color.red, MaxLifeTime);
            _lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy();
        }

    }


    private void Destroy()
    {
        if (ParticleDestroyBlast)
        {
            Instantiate(ParticleDestroyBlast, transform.position, new Quaternion());
        }
        TracerEffect.transform.parent = null;
        Destroy(gameObject);
    }
}
