using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputControl : MonoBehaviour
{
    public GameObject cameraOrbit;
    public static bool movable = true;
    public float rotateSpeed = 8f;
    private float moveSpeed = 0.1f;
    public CameraController cameraController;

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
            float v = moveSpeed * Input.GetAxis("Mouse Y");

            cameraOrbit.transform.position = new Vector3(cameraOrbit.transform.position.x + v, cameraOrbit.transform.position.y, cameraOrbit.transform.position.z + h);
            cameraController.moveTargetPosition(h, v);
        }

        float scrollFactor = Input.GetAxis("Mouse ScrollWheel");

        if (scrollFactor != 0)
        {
            cameraOrbit.transform.localScale = cameraOrbit.transform.localScale * (1f - scrollFactor);
        }
    }
}
