#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

/// <summary>
/// Alien Shooter moves like Controller
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class ActorController : UnimmortallObject
{
    NavMeshAgent Agent;
    [SerializeField]
    private Transform Visual;
    [SerializeField]
    private float MoveSpeed;

    protected override void Start()
    {
        base.Start();
        ConstraintSource lookPoint = new ConstraintSource();
        lookPoint.sourceTransform = CamOrto.SharedLookPoint;
        Actor = this;
        Agent = this.GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();
        var pos = this.transform.position;
        pos.x -= Input.GetAxis("Horizontal")*MoveSpeed;
        pos.z -= Input.GetAxis("Vertical")*MoveSpeed;
        Visual.transform.forward = -1 * Vector3.Normalize(this.transform.position - CamOrto.SharedLookPoint.position);
        Agent.nextPosition = pos;
    }
}
