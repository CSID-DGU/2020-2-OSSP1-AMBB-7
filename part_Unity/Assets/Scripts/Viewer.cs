using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
	[SerializeField] private GameObject H_BEAM;
	[SerializeField] private GameObject PILLAR;
	[SerializeField] private GameObject CONNECTOR;

	private List<BeamLine> beamLines;
	private GameObject viewerParent;
	private HashSet<Vector3> connectors;

	private List<List<BeamLine>> beamLinesList;
	// Start is called before the first frame update
	void Start()
	{
		viewerParent = GameObject.Find("ViewerParent");
		connectors = new HashSet<Vector3>();
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
/*		new BeamLine(new Vector3(0, 0, 0), new Vector3(0, 0, 1)),
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
		new BeamLine(new Vector3(1, 1, 1), new Vector3(1, 0, 1)),*/


		new BeamLine(new Vector3(0, 0, 0), new Vector3(0, 0, 2)),
		new BeamLine(new Vector3(0, 0, 0), new Vector3(2, 0, 0)),
		new BeamLine(new Vector3(0, 0, 0), new Vector3(0, 2, 0)),
		new BeamLine(new Vector3(2, 0, 0), new Vector3(2, 0, 2)),
		new BeamLine(new Vector3(2, 0, 0), new Vector3(2, 2, 0)),
		new BeamLine(new Vector3(0, 2, 0), new Vector3(2, 2, 0)),
		new BeamLine(new Vector3(0, 0, 2), new Vector3(2, 0, 2)),
		new BeamLine(new Vector3(0, 2, 0), new Vector3(0, 2, 2)),
		new BeamLine(new Vector3(0, 2, 2), new Vector3(0, 0, 2)),
		new BeamLine(new Vector3(0, 2, 2), new Vector3(2, 2, 2)),
		new BeamLine(new Vector3(2, 2, 2), new Vector3(2, 2, 0)),
		new BeamLine(new Vector3(2, 2, 2), new Vector3(2, 0, 2)),
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
			connectors.Add(cur[i].start);
			connectors.Add(cur[i].end);
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
			createdObject.transform.localScale = getScale(createdObject.transform.localScale, cur[i].end - cur[i].start, cur[i].type);
			/*Debug.Log(createdObject.transform.position + " || " + position);*/

			// Add Beam Info to object
			//createdObject.GetComponent<BeamInfo>().Info = cur[i].info; // REAL USE
			createdObject.GetComponent<BeamInfo>().Info = "Beam " + i; // FOR TESTING
		}
		foreach (Vector3 e in connectors)
		{
			gameObject = CONNECTOR;
			createdObject = Instantiate(gameObject, e, Quaternion.identity);
			createdObject.transform.parent = viewerParent.transform;
		}
	}

	private Vector3 getScale(Vector3 obj, Vector3 length, RAKE.BEAM_TYPE type)
	{
		Vector3 ret = obj;
		float len = Mathf.Sqrt(length.x * length.x + length.y * length.y + length.z * length.z);
		if (type == RAKE.BEAM_TYPE.H)
		{
			ret.x *= len;
		}
		else if (type == RAKE.BEAM_TYPE.PILLAR)
		{
			ret.y *= len;
		}
		return ret;
	}
}
