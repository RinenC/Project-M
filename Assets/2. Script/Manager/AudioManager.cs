using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioSource audioPlayer;

    public List<AudioClip> songlist;

    public bool isPlay = false;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void StartSong()
    {
        if (GameManager.Instance.isStartTimerEnd && GameManager.Instance.playTimer > 0 && !isPlay)
        {
            GetComponent<AudioSource>().clip = songlist[GameManager.Instance.Idx];
            audioPlayer.Play();
            isPlay = true;
        }
    }

    public void StopSong()
    {
        if (GameManager.Instance.isPlayTimerEnd)
        {
            audioPlayer.Stop();
            isPlay = false;
        }
    }
}
