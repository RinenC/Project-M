using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Songinfo songinfo;
    
    public PlayerControl playercontrol;

    public string playerName = "Player";

    public float startTimer = 6f;
    public bool isStartTimerEnd = false; //���� ����

    public float playTimer = 61f;
    public bool isPlayTimerEnd = false; //���� ��

    public float nextTimer = 6f;

    private int idx = 0;

    public void SetSong(Songinfo _songinfo)
    {
        songinfo = _songinfo;

        Debug.Log(songinfo.title);
    }

    public int GetIdx()
    {
        return this.idx;
    }

    public void SetIdx(int idx)
    {
        this.idx = idx;
    }

    public int Idx
    {
        get { return this.idx; }
        set { this.idx = value; }
    }
}
