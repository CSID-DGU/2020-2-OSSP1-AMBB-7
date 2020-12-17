using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class ButtonHandler : MonoBehaviour
{
    Button button;
    Text text;
    string currentDate = DateTime.Now.ToString(("HH-mm-ss_yyMMdd"));
    string path = System.Environment.CurrentDirectory;
    public GameObject controlObject;

    public void OnClickButton()
    {
        WriteInfo();
        Debug.Log("path : " + path);
        StartCoroutine(CaptureJpg());

        StartCoroutine(ShowSaveText());
    }

    public void WriteInfo()
    {
        StreamWriter sw = new StreamWriter(path + "/Output/3DmodelInfo.txt");
        sw.WriteLine("Add 3d object's information (Beam's type, number, price...)");
        sw.Close();
    }

    // Start is called before the first frame update
    void Start()
    {

        controlObject = GameObject.Find("SaveText");
        text = controlObject.GetComponent<Text>();
        text.text = "";
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);

    }

    public IEnumerator CaptureJpg(string fileName = "3DmodelCapture", bool wait = true)
    {
        yield return null;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot(path + "/Output/" + fileName + "_" + currentDate + ".jpg");

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        if (wait)
            yield return new WaitForSeconds(1);
    }
    public IEnumerator CaptureJpgforPDF(string fileName = "3DmodelCapture", bool wait = true)
    {
        yield return null;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot(path + "/Output/" + fileName + ".jpg");

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        if (wait)
            yield return new WaitForSeconds(1);
    }


    public IEnumerator ShowSaveText()
    {
        text.CrossFadeAlpha(1.0f, 0.01f, false);
        text.text = "저장되었습니다!";
        yield return new WaitForSeconds(2);
        text.CrossFadeAlpha(0.0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        text.text = "";
    }
}