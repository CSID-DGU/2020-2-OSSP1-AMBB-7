using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputControl : MonoBehaviour
{
    public GameObject cameraOrbit;
    public static bool movable = true;
    public float rotateSpeed = 8f;
    private float moveSpeed = 0.5f;
    public CameraController cameraController;
    public Camera camera;

    private void Update()
    {
        if (Input.GetMouseButton(0) && movable == true)
        {
            float h = rotateSpeed * Input.GetAxis("Mouse X");
            float v = rotateSpeed * Input.GetAxis("Mouse Y");

            if (cameraOrbit.transform.eulerAngles.z + v <= 0.1f || cameraOrbit.transform.eulerAngles.z + v >= 179.9f)
                v = 0;

            cameraOrbit.transform.eulerAngles = new Vector3(cameraOrbit.transform.eulerAngles.x, cameraOrbit.transform.eulerAngles.y + h, cameraOrbit.transform.eulerAngles.z + v);
        }
        else if (Input.GetMouseButton(1))
        {
            float h = moveSpeed * Input.GetAxis("Mouse X");
            float v = -moveSpeed * Input.GetAxis("Mouse Y");

            float angle = cameraOrbit.transform.eulerAngles.y * Mathf.Deg2Rad;

            float xDelta = h*Mathf.Sin(angle);
            float yDelta = v;
            float zDelta = h*Mathf.Cos(angle);

            cameraOrbit.transform.position = new Vector3(cameraOrbit.transform.position.x + xDelta, cameraOrbit.transform.position.y + yDelta, cameraOrbit.transform.position.z + zDelta);
            
            cameraController.moveTargetPosition(xDelta, yDelta, zDelta);
        }
        else if(Input.GetKeyDown(KeyCode.F))
        {
            cameraController.moveTargetPosition(ReadDonghos.rotatePoint.x, ReadDonghos.rotatePoint.y, ReadDonghos.rotatePoint.z, 0);
            cameraOrbit.transform.position = ReadDonghos.rotatePoint;
        }

        float scrollFactor = Input.GetAxis("Mouse ScrollWheel");

        if (scrollFactor != 0)
        {
            if (!camera.orthographic)
            {
                cameraOrbit.transform.localScale = cameraOrbit.transform.localScale * (1f - scrollFactor);
            }
            else
            {
                camera.orthographicSize -= scrollFactor;
            }
        }
    }
}
