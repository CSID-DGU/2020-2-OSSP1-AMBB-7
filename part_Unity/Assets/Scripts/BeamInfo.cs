using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamInfo : MonoBehaviour
{
    private Text beamInfoText;
    public string Info { get; set; }

    private void Start()
    {
        beamInfoText = GameObject.Find("BeamInfoWindow/Text").GetComponent<Text>();
    }

    private void OnMouseOver()
    {
        beamInfoText.text = Info;
    }
}
