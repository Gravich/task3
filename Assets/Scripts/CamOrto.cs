#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamOrto : MonoBehaviour
{
    public static Transform SharedLookPoint;

    private Camera MainCam;
    private Transform CameraTransform;

    [SerializeField]
    private Transform LookAtPointPos;

    [SerializeField]
    private float ScrollSpeed;
    [SerializeField]
    private float RotationSpeed;
    [SerializeField]
    private float CamSpeed;
    [SerializeField]
    private const float ScrollMultiplier = 500f;

    void Start()
    {
        SharedLookPoint = LookAtPointPos;
        CameraTransform = Camera.main.transform;//схороним ссылку на трансформ заранее, к нему придетс€ посто€нно обращатьс€
    }


    void Update()
    {
        CameraRotate();
        MoveLookPoint();
        FindActor();
    }

    private void CameraRotate()
    {
        CameraTransform.position += Input.GetAxis("Mouse ScrollWheel") * CameraTransform.forward * ScrollSpeed * Time.deltaTime * ScrollMultiplier;//приближение камеры
    }

    private void MoveLookPoint()
    {
        Ray LookAtPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        if (Physics.Raycast(LookAtPoint, out point))
        {
            LookAtPointPos.position = point.point;
        }
    }

    public void FindActor()//функци€ центрировани€ камеры на игроке
    {
        if (UnimmortallObject.Actor)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, UnimmortallObject.Actor.transform.position, 0.1f);
            this.transform.position = Vector3.Lerp(this.transform.position, LookAtPointPos.position, 0.01f);
        }
    }

}


