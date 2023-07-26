using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPopup : MonoBehaviour
{
    public string popupType;
    public TMP_InputField inputfield;
    public string tmp = "";

    public void SetName()
    {
        tmp = this.inputfield.text;
        this.inputfield.text = "";
        GameManager.Instance.playerName = tmp;
    }

    public void Enter()
    {
        if (popupType == "Name")
            SetName();

        Destroy(this.gameObject);
    }
}
