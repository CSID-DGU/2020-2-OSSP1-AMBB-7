using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ButtonHandler : MonoBehaviour
{
    Button button;
    Text text;
    public GameObject controlObject;
    
    public void OnClickButton()
    {
        WriteInfo();
        StartCoroutine(CaptureJpg());

        controlObject.SetActive(true);
        text = controlObject.GetComponent<Text>();
    }

    public void WriteInfo()
    {
        StreamWriter sw = new StreamWriter("Assets/Output/3DmodelInfo.txt");
        sw.WriteLine("Add 3d object's information (Beam's type, number, price...)");
        sw.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        controlObject = GameObject.Find("SaveText");
        controlObject.SetActive(false);
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }

    public IEnumerator CaptureJpg()
    {
        yield return null;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot("Assets/Output/3DmodelCapture.jpg");
        text.CrossFadeAlpha(1.0f, 0.01f, false);

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        yield return new WaitForSeconds(1);
        text.CrossFadeAlpha(0.0f, 0.5f, false);
    }
}
