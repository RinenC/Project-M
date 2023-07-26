using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupButton : MonoBehaviour
{
    public GameObject inputPopup;
    public GameObject popup;
    public GameObject ui;

    public void OnCreateInputPopup(string type)
    {
        inputPopup.GetComponent<InputPopup>().popupType = type;
        GameObject prefabPopup = Instantiate(inputPopup, ui.transform);
    }
}
