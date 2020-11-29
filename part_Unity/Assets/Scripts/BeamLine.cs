using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BEAM_TYPE = RAKE.BEAM_TYPE;

public class BeamLine
{
    /// <summary>
    /// start: line start
    /// </summary>
    public Vector3 start;

    /// <summary>
    /// start: line end
    /// </summary>
    public Vector3 end;

    /// <summary>
    /// type: beam type
    /// </summary>
    public BEAM_TYPE type;

    public BeamLine(Vector3 start, Vector3 end, BEAM_TYPE type = BEAM_TYPE.NONE)
	{
        this.start = start;
        this.end = end;
        this.type = type;
	}
}