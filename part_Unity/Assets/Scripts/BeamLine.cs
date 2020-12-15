using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BEAM_TYPE = RAKE.BEAM_TYPE;

public class BeamLine
{
    private Vector3 _start;
    private Vector3 _end;
    private BEAM_TYPE _type;
    private string _info;
    private int _price;

    /// <summary>
    /// start: line start
    /// </summary>
    public Vector3 start { get { return _start; } }

    /// <summary>
    /// start: line end
    /// </summary>
    public Vector3 end { get { return _end; } }

    /// <summary>
    /// type: beam type
    /// </summary>
    public BEAM_TYPE type { get { return _type; } set { _type = value; } }

    /// <summary>
    /// Beam information
    /// </summary>
    public string info { get { return _info; } }

    public int price { get { return _price; } }

    /// <summary>
    /// default contructor
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public BeamLine(Vector3 start, Vector3 end)
	{
        _start = start;
        _end = end;
        type = BEAM_TYPE.NONE;
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
        _start = start;
        _end = end;
        _type = BEAM_TYPE.H;
        _info = setInfo(H, W, t1, t2);
        _price = price;
    }

    /// <summary>
    /// contructor for PILLAR
    /// </summary>
    /// <param name="start">start point</param>
    /// <param name="end">start point</param>
    /// <param name="t1">thickness</param>
    public BeamLine(Vector3 start, Vector3 end, double t1, int price)
    {
        _start = start;
        _end = end;
        _type = BEAM_TYPE.PILLAR;
        _info = setInfo(t1);
        _price = price;
    }

    public BeamLine(Vector3 Point, int price)
	{
        _start = _end = Point;
        _type = BEAM_TYPE.CONNECTOR;
        _price = price;
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

    public string setInfo(double H, double W, double t1, double t2)
	{
        return H + "×" + W + "×" + t1 + "×" + t2;
	}

    public string setInfo(double t1)
	{
        return 100 + "×" + 100 + "×" + t1;
    }
}