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

    public void moveTargetPosition(float xDelta, float yDelta, float zDelta)
    {
        target.position = new Vector3(target.position.x + xDelta, target.position.y + yDelta, target.position.z + zDelta);
    }
}
