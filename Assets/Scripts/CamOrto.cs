using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamOrto : MonoBehaviour
{
    public static Vector3 SharedLookPoint;
    
    [SerializeField]
    private float ScrollSpeed;
    [SerializeField]
    private float RotationSpeed;
    [SerializeField]
    private float CamSpeed;
    private const float ScrollMultiplier = 500f;


    private Ray LookPoint;
    private RaycastHit point;
    private Transform CameraTransform;

    void Start()
    {
        SharedLookPoint = new Vector3();
        CameraTransform = Camera.main.transform;
    }


    void Update()
    {
        ModifyCameraLookPoint();
        InterpolateToActorPosition();
    }


    private void ModifyCameraLookPoint()
    {
        CameraTransform.position += Input.GetAxis("Mouse ScrollWheel") * CameraTransform.forward * ScrollSpeed * Time.deltaTime * ScrollMultiplier;

        LookPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(LookPoint, out point))
        {
            SharedLookPoint = point.point;
        }
    }


    public void InterpolateToActorPosition()
    {
        if (ActorController.Actor)
        {
           transform.position = Vector3.Lerp(transform.position, ActorController.Actor.transform.position, 0.1f);
            transform.position = Vector3.Lerp(transform.position, SharedLookPoint, 0.01f);
        }
    }

}


