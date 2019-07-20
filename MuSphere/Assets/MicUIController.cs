using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MicUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button[] micOptions;
    public Button buttonPrefab;
    public AudioController AC;

    CanvasGroup G;

    int numButtons;

    int yPos;

    public void Start() {
        //G = this.GetComponent<CanvasGroup>();

        InstantiateButtons();
    }

    public void InstantiateButtons() {

        numButtons = 0;
        yPos = 0;

        string[] buttonNames = AC.getMics();

        numButtons = buttonNames.Length;

        micOptions = new Button[numButtons];

        for (int i = 0; i < numButtons; i++){
            
            micOptions[i] = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as Button;

            var rectTransform = micOptions[i].GetComponent<RectTransform>();
            rectTransform.SetParent(this.transform);

            rectTransform.anchoredPosition3D = new Vector3(0, -yPos, 0);

            yPos += 50;

            //Set text
            Text t = micOptions[i].GetComponentInChildren(typeof(Text)) as Text;
            t.text = buttonNames[i];

            //Set OnClickHandlers
            ButtonEventHandler beh = micOptions[i].gameObject.AddComponent(typeof(ButtonEventHandler)) as ButtonEventHandler;
            beh.MUIC = this;

        }

        hide();

    }
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        show();
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hide();
    }

    void show() {

        foreach (Button button in micOptions) {
            Image ButtonBackground = button.GetComponent<Image>();
            Color color = ButtonBackground.color;
            color.a = 1f;
            ButtonBackground.color = color;
        }

    }

    void hide() {
          foreach (Button button in micOptions) {
            Image ButtonBackground = button.GetComponent<Image>();
            Color color = ButtonBackground.color;
            color.a = 0f;
            ButtonBackground.color = color;
        }
    }

    public void onButtonClick(Button button) {
        for (int i = 0; i < numButtons; i++){
            if (button == micOptions[i]) {
                AC.changeMic(i);
            }
        }
    }
}
