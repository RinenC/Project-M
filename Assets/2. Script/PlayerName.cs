using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public TMP_Text nameGO;
    private string playerName;
    
    public string GetPlayerName()
    {
        return this.name;
    }

    public void SetPlayerName()
    {
        this.name = GameManager.Instance.playerName;
    }

    public string _PlayerName
    {
        get { return this.name; }
        set { this.playerName = value; }
    }

    public void Update()
    {
        nameGO.text = GameManager.Instance.playerName;
    }
}
