using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sfs2X.Entities.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UnityEngine.UI;

using System;

public class PrintingManager : MonoBehaviour
{
    private Vector3[] angles = { new Vector3(0,-90,90), new Vector3(0,0,90), new Vector3(0,-180, 90),
                                 new Vector3(0,0,0), new Vector3(0,90,90), new Vector3(0, 90, 180)};
    public GameObject cameraOrbit;
    List<string> itemsEng = new List<string>();
    Button button;

    string assetsPath = null;
    string pdfPath = null;
    void Start()
    {
        assetsPath = Application.dataPath;
        pdfPath = assetsPath + "/Output/pdfExtraction.pdf";

        itemsEng.Add("Front View");
        itemsEng.Add("Left Side View");
        itemsEng.Add("Right Side View");
        itemsEng.Add("Floor Plan");
        itemsEng.Add("Rear View");
        itemsEng.Add("Bottom View");

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        StartCoroutine(StartCapturing());
    }

    public IEnumerator StartCapturing()
    {
        int idx = 0;
        foreach (var item in itemsEng)
        {
            cameraOrbit.transform.eulerAngles = angles[idx];
            StartCoroutine(GameObject.Find("Button").GetComponent<ButtonHandler>().CaptureJpg("Views/" + item, false));
            yield return new WaitForSeconds(0.2f);
            Debug.Log(idx);
            idx++;
        }

        GenerateFile();
    }

    public void GenerateFile()
    {
        if (File.Exists(pdfPath))
            File.Delete(pdfPath);
        using (var fileStream = new FileStream(pdfPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            var document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            var writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            foreach (var item in itemsEng)
            {
                document.NewPage();
                //var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Paragraph p = new Paragraph(item);
                p.Alignment = Element.ALIGN_CENTER;
                document.Add(p);

                string imageURL = assetsPath + "/Output/Views/" + item + ".jpg";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScaleToFit(480f, 360f); //image resizing
                jpg.SpacingBefore = 10f;    //image spacing
                jpg.SpacingAfter = 1f;      //image spacing
                jpg.Alignment = Element.ALIGN_CENTER;
                document.Add(jpg);
            }

            document.Close();
            writer.Close();
        }

        PrintFiles();   // opens generated pdf file
    }


    void PrintFiles()
    {
        Debug.Log(pdfPath);
        if (pdfPath == null)
            return;

        if (File.Exists(pdfPath))
        {
            Debug.Log("file found");
        }
        else
        {
            Debug.Log("file not found");
            return;
        }
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = pdfPath;

        process.Start();
    }
}