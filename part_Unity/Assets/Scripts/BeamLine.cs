using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BEAM_TYPE = RAKE.BEAM_TYPE;

public class BeamLine
{
    private static string _H_prefix = "H Beam ";
    private static string _PILLAR_prefix = "Pillar ";
    private static string _CONNECTOR_prefix = "Connector ";

    /// <summary>
    /// start: line start
    /// </summary>
    public Vector3 start { get; }

    /// <summary>
    /// start: line end
    /// </summary>
    public Vector3 end { get; }

    /// <summary>
    /// type: beam type
    /// </summary>
    public BEAM_TYPE type { get; set; }


    /// <summary>
    /// Beam information
    /// </summary>
    public string info { get; set; }

    /// <summary>
    /// Beam information to print
    /// </summary>
    public string InfoToPrint { get {
            string prefix = "";
            if (type == RAKE.BEAM_TYPE.H)
            {
                prefix = _H_prefix;
            }
            else if (type == RAKE.BEAM_TYPE.PILLAR)
            {
                prefix = _PILLAR_prefix;
            }
            else if (type == RAKE.BEAM_TYPE.CONNECTOR)
			{
                prefix = _CONNECTOR_prefix;
			}
            return prefix + info;
        }
    }

    public int price { get; }

    /// <summary>
    /// default contructor
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public BeamLine(Vector3 start, Vector3 end)
	{
        this.start = start;
        this.end = end;
        type = BEAM_TYPE.NONE;
        info = "";
	}

    /// <summary>
    /// contructor for H Beam
    /// </summary>
    /// <param name="start">start point</param>
    /// <param name="end">start point</param>
    /// <param name="H">thickness</param>
    /// <param name="W">thickness</param>
    /// <param name="t1">thickness</param>
    /// <param name="t2">thickness</param>
    /// <param name="price">price</param>
    public BeamLine(Vector3 start, Vector3 end, double H, double W, double t1, double t2, int price)
	{
        this.start = start;
        this.end = end;
        type = BEAM_TYPE.H;
        info = getInfo(H, W, t1, t2);
        this.price = price;
    }

    /// <summary>
    /// contructor for PILLAR
    /// </summary>
    /// <param name="start">start point</param>
    /// <param name="end">start point</param>
    /// <param name="t1">thickness</param>
    public BeamLine(Vector3 start, Vector3 end, double t1, int price)
    {
        this.start = start;
        this.end = end;
        type = BEAM_TYPE.PILLAR;
        info = getInfo(t1);
        this.price = price;
    }

    public BeamLine(Vector3 Point, int price)
	{
        start = end = Point;
        type = BEAM_TYPE.CONNECTOR;
        this.price = price;
	}

    public double getSize()
	{
        double ret = 1;
        string[] thick = info.Split('×');
        for (int i = 0; i < thick.Length; i++) ret *= double.Parse(thick[i]);
        if (type == BEAM_TYPE.PILLAR)
		{
            ret *= 100 * 100;
		}
        return ret;
	}

    public string getInfo(double H, double W, double t1, double t2)
	{
        return H + "×" + W + "×" + t1 + "×" + t2;
	}

    public string getInfo(double t1)
	{
        return 100 + "×" + 100 + "×" + t1;
    }

    public double getLength()
	{
        float dx = start.x - end.x;
        float dy = start.y - end.y;
        float dz = start.z - end.z;
        return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
	}
}