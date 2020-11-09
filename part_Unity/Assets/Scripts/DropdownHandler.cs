using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public Transform target;
    public GameObject cameraOrbit;

    private Vector3[] angles = { new Vector3(0,-90,90), new Vector3(0,0,90), new Vector3(0,-180, 90),
                                 new Vector3(0,0,0), new Vector3(0,90,90), new Vector3(0, 90, 180)};

    // Start is called before the first frame update
    void Start()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        
        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("자유시점");
        items.Add("정면도");
        items.Add("좌측면도");
        items.Add("우측면도");
        items.Add("평면도");
        items.Add("배면도");
        items.Add("저면도");

        foreach(var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;

        if (index != 0)
        {
            cameraOrbit.transform.eulerAngles = angles[index - 1];
        }
    }
}
