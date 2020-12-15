using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAKE;

public class BeamManager
{
    public static List<BeamLine> HBeam
    {
        get
        {
            return new List<BeamLine>(new BeamLine[]{
                new BeamLine(Vector3.zero, Vector3.zero, 150, 100, 3.2, 4.5, 168000),
                new BeamLine(Vector3.zero, Vector3.zero, 150, 100, 3.2, 6.0, 198000),
                new BeamLine(Vector3.zero, Vector3.zero, 150, 100, 6.0, 9.0, 218000),
                new BeamLine(Vector3.zero, Vector3.zero, 200, 100, 3.2, 6.0, 208000),
                new BeamLine(Vector3.zero, Vector3.zero, 200, 100, 5.5, 8.0, 228000),
                new BeamLine(Vector3.zero, Vector3.zero, 200, 150, 6.0, 9.0, 323000)
            });
        }
    }

    public static List<BeamLine> PillarBeam
    {
        get
        {
            return new List<BeamLine>(new BeamLine[]{
                new BeamLine(Vector3.zero, Vector3.zero, 3.2, 120000),
                new BeamLine(Vector3.zero, Vector3.zero, 4.5, 140000),
                new BeamLine(Vector3.zero, Vector3.zero, 6, 160000),
                new BeamLine(Vector3.zero, Vector3.zero, 9, 180000)
            });
        }
    }

    public static List<BeamLine> Connector
    {
        get
        {
            return new List<BeamLine>(new BeamLine[]{
                new BeamLine(Vector3.zero, 70000),
                new BeamLine(Vector3.zero, 80000),
                new BeamLine(Vector3.zero, 100000)
            });
        }
    }
}