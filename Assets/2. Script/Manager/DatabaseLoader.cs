using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

[System.Serializable]
public class Songinfo
{
    public int idx;
    public string title;
    public string answer;
    public string hint1;
    public string hint2;
    public string lylics;
}

public class DatabaseLoader : MonoSingleton<DatabaseLoader>
{
    public List<Songinfo> songinfoList = new List<Songinfo>();
    public const string jsonFilePath = "songtable";

    void Awake()
    {
        var jsonFilePath = Resources.Load<TextAsset>("songtable");
        songinfoList = JsonConvert.DeserializeObject<List<Songinfo>>(jsonFilePath.ToString());

        GameManager.Instance.SetSong(GetSonginfoByIdx(GameManager.Instance.Idx));
    }

    static T DeepCopy<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var tmp = JsonConvert.DeserializeObject<T>(json);

        return tmp;
    }

    public Songinfo GetSonginfoByIdx(int idx)
    {
        return DeepCopy(songinfoList.Find(x => x.idx == idx));
    }
}
