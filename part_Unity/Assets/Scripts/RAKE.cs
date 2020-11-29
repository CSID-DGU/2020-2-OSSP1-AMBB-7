using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAKE
{
    public interface BEAM
    {
        double Size();
    }
    public class HBeam : BEAM
    {
        int T1 { get; set; }
        int T2 { get; set; }
        double H { get; set; }
        double W { get; set; }

        public HBeam(int t1, int t2, double h, double w)
        {
            T1 = t1;
            T2 = t2;
            H = h;
            W = w;
        }

        public double Size() { return T1 * T2 * H * W; }
    }

    public class PILLARBeam : BEAM
    {
        double T1 { get; set; }

        public PILLARBeam(double t1)
        {
            T1 = t1;
        }

        public double Size() { return T1; }
    }

    public enum BEAM_TYPE
    {
        NONE,
        FLAGE,
        OPEN_CUBE,
        CLOSED_CUBE,
        CLOSED_CUBE_200,
        PILLAR,
        H,
/*        NONE,
        FLAGE,
        OPEN_CUBE,
        CLOSED_CUBE,
        CLOSED_CUBE_200,
        PILLAR_3_2,
        PILLAR_4_5,
        PILLAR_6,
        PILLAR_9,
        H_1_1_3_4,
        H_1_1_3_6,
        H_2_1_3_6,
        H_2_1_6_9,*/
    }
}