using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLineUseCube : MonoBehaviour
{
    Transform transform;
/*    Vector3 initialSize;*/

    [SerializeField] Slider xSlider, ySlider, zSlider;

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
/*        initialSize = transform.localScale;*/
    }

    // Update is called once per frame
    void Update()
    {
        Resize(xSlider.value, ySlider.value, zSlider.value);
    }

    void Resize(float x, float y, float z)
    {
        transform.localScale = 4.0f * new Vector3(x + 0.2f, y + 0.2f, z + 0.2f);
    }
}
