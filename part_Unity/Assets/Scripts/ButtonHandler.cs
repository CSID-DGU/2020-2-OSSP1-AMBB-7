using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ButtonHandler : MonoBehaviour
{
    Button button;
    Button button2;
    public GameObject controlObject;
    
    public void OnClickButton()
    {
        Writetxt();
        Capturejpg();

        controlObject.SetActive(true);
        button2 = controlObject.GetComponent<Button>();
        button2.onClick.AddListener(ButtonHidden);
    }

    public void ButtonHidden()
    {
        controlObject.SetActive(false);
    }

    public void Writetxt()
    {
        StreamWriter sw = new StreamWriter("Assets/Output/3DmodelInfo.txt");
        sw.WriteLine("Add 3d object's information (Beam's type, number, price...)");
        sw.Close();
    }

    public void Capturejpg()
    {
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        tex.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, true);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes("Assets/Output/3DmodelCapture.jpg", bytes);
        DestroyImmediate(tex);
    }
    // Start is called before the first frame update
    void Start()
    {
        controlObject = GameObject.Find("Button2");
        controlObject.SetActive(false);
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }
}
