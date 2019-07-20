using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEventHandler : MonoBehaviour
{

    public MicUIController MUIC;

    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(sendEventToMicUIController);
    }

    void sendEventToMicUIController() {
        MUIC.onButtonClick(button);
    }
}
