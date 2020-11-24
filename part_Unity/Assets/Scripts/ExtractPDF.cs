using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using sharpPDF;
using sharpPDF.Enumerators;
using UnityEngine.UI;


public class ExtractPDF : MonoBehaviour {

	internal string attacName = "Assets/Output/team7_Lake_pdfExtraction.pdf";
	Button button;
	private Vector3[] angles = { new Vector3(0,-90,90), new Vector3(0,0,90), new Vector3(0,-180, 90),
								 new Vector3(0,0,0), new Vector3(0,90,90), new Vector3(0, 90, 180)};
	public GameObject cameraOrbit;

	List<string> itemsEng = new List<string>();

	// Use this for initialization
	void Start ()
	{
		itemsEng.Add("Front View"); // 정면도
		itemsEng.Add("Left Side View"); // 좌측면도
		itemsEng.Add("Right Side View"); //우측면도
		itemsEng.Add("Floor Plan");// 평면도
		itemsEng.Add("Rear View"); // 배면도
		itemsEng.Add("Bottom View"); // 저면도;
		

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
		StartCoroutine(CreatePDF());
	}

	// Update is called once per frame
	public IEnumerator CreatePDF () {
		pdfDocument myDoc = new pdfDocument("team7_Lake_pdfExtraction", "team7_Lake", false);
	
		foreach(var item in itemsEng)
        {
			pdfPage myPage = myDoc.addPage();
			//		Debug.Log ( "Continue to create PDF");
			myPage.addText(item, 10, 730, predefinedFont.csHelveticaOblique, 30, new pdfColor(predefinedColor.csOrange));

			string s = "Assets/Output/Views/" + item + ".jpg";
			Debug.Log(Path.GetFullPath(s));
			yield return StartCoroutine(myPage.newAddImage("FILE://" + Path.GetFullPath(s), 0, 200));
		}
		
		myDoc.createPDF(attacName);
	}
}

/*
// Table's creation
pdfTable myTable = new pdfTable();
//Set table's border
myTable.borderSize = 1;
myTable.borderColor = new pdfColor(predefinedColor.csDarkBlue);

// Add Columns to a grid
myTable.tableHeader.addColumn(new pdfTableColumn("Model", predefinedAlignment.csRight, 120));
myTable.tableHeader.addColumn(new pdfTableColumn("Speed", predefinedAlignment.csCenter, 120));
myTable.tableHeader.addColumn(new pdfTableColumn("Weight", predefinedAlignment.csLeft, 150));
myTable.tableHeader.addColumn(new pdfTableColumn("Color", predefinedAlignment.csLeft, 150));


pdfTableRow myRow = myTable.createRow();
myRow[0].columnValue = "A";
myRow[1].columnValue = "100 km/h";
myRow[2].columnValue = "180Kg";
myRow[3].columnValue = "Orange";

myTable.addRow(myRow);

pdfTableRow myRow1 = myTable.createRow();
myRow1[0].columnValue = "B";
myRow1[1].columnValue = "130 km/h";
myRow1[2].columnValue = "150Kg";
myRow1[3].columnValue = "Yellow";

myTable.addRow(myRow1);



// Set Header's Style
myTable.tableHeaderStyle = new pdfTableRowStyle(predefinedFont.csCourierBoldOblique, 12, new pdfColor(predefinedColor.csBlack), new pdfColor(predefinedColor.csLightOrange));
// Set Row's Style
myTable.rowStyle = new pdfTableRowStyle(predefinedFont.csCourier, 8, new pdfColor(predefinedColor.csBlack), new pdfColor(predefinedColor.csWhite));
// Set Alternate Row's Style
myTable.alternateRowStyle = new pdfTableRowStyle(predefinedFont.csCourier, 8, new pdfColor(predefinedColor.csBlack), new pdfColor(predefinedColor.csLightYellow));
// Set Cellpadding
myTable.cellpadding = 10;
// Put the table on the page object
myFirstPage.addTable(myTable, 5, 700);
*/