using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public void SceneClickListener(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void SceneChangeForMainScene()
	{
        string floorText = GameObject.Find("selectedMsgFloor").GetComponent<UnityEngine.UI.Text>().text;
        string frontText = GameObject.Find("selectedMsgFront").GetComponent<UnityEngine.UI.Text>().text;
        string leftText = GameObject.Find("selectedMsgLeftSide").GetComponent<UnityEngine.UI.Text>().text;
        string rightText = GameObject.Find("selectedMsgFloorRightSide").GetComponent<UnityEngine.UI.Text>().text;
        string roofText = GameObject.Find("selectedMsgRoofFloor").GetComponent<UnityEngine.UI.Text>().text;
        string rearText = GameObject.Find("selectedMsgRear").GetComponent<UnityEngine.UI.Text>().text;
        PlayerPrefs.SetString(StaticVariable.FLOOR, floorText);
        PlayerPrefs.SetString(StaticVariable.FRONT, frontText);
        PlayerPrefs.SetString(StaticVariable.LEFT, leftText);
        PlayerPrefs.SetString(StaticVariable.RIGHT, rightText);
        PlayerPrefs.SetString(StaticVariable.ROOF, roofText);
        PlayerPrefs.SetString(StaticVariable.REAR, rearText);
        SceneClickListener(1);
	}
}