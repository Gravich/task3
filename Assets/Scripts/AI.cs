#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AI : UnimmortallObject
{
    [SerializeField]
    private float FOV;
    [SerializeField]
    private float Range;
    [SerializeField]
    private List<Transform> WayPoints;
    private NavMeshAgent Walker;
    private Weapon Canon;

    private short CurrentPoint;

    protected override void Start()
    {
        base.Start();
        //устанавливаем первую вейпоинт. За нее берем исходную позицию бота на карте
        if (WayPoints == null)
        {
            WayPoints = new List<Transform>();
        }
        Walker = GetComponent<NavMeshAgent>();
        GameObject basePos = new GameObject();
        basePos.transform.position = transform.position;
        WayPoints.Add(basePos.transform);
        CurrentPoint = 0;
        //
        Canon = GetComponent<Weapon>();
    }

    public override void TakeDamage(float DMG)
    {
        base.TakeDamage(DMG);
            this.transform.forward = -1*Vector3.Normalize(this.transform.position - Actor.transform.position);
    }
    protected override void Update()
    {
        base.Update();
        Walk();
        AlarmCheck();
    }

    void Walk()//функция зацикленного хождения туда-сюда, согласно списку вейпоинтов. Если вейпоинт один (базовый) - будем стоять на месте
    {          //если в инспекторе задан хотя бы один вейпоинт - будем ходить до него и обратно
        if (!Walker.hasPath)
        {
            if (CurrentPoint < WayPoints.Count)
            {
                Walker.destination = WayPoints[CurrentPoint].position;
                Debug.Log($"{this.name}: {CurrentPoint}");
                CurrentPoint++;
            }
            else
                CurrentPoint = 0;
        }
    }

    void AlarmCheck()//функция сканирования поля зрения на наличие игрока
    {                //бот будет чекать, попадает ли игрок в поле зрения. Если да - рейкастит в него. Если луч не встречает препятствий - можно считать, что игрок виден
        if (Actor)
        {
            //Расчет попадания в поле зрения
            Vector3 search = Actor.transform.position - transform.position;
            float angle = Vector3.Dot(search.normalized, transform.forward);
            if (angle > FOV)//если игрок в поле зрения, чекаем, не за стеной ли он
            {
                RaycastHit checkedObj;
                if (Physics.Raycast(transform.position, search, out checkedObj, Range))
                {
                    var castedActor = checkedObj.collider.gameObject.GetComponentInParent<ActorController>();
                    if (castedActor)
                    {
                        Attack();
                    }
                }
            }
        }
    }

    void Attack()
    {
        if(Actor)
        {
            Canon.Shoot(Actor.transform);
            this.transform.forward = -1 * Vector3.Normalize(this.transform.position - Actor.transform.position);
        }
    }
}
