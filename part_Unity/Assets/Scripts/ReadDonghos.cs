using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadDonghos
{
	private List<BeamLine> beamList;
	string path;
	public static Vector3 rotatePoint = new Vector3(0, 0, 0);
	public ReadDonghos()
	{
		path = System.Environment.CurrentDirectory;
		beamList = new List<BeamLine>();
	}

	public List<BeamLine> run()
	{
		prepareRun();
		runPython();
		runCPP();
		ReadTXTFiles();
/*		removeTrash();*/
		return beamList;
	}

	public void prepareRun()
	{
		string[] rename = new string[]
		{
			"roof_floor_view.xls",
			"right_side_view.xls",
			"rear_view.xls",
			"left_side_view.xls",
			"front_view.xls",
			"floor_view.xls"
		};
		string[] playerprefs = new string[]
		{
			StaticVariable.ROOF,
			StaticVariable.RIGHT,
			StaticVariable.REAR,
			StaticVariable.LEFT,
			StaticVariable.FRONT,
			StaticVariable.FLOOR
		};
		for (int i = 0; i < 6; i++)
		{
			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "CMD.exe",
					Arguments = "/C cp " + PlayerPrefs.GetString(playerprefs[i]) + " " + path + "\\" + rename[i],
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};
			proc.Start();
			proc.WaitForExit();
			proc.Close();
		}
	}

	private void runPython()
	{
		var proc = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "CMD.exe",
				Arguments = "/C cd " + path + " & " + path + "\\PolyLine_Extraction.exe",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			}
		};
		proc.Start();
		proc.WaitForExit();
		proc.Close();
	}

	private void runCPP()
	{
		var proc = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "CMD.exe",
				Arguments = "/C cd " + path + " & " + path + "\\Link.exe",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			}
		};
		proc.Start();
		proc.WaitForExit();
		proc.Close();
	}

	private void ReadTXTFiles()
	{
		string textValue = System.IO.File.ReadAllText(path + "\\3d.txt");
		UnityEngine.Debug.Log(textValue);
		string[] lines = textValue.Split('\n');
		for (int i = 0; i < lines.Length; i++)
		{
			if (lines[i].Equals("")) continue;
			string[] points = lines[i].Split(' ');
			float[] fpoints = new float[6];
			for (int j = 0; j < 6; j++)
			{
				fpoints[j] = float.Parse(points[j]);
				if (j % 3 == 0)
					rotatePoint.x += fpoints[j];
				else if (j % 3 == 1)
					rotatePoint.z += fpoints[j];
				else if (j % 3 == 2) 
					rotatePoint.y += fpoints[j];
			}
			Vector3 p1, p2;
			p1 = new Vector3(fpoints[0], fpoints[2], fpoints[1]);
			p2 = new Vector3(fpoints[3], fpoints[5], fpoints[4]);
			beamList.Add(new BeamLine(p1, p2));
			UnityEngine.Debug.Log(beamList[beamList.Count - 1].start + " " + beamList[beamList.Count - 1].end);
		}
		UnityEngine.Debug.Log("giumoring : " + lines.Length + ", " + rotatePoint.x + ", " + rotatePoint.y + ", " + rotatePoint.z);
		rotatePoint.x = (float)rotatePoint.x / (float)lines.Length / 2;
		rotatePoint.y = (float)rotatePoint.y / (float)lines.Length / 2;
		rotatePoint.z = (float)rotatePoint.z / (float)lines.Length / 2;
	}

	public void removeTrash()
	{
		var proc = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "CMD.exe",
				Arguments = "/C rm -rf *.xls *.csv 3d.txt",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			}
		};
		proc.Start();
		proc.WaitForExit();
		proc.Close();
	}
}
