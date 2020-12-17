using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShowListHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] fileEntries = Directory.GetFiles("file://" + Application.dataPath + "/" , "*.jpg");
        List<string> files = new List<string>();
        foreach (string fileName in fileEntries)
        {
            //Debug.Log(Path.GetFileName(fileName));
            files.Add(fileName);
        }

        var dropdown = transform.GetComponent<Dropdown>();

        dropdown.options.Clear();

        foreach (var item in files)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });

        GameObject rawImageObj = GameObject.Find("RawImage");
        RawImage rawImage = rawImageObj.GetComponent<RawImage>();
        rawImage.color = (new Color(189 / 255f, 195 / 255f, 200 / 255f));
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;

        string[] fileEntries = Directory.GetFiles("file://" + Application.dataPath + "/", "*.jpg");
        List<string> files2 = new List<string>();
        foreach (string fileName in fileEntries)
        {
            files2.Add(fileName);
        }

        GameObject rawImageObj = GameObject.Find("RawImage");
        RawImage rawImage = rawImageObj.GetComponent<RawImage>();
        rawImage.color = (new Color(255 / 255f, 255 / 255f, 255 / 255f));
        byte[] fileData = System.IO.File.ReadAllBytes(files2[index]);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);
        rawImage.texture = tex;

    }
}