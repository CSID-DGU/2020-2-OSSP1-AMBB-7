using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
public class FileController : MonoBehaviour
{

    [SerializeField] public Button floorButton;
    [SerializeField] public Text floorText;

    [SerializeField] public Button frontButton;
    [SerializeField] public Text frontText;
    
    [SerializeField] public Button leftSideButton;
    [SerializeField] public Text leftSideText;
    
    [SerializeField] public Button rightSideButton;
    [SerializeField] public Text rightSideText;
    
    [SerializeField] public Button RoofFloorButton;
    [SerializeField] public Text RoofFloorText;
    
    [SerializeField] public Button RearButton;
    [SerializeField] public Text RearText;
    
    private ExtensionFilter[] extensions;

    // Start is called before the first frame update
    void Start()
    {
        // Read Only xls file
        extensions = new[] {
            new ExtensionFilter("Exel Files", "xls"),
        };
        // add Button onClick Listener
        /*        Button[] buttons = new Button[] {
                    floorButton,
                    frontButton,
                    leftSideButton,
                    rightSideButton,
                    RoofFloorButton,
                    RearButton
                };
                Text[] texts = new Text[] {
                    floorText,
                    frontText,
                    leftSideText,
                    rightSideText,
                    RoofFloorText,
                    RearText,
                };
                for (int i = 0; i < 6; i++)
                {
                    Debug.Log(i + " " + buttons.Length + " " + texts.Length);
                    buttons[i].onClick.AddListener(delegate { selectButton(texts[i]); });
                }*/
        floorButton.onClick.AddListener(delegate { selectButton(floorText); });
        frontButton.onClick.AddListener(delegate { selectButton(frontText); });
        leftSideButton.onClick.AddListener(delegate { selectButton(leftSideText); });
        rightSideButton.onClick.AddListener(delegate { selectButton(rightSideText); });
        RoofFloorButton.onClick.AddListener(delegate { selectButton(RoofFloorText); });
        RearButton.onClick.AddListener(delegate { selectButton(RearText); });
    }

    private void selectButton(Text text)
	{
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (paths.Length > 0) text.text = paths[0];
	}
}
