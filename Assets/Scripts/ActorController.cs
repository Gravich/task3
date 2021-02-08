using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

/// <summary>
/// Alien Shooter moves like Controller
/// </summary>
public class ActorController : Creature
{
    public static ActorController Actor;
    [Header("Actor Config")]
    [SerializeField]
    private Transform Visual;

    [SerializeField]
    private float MoveSpeed;
    public int Mark;

    protected override void Start()
    {
        base.Start();
        Actor = this;
    }

    protected override void Update()
    {
        base.Update();
        var pos = this.transform.position;
        pos.x -= Input.GetAxis("Horizontal")*MoveSpeed;
        pos.z -= Input.GetAxis("Vertical")*MoveSpeed;
        Visual.transform.forward = -1 * Vector3.Normalize(this.transform.position - CamOrto.SharedLookPoint);
        Agent.nextPosition = pos;
    }
}
