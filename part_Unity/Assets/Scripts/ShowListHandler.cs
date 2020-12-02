using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShowListHandler : MonoBehaviour
{
    private int[] angles = { 10, 20, 30, 40, 50, 60 };

    // Start is called before the first frame update
    void Start()
    {
        string[] fileEntries = Directory.GetFiles("Assets/Output/Views/", "*.jpg");
        List<string> files = new List<string>();
        foreach (string fileName in fileEntries)
        {
            //Debug.Log(Path.GetFileName(fileName));
            files.Add(Path.GetFileName(fileName));
        }

        var dropdown = transform.GetComponent<Dropdown>();

        dropdown.options.Clear();

        foreach (var item in files)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;

        string[] fileEntries = Directory.GetFiles("Assets/Output/Views/", "*.jpg");
        List<string> files2 = new List<string>();
        foreach (string fileName in fileEntries)
        {
            files2.Add(fileName);
        }

        GameObject rawImageObj = GameObject.Find("RawImage");
        RawImage rawImage = rawImageObj.GetComponent<RawImage>();
        byte[] fileData = System.IO.File.ReadAllBytes(files2[index]);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);
        rawImage.texture = tex;

        Debug.Log("Value: " + files2[index]);

    }
}