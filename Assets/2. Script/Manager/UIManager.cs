using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    #region Main
    [Header("Main UI")]
    public GameObject go;
    #endregion
    #region Play
    [Header("Play UI")]
    public TMP_Text queue_song;
    public TMP_Text left_time;
    public TMP_Text hints;
    public TMP_Text answer;
    public TMP_Text lylics;
    public TMP_Text chat;
    public TMP_Text inputfield;
    public Button enterText;

    #endregion

    private void Start()
    {

    }

    private void Update()
    {
        PlayFlow();
    }

    public void SetSceneidx(int idx)
    {
        SceneManager.LoadScene(idx);
    }

    public void SetUI()
    {
        SetQueueSong();
        SetLeftTime();
        SetLylics();
    }

    public void PlayFlow()
    {
        if (ChatManager.Instance.isConnect) //채팅이 연결되면
        {
            if (GameManager.Instance.startTimer > 0)
            {
                SetStartTime();
                GameManager.Instance.startTimer -= Time.deltaTime; //시작 전 타이머가 진행된다.
            }

            // * 본 게임

            if (GameManager.Instance.startTimer <= 0) //시작 전 타이머가 0이 되면
            {
                GameManager.Instance.isStartTimerEnd = true; //타이머가 끝났다고 알린다.
            }

            if (GameManager.Instance.isStartTimerEnd) //타이머가 끝나면
            {
                SetUI(); //UI를 출력하고
                AudioManager.Instance.StartSong(); //노래를 시작한다.
                if(ChatManager.Instance.isSend)
                {
                    if (ChatManager.Instance.inputFieldChat.text == GameManager.Instance.songinfo.answer)
                        lylics.text = GameManager.Instance.playerName + "님이 정답을 맞추셨습니다 ! ! !";
                }

                if(GameManager.Instance.isPlayTimerEnd) //플레이 타임이 끝나면
                {
                    AudioManager.Instance.StopSong(); //노래를 멈춘다.
                    NextSong();
                }
            }

            // * 본 게임
        }
    }

    public void SetQueueSong()
    {
        queue_song.text = "남은곡" + " [ " + (DatabaseLoader.Instance.songinfoList.Count - GameManager.Instance.songinfo.idx) + " / " + DatabaseLoader.Instance.songinfoList.Count + " ] ";
    }

    public void SetStartTime()
    {
        left_time.text = (int)GameManager.Instance.startTimer + "초 후 게임이 시작됩니다.";
    }

    public void SetLeftTime()
    {
        left_time.text = "- " + (int)GameManager.Instance.playTimer + " 초 -";

        if (GameManager.Instance.isStartTimerEnd)
        {
            if (GameManager.Instance.playTimer > 0)
                GameManager.Instance.playTimer -= Time.deltaTime;
            if (GameManager.Instance.playTimer <= 0)
            {
                GameManager.Instance.isPlayTimerEnd = true;
            }
        }
    }

    public void NextSong()
    {
        if(GameManager.Instance.isPlayTimerEnd)
        {
            if (GameManager.Instance.nextTimer > 0)
            {
                GameManager.Instance.nextTimer -= Time.deltaTime;
                lylics.text = (int)GameManager.Instance.nextTimer + "초 후 다음 곡이 시작됩니다.";
            }
            if(GameManager.Instance.nextTimer <= 0)
            {
                GameManager.Instance.playTimer = 61f;
                GameManager.Instance.nextTimer = 6f;
                GameManager.Instance.Idx++;
                GameManager.Instance.SetSong(DatabaseLoader.Instance.GetSonginfoByIdx(GameManager.Instance.Idx));
                GameManager.Instance.isPlayTimerEnd = false;
            }
        }
    }

    public void SetHint()
    {
        hints.text = "힌트 1 : " + GameManager.Instance.songinfo.hint1 + "\n" + "힌트 2 : " + GameManager.Instance.songinfo.hint2;
    }

    public void SetAnswer()
    {
        answer.text = "정답 : " + GameManager.Instance.songinfo.answer;
    }

    public void SetLylics()
    {
        string[] split_text;
        split_text = GameManager.Instance.songinfo.lylics.Split('/');
        lylics.text = "" + split_text[0] + "\n" + split_text[1] /*+ "\n" + split_text[2]*/;
    }
}
