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
        //������������� ������ ��������. �� ��� ����� �������� ������� ���� �� �����
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

    void Walk()//������� ������������ �������� ����-����, �������� ������ ����������. ���� �������� ���� (�������) - ����� ������ �� �����
    {          //���� � ���������� ����� ���� �� ���� �������� - ����� ������ �� ���� � �������
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

    void AlarmCheck()//������� ������������ ���� ������ �� ������� ������
    {                //��� ����� ������, �������� �� ����� � ���� ������. ���� �� - ��������� � ����. ���� ��� �� ��������� ����������� - ����� �������, ��� ����� �����
        if (Actor)
        {
            //������ ��������� � ���� ������
            Vector3 search = Actor.transform.position - transform.position;
            float angle = Vector3.Dot(search.normalized, transform.forward);
            if (angle > FOV)//���� ����� � ���� ������, ������, �� �� ������ �� ��
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
