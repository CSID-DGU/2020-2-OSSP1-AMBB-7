using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAKE;

public class BeamManager
{
    public static List<HBeam> HBeam {
        get
        {
            return new List<HBeam>(new HBeam[]{
                new HBeam(150, 100, 3.2, 4.5),
                new HBeam(150, 100, 3.2, 6.0),
                new HBeam(150, 100, 6.0, 9.0),
                new HBeam(200, 100, 3.2, 6.0),
                new HBeam(200, 100, 5.5, 8.0),
                new HBeam(200, 150, 6.0, 9.0)
            });
        }
    }

    public static List<PILLARBeam> PillarBeam
    {
        get
        {
            return new List<PILLARBeam>(new PILLARBeam[]{
                new PILLARBeam(3.2),
                new PILLARBeam(4.5),
                new PILLARBeam(6),
                new PILLARBeam(9)
            });
        }
    }
}
