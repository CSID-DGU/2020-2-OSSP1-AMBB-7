using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAKE;

class BeamGenerator
{
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
		List<BeamLine> ret = new List<BeamLine>(list);
		int listLength = list.Count;
		for (int i = 0; i < listLength; i++) ret[i].type = getType(ret[i]);
		Func<BeamLine, BeamLine, int> comp = delegate (BeamLine l, BeamLine r)
		{
			double lM = Math.Max(l.start.y, l.end.y), rM = Math.Max(r.start.y, r.end.y);
			double lm = Math.Min(l.start.y, l.end.y), rm = Math.Min(r.start.y, r.end.y);
			if (lM > rM) return 1; // 바꿈
			else if (lM == rM)
			{
				if (lm == lM && rm != rM) return 1;
			}
			return -1;
		};
		for (int i = 0; i < listLength; i++)
		{
			for (int j = i + 1; j < listLength; j++)
			{
				if (comp(ret[i], ret[j]) == 1)
				{
					BeamLine t = ret[i];
					ret[i] = ret[j];
					ret[j] = t;
				}
			}
		}
		// sort given BEAM
		List<BeamLine> HBeam = BeamManager.HBeam;
		List<BeamLine> Pillar = BeamManager.PillarBeam;
		HBeam.Sort(delegate (BeamLine l, BeamLine r)
		{
			if (l.getSize() > r.getSize()) return 1;
			return -1;
		});
		Pillar.Sort(delegate (BeamLine l, BeamLine r)
		{
			if (l.getSize() > r.getSize()) return 1;
			return -1;
		});
		int hIdx = 0, pIdx = 0;
		bool isH = true;
		int HLength = HBeam.Count, PillarLength = Pillar.Count;
		for (int i = 0; i < listLength; i++)
		{
			if (isH)
			{
				if (ret[i].type == BEAM_TYPE.PILLAR)
				{
					hIdx++;
					if (hIdx <= HLength) hIdx = HLength - 1;
					i--;
					isH = false;
					continue;
				}
				ret[i].info = HBeam[hIdx].info;
			}
			else
			{
				if (ret[i].type == BEAM_TYPE.H)
				{
					pIdx++;
					if (pIdx <= PillarLength) pIdx = PillarLength - 1;
					i--;
					isH = true;
					continue;
				}
				ret[i].info = Pillar[pIdx].info;
			}
		}
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
		if (line.start.y != line.end.y)
		{
			ret = BEAM_TYPE.PILLAR;
		}
		else
		{
			ret = BEAM_TYPE.H;
		}
/*		Debug.Log(line.start + " || " + line.end + " || " + ret);*/
		return ret;
	}
}