using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.WSA.Input;

public class CameraController : MonoBehaviour
{
    public Transform cameraOrbit;
    public Transform target;

    // beam들의 list에서 받은 beam들의 좌표 중 원점으로부터 가장 먼 beam의 원점 사이의 거리
    public float max_distance = 3;

    void Start()
    {
        cameraOrbit.position = target.position;
        cameraOrbit.localScale = new Vector3(max_distance, max_distance, max_distance);
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
