using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
	[SerializeField] private GameObject H_BEAM;
	[SerializeField] private GameObject PILLAR;
	private List<BeamLine> beamLines;
	private GameObject viewerParent;

	private List<List<BeamLine>> beamLinesList;
	// Start is called before the first frame update
	void Start()
	{
		viewerParent = GameObject.Find("ViewerParent");
		ReadBeamLines();
		beamLinesList = BeamGenerator.generator(beamLines);
		View();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void ReadBeamLines()
	{
		beamLines = new List<BeamLine>(new BeamLine[]{
		new BeamLine(new Vector3(0, 0, 0), new Vector3(0, 0, 1)),
		new BeamLine(new Vector3(0, 0, 0), new Vector3(1, 0, 0)),
		new BeamLine(new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
		new BeamLine(new Vector3(1, 0, 0), new Vector3(1, 0, 1)),
		new BeamLine(new Vector3(1, 0, 0), new Vector3(1, 1, 0)),
		new BeamLine(new Vector3(0, 1, 0), new Vector3(1, 1, 0)),
		new BeamLine(new Vector3(0, 0, 1), new Vector3(1, 0, 1)),
		new BeamLine(new Vector3(0, 1, 0), new Vector3(0, 1, 1)),
		new BeamLine(new Vector3(0, 1, 1), new Vector3(0, 0, 1)),
		new BeamLine(new Vector3(0, 1, 1), new Vector3(1, 1, 1)),
		new BeamLine(new Vector3(1, 1, 1), new Vector3(1, 1, 0)),
		new BeamLine(new Vector3(1, 1, 1), new Vector3(1, 0, 1)),
		});
	}

	void View() // TODO: Modify to show a list of various combinations. Just one for now.
	{
		List<BeamLine> cur = beamLinesList[0];
/*		List<BeamLine> cur = beamLines;*/
		GameObject gameObject = null, createdObject;
		Vector3 position, rotation;
		for (int i = 0; i < cur.Count; i++)
		{
			rotation = Vector3.zero;
			if (cur[i].type == RAKE.BEAM_TYPE.H)
			{
				gameObject = H_BEAM;
				if (cur[i].start.z != cur[i].end.z) rotation.y = 90;
			}
			else if (cur[i].type == RAKE.BEAM_TYPE.PILLAR)
			{
				gameObject = PILLAR;
			}
			position = cur[i].start + cur[i].end;
			position.x /= 2.0f;
			position.y /= 2.0f;
			position.z /= 2.0f;
			createdObject = Instantiate(gameObject, position, Quaternion.Euler(rotation));
			createdObject.transform.parent = viewerParent.transform;
			/*Debug.Log(createdObject.transform.position + " || " + position);*/
		}
	}
}
