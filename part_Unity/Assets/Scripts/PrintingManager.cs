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
        assetsPath = System.Environment.CurrentDirectory;
        pdfPath = "pdfExtraction.pdf";

        itemsEng.Add("FrontView");
        itemsEng.Add("LeftSideView");
        itemsEng.Add("RightSideView");
        itemsEng.Add("FloorPlan");
        itemsEng.Add("RearView");
        itemsEng.Add("BottomView");

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
            StartCoroutine(GameObject.Find("Button").GetComponent<ButtonHandler>().CaptureJpgforPDF(item, false));
            yield return new WaitForSeconds(0.2f);
            Debug.Log(idx);
            idx++;
        }

        GenerateFile();
    }

    public class KeyInfo
    {
        public KeyInfo(string _type, int _price, int _cnt = 0)
        {
            type = _type;
            price = _price;
            cnt = _cnt;
        }
        public string type; // infoToPrint
        public int price = 0; // price
        public int cnt = 0; // 
    }
    public void GenerateFile()
    {
        if (File.Exists(pdfPath))
            File.Delete(pdfPath);
        using (var fileStream = new FileStream(pdfPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            var document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            var writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();
            document.NewPage();

            List<KeyInfo> HBeam = new List<KeyInfo>();
            List<KeyInfo> Pillar = new List<KeyInfo>();
            List<KeyInfo> Connector = new List<KeyInfo>();

            int cnt_HBeam = 0, cnt_Pillar = 0, cnt_Connector = 0, total_Price = 0, total_Number = 0;

            Paragraph p1 = new Paragraph("Detailed Statement");
            p1.Alignment = Element.ALIGN_CENTER;
            document.Add(p1);

            PdfPTable pdfTable = new PdfPTable(3);
            pdfTable.SpacingBefore = 100f;
            pdfTable.DefaultCell.Padding = 5;
            pdfTable.WidthPercentage = 90;
            pdfTable.DefaultCell.BorderWidth = 0.5f;
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            //pdfTable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(64, 134, 170);

            for (int i = 0; i < Viewer.beamLinesList[0].Count; i++)
            {
                bool find = false;
                if (Viewer.beamLinesList[0][i].type == RAKE.BEAM_TYPE.H)
                {
                    for (int j = 0; j < HBeam.Count; j++)
                    {
                        if (HBeam[j].type == Viewer.beamLinesList[0][i].InfoToPrint)
                        {
                            HBeam[j].cnt++;
                            find = true;
                            break;
                        }
                    }
                    if (find == false)
                    {
                        cnt_HBeam++;
                        HBeam.Add(new KeyInfo(Viewer.beamLinesList[0][i].InfoToPrint, Viewer.beamLinesList[0][i].price, 1));
                    }
                }
                else if (Viewer.beamLinesList[0][i].type == RAKE.BEAM_TYPE.PILLAR)
                {
                    for (int j = 0; j < Pillar.Count; j++)
                    {
                        if (Pillar[j].type == Viewer.beamLinesList[0][i].InfoToPrint)
                        {
                            Pillar[j].cnt++;
                            find = true;
                            break;
                        }
                    }
                    if (find == false)
                    {
                        cnt_Pillar++;
                        Pillar.Add(new KeyInfo(Viewer.beamLinesList[0][i].InfoToPrint, Viewer.beamLinesList[0][i].price, 1));
                    }
                }
                else if (Viewer.beamLinesList[0][i].type == RAKE.BEAM_TYPE.CONNECTOR)
                {
                    for (int j = 0; j < Connector.Count; j++)
                    {
                        if (Connector[j].type == Viewer.beamLinesList[0][i].InfoToPrint)
                        {
                            Connector[j].cnt++;
                            find = true;
                            break;
                        }
                    }
                    if (find == false)
                    {
                        cnt_Connector++;
                        Connector.Add(new KeyInfo(Viewer.beamLinesList[0][i].InfoToPrint, Viewer.beamLinesList[0][i].price, 1));
                    }
                }
                total_Price += Viewer.beamLinesList[0][i].price;
                total_Number++;
            }

            PdfPCell cell = new PdfPCell(new Phrase("Detailed Statements of Architecture"));
            cell.Colspan = 3;
            cell.HorizontalAlignment = 1;
            cell.Padding = 10;
            pdfTable.AddCell(cell);
            pdfTable.AddCell(new Phrase("Item / Type"));
            pdfTable.AddCell(new Phrase("Numbers of Item"));
            pdfTable.AddCell(new Phrase("Price"));

            cell = new PdfPCell(new Phrase("H beam"));
            cell.Colspan = 3;
            cell.HorizontalAlignment = 0;
            cell.Padding = 10;
            pdfTable.AddCell(cell);
            for (int i = 0; i < cnt_HBeam; i++)
            {
                pdfTable.AddCell(new Phrase(HBeam[i].type));
                pdfTable.AddCell(new Phrase(HBeam[i].cnt.ToString()));
                pdfTable.AddCell(new Phrase(HBeam[i].price.ToString()));
            }

            cell = new PdfPCell(new Phrase("Pillar"));
            cell.Colspan = 3;
            cell.HorizontalAlignment = 0;
            cell.Padding = 10;
            pdfTable.AddCell(cell);

            for (int i = 0; i < cnt_Pillar; i++)
            {
                pdfTable.AddCell(new Phrase(Pillar[i].type));
                pdfTable.AddCell(new Phrase(Pillar[i].cnt.ToString()));
                pdfTable.AddCell(new Phrase(Pillar[i].price.ToString()));
            }

            cell = new PdfPCell(new Phrase("Connector"));
            cell.Colspan = 3;
            cell.HorizontalAlignment = 0;
            cell.Padding = 10;
            pdfTable.AddCell(cell);

            for (int i = 0; i < cnt_Connector; i++)
            {
                pdfTable.AddCell(new Phrase(Connector[i].type));
                pdfTable.AddCell(new Phrase(Connector[i].cnt.ToString()));
                pdfTable.AddCell(new Phrase(Connector[i].price.ToString()));
            }



            pdfTable.AddCell(new Phrase("Total Price : "));
            pdfTable.AddCell(new Phrase(total_Number.ToString()));
            pdfTable.AddCell(new Phrase(total_Price.ToString()));


            document.Add(pdfTable);
            int oddnum = 0;
            foreach (var item in itemsEng)
            {
                if (oddnum++ % 2 == 0) document.NewPage();
                //var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Paragraph p = new Paragraph(item);
                p.Alignment = Element.ALIGN_CENTER;
                document.Add(p);

                string imageURL = "file://" + Application.dataPath + "/" + item +".jpg"; //relative path that works!
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScaleToFit(480f, 360f); //image resizing
                jpg.SpacingBefore = 10f;    //image spacing
                jpg.SpacingAfter = 10f;      //image spacing
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