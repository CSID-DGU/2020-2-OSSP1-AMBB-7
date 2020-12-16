using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamInfo : MonoBehaviour
{
    private Text beamInfoText;
    private MeshRenderer mesh;
    private Color original = new Color(0.8f, 0.8f, 0.8f);
    private Color highlight = new Color(0.4f, 1f, 0.4f);
    public string Info { get; set; }

    private void Start()
    {
        beamInfoText = GameObject.Find("BeamInfoWindow/Text").GetComponent<Text>();
        mesh = GetComponentInChildren<MeshRenderer>();
    }

    private void OnMouseEnter()
    {
        beamInfoText.text = Info;
        mesh.material.color = highlight;
    }


    private void OnMouseExit()
    {
        beamInfoText.text = "";
        mesh.material.color = original;
    }


    private void Update() // FOR DEBUGGING
    {
        Debug.DrawLine(gameObject.transform.position, Vector3.right);
    }
}
