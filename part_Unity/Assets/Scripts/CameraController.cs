using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.WSA.Input;

public class CameraController : MonoBehaviour
{
    public Transform cameraOrbit;
    public Transform target;

    void Start()
    {
        cameraOrbit.position = target.position;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);

        transform.LookAt(target.position);
    }

    public void moveTargetPosition(float h, float v)
    {
        target.position = new Vector3(target.position.x + v, target.position.y, target.position.z + h);
    }
}
