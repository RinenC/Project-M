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
        if (ChatManager.Instance.isConnect) //ä���� ����Ǹ�
        {
            if (GameManager.Instance.startTimer > 0)
            {
                SetStartTime();
                GameManager.Instance.startTimer -= Time.deltaTime; //���� �� Ÿ�̸Ӱ� ����ȴ�.
            }

            // * �� ����

            if (GameManager.Instance.startTimer <= 0) //���� �� Ÿ�̸Ӱ� 0�� �Ǹ�
            {
                GameManager.Instance.isStartTimerEnd = true; //Ÿ�̸Ӱ� �����ٰ� �˸���.
            }

            if (GameManager.Instance.isStartTimerEnd) //Ÿ�̸Ӱ� ������
            {
                SetUI(); //UI�� ����ϰ�
                AudioManager.Instance.StartSong(); //�뷡�� �����Ѵ�.
                if(ChatManager.Instance.isSend)
                {
                    if (ChatManager.Instance.inputFieldChat.text == GameManager.Instance.songinfo.answer)
                        lylics.text = GameManager.Instance.playerName + "���� ������ ���߼̽��ϴ� ! ! !";
                }

                if(GameManager.Instance.isPlayTimerEnd) //�÷��� Ÿ���� ������
                {
                    AudioManager.Instance.StopSong(); //�뷡�� �����.
                    NextSong();
                }
            }

            // * �� ����
        }
    }

    public void SetQueueSong()
    {
        queue_song.text = "������" + " [ " + (DatabaseLoader.Instance.songinfoList.Count - GameManager.Instance.songinfo.idx) + " / " + DatabaseLoader.Instance.songinfoList.Count + " ] ";
    }

    public void SetStartTime()
    {
        left_time.text = (int)GameManager.Instance.startTimer + "�� �� ������ ���۵˴ϴ�.";
    }

    public void SetLeftTime()
    {
        left_time.text = "- " + (int)GameManager.Instance.playTimer + " �� -";

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
                lylics.text = (int)GameManager.Instance.nextTimer + "�� �� ���� ���� ���۵˴ϴ�.";
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
        hints.text = "��Ʈ 1 : " + GameManager.Instance.songinfo.hint1 + "\n" + "��Ʈ 2 : " + GameManager.Instance.songinfo.hint2;
    }

    public void SetAnswer()
    {
        answer.text = "���� : " + GameManager.Instance.songinfo.answer;
    }

    public void SetLylics()
    {
        string[] split_text;
        split_text = GameManager.Instance.songinfo.lylics.Split('/');
        lylics.text = "" + split_text[0] + "\n" + split_text[1] /*+ "\n" + split_text[2]*/;
    }
}
