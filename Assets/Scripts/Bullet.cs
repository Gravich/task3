#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem blast;
    [SerializeField]
    private TrailRenderer Tracer;
    [SerializeField]
    private bool isSuperCanon;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _balisticsCoeff = 0.234f;
    [SerializeField]
    private float _bulletWeight = 3.6f;
    [SerializeField]
    private float _maxLifeTime = 3.0f;
    private float _lifeTime;
    private const float _balisticsConst = 0.0052834f;

    private RaycastHit _rayHit;
    private Vector3 _oldPosition;
    private Vector3 _velocity;

    void Start()
    {
        _lifeTime = _maxLifeTime;
    }

    public void Shoot(Vector3 shootPosition, Vector3 shootDirection)
    {
            _velocity = shootDirection * _speed;
            transform.position = shootPosition;
    }


    void Update()
    {
        if (_lifeTime > 0)
        {
            _oldPosition = transform.position;
            transform.position += _velocity * Time.deltaTime;

            // Баллистика
            var deltaPos = (transform.position - _oldPosition).magnitude;
            var m = (_balisticsConst * deltaPos) / _balisticsCoeff;
            _velocity = Mathf.Pow(-m + Mathf.Sqrt(_velocity.magnitude), 2) * _velocity.normalized;
            _velocity += Physics.gravity * Time.deltaTime;
            //


            if (Physics.Linecast(_oldPosition, transform.position, out _rayHit))
            {
                var target = _rayHit.collider.gameObject.GetComponent<UnimmortallObject>();
                if (target)
                {
                    var velocityMag = _velocity.magnitude;
                    var energy = (_bulletWeight / 100.0f) * velocityMag * velocityMag / 2.0f;
                    target.TakeDamage(energy / 10);
                }
                Instantiate(blast, transform.position, new Quaternion());
                if (isSuperCanon) Tracer.transform.parent = null;
                Destroy(this.gameObject);
            }

            Debug.DrawLine(_oldPosition, transform.position, Color.red, _maxLifeTime);
            _lifeTime -= Time.deltaTime;
        }
        else
        {
            Instantiate(blast, transform.position, new Quaternion());
            if (isSuperCanon) Tracer.transform.parent = null;
            Destroy(this.gameObject);
        }

    }
}
