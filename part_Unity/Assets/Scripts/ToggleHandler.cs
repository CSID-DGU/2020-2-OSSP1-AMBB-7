using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    Toggle toggle;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        ToggleValueChanged(toggle);
        toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(toggle); });
    }

    void ToggleValueChanged(Toggle toggle)
    {
        if(toggle.isOn == false)
            camera.orthographic = false;
        else
            camera.orthographic = true;
    }

}
