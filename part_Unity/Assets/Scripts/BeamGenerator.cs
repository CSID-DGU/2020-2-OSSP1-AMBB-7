using System.Collections;
using System.Collections.Generic;
using BEAM_TYPE = RAKE.BEAM_TYPE;

class BeamGenerator {
    /// <summary>
    /// return all combination of beam
    /// </summary>
    /// <param name="list">get list of beamline(What Team 1 did)</param>
    /// <returns></returns>
    public static List<List<BeamLine>> generator(in List<BeamLine> list)
    {
        List<List<BeamLine>> ret = new List<List<BeamLine>>();
        ret.Add(normal(list));
        return ret;
    }
    /// <summary>
    /// basic generator
    /// </summary>
    /// <param name="list">???</param>
    /// <returns></returns>
    private static List<BeamLine> normal(in List<BeamLine> list)
    {
        List<BeamLine> ret = new List<BeamLine>();
        int listLength = list.Count;
        for (int i = 0; i < listLength; i++) ret[i].type = BEAM_TYPE.NONE;
        return ret;
    }

    /// <summary>
    /// return line type
    /// </summary>
    /// <param name="line">pass one line</param>
    /// <returns></returns>
    private static BEAM_TYPE getType(in BeamLine line)
    {
        BEAM_TYPE ret;
        if (line.start.Z == line.end.Z)
        {
            ret = BEAM_TYPE.B;
        }
        else
        {
            ret = BEAM_TYPE.C;
        }
        return ret;
    }
}