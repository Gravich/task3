using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AI : Creature
{
    [Header("AI Config")]
    [SerializeField]
    protected Weapon Canon;
    [SerializeField]
    private float FOV;
    [SerializeField]
    private float AgroRange;

    [SerializeField]
    private List<Transform> WayPoints;
    private short CurrentWayPoint;

    [SerializeField]
    private float AgroTimerDefault;
    [SerializeField]
    private float ForvardingRange;
    private float AgroTimer;
    private bool isAgro;

    protected override void Start()
    {
        base.Start();
        //устанавливаем первую вейпоинт. «а нее берем исходную позицию бота на карте
        if (WayPoints == null)
        {
            WayPoints = new List<Transform>();
        }
        GameObject basePos = new GameObject();
        basePos.transform.position = transform.position;
        WayPoints.Add(basePos.transform);
        CurrentWayPoint = 0;
    }


    protected override void Update()
    {
        base.Update();
        Walk();
        AlarmCheck();
    }


    void Walk()
    {
        if (isAgro)
        {
            if (AgroTimer > 0)
            {
                if (ActorController.Actor && Vector3.Distance(transform.position, ActorController.Actor.transform.position) > ForvardingRange)
                {
                    Agent.destination = ActorController.Actor.transform.position;
                }
                else
                {
                    Agent.destination = transform.position;
                }
            }
            else
            {
                SelectWayPoint();
            }
        }
        else
        {
            if (!Agent.hasPath)
            {
                SelectWayPoint();
            }
        }
    }


    private void SelectWayPoint()
    {
        if (CurrentWayPoint + 1 >= WayPoints.Count)
        {
            CurrentWayPoint = 0;
        }
        else
        {
            CurrentWayPoint++;
        }
        Agent.destination = WayPoints[CurrentWayPoint].position;
    }
    
    
    public override void TakeDamage(float DMG)
    {
        base.TakeDamage(DMG);
        BeAgro();
    }


    private void BeAgro()
    {
        AgroTimer = AgroTimerDefault;
        isAgro = true;
        Attack();
    }


    void AlarmCheck()
    {
        if (AgroTimer>0)
        {
            AgroTimer -= Time.deltaTime;
        }
        else
        {
            isAgro = false;
        }
        if (ActorController.Actor)
        {
            //–асчет попадани€ в поле зрени€
            Vector3 search = ActorController.Actor.transform.position - transform.position;
            float angle = Vector3.Dot(search.normalized, transform.forward);
            if (angle > FOV)//если игрок в поле зрени€, чекаем, не за стеной ли он
            {
                RaycastHit checkedObj;
                if (Physics.Raycast(transform.position, search, out checkedObj, AgroRange))
                {
                    var castedActor = checkedObj.collider.gameObject.GetComponentInParent<ActorController>();
                    if (castedActor)
                    {
                        BeAgro();
                    }
                }
            }
        }
    }


    void Attack()
    {
        if(ActorController.Actor)
        {
            var _target = ActorController.Actor.transform.position;
            _target.y += ActorController.Actor.Mark;
            Canon.Shoot(_target);
            transform.forward = -1 * Vector3.Normalize(transform.position - ActorController.Actor.transform.position);
        }
    }


    public override object RespawnData
    {
        get
        {
            List<Transform> _wayPoints = new List<Transform>();
            foreach (var point in WayPoints)
            {
                _wayPoints.Add(point);
            }
            return WayPoints;
        }
        set
        {
            WayPoints = (List<Transform>)value;
        }
    }
}