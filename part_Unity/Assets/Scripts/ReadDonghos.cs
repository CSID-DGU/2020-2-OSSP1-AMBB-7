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
		path = System.Environment.CurrentDirectory + "\\Assets\\Scripts";
		beamList = new List<BeamLine>();
	}

	public List<BeamLine> run()
	{
		runPython();
		runCPP();
		ReadTXTFiles();
		return beamList;
	}

	private void runPython()
	{
		var proc = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "CMD.exe",
				/*				Arguments = "/C cd " + path + " & ls",*/
				Arguments = "/C cd " + path + "\\Donghos & python3 PolyLine_Extraction.py",
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
				Arguments = "/C cd " + path + "\\Donghos & g++ -o Link.exe Link.cpp & pwd & " + path + "\\Donghos\\Link.exe",
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
		string textValue = System.IO.File.ReadAllText(path + "\\Donghos\\3d.txt");
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
}
