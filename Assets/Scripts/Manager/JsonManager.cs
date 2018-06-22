using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System;

public enum PlayerAttribute
{
    Lv,
    Hp,
    Armor,
    Key,
    Hoe,
    Tnt,
    Map,
    Gold,
}

public class JsonManager
{
    private static JsonManager _instance;
    public static JsonManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new JsonManager();
                _instance.CreateDefaultJson();
            }
            return _instance;
        }
    }

    #region Json Path
    private const string _fileDir = "Save";
    private const string _fileName = "GameData.json";
    private readonly string fileDir;
    private readonly string filePath;

    public JsonManager()
    {
#if UNITY_ANDROID
        fileDir = string.Format("{0}/{1}", Application.persistentDataPath,fileDir);
        filePath = string.Format("{0}/{1}", fileDir, fileName);
#elif UNITY_IPHONE
        fileDir = string.Format("{0}/{1}", Application.persistentDataPath,fileDir);
        filePath = string.Format("{0}/{1}", fileDir, fileName);
#else
        fileDir = string.Format("{0}/{1}", Application.dataPath, _fileDir);
        filePath = string.Format("{0}/{1}", fileDir, _fileName);
#endif
    }
    #endregion


    #region Base Function
    private string Read()
    {
        string result = "";
        if (CheckGameData())
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                result = sr.ReadToEnd();
            }
        }
        return result;
    }


    private T GetData<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    private string MakeJson(object o)
    {
        return JsonConvert.SerializeObject(o);
    }

    private string SaveData(object gameData)
    {
        if (!Directory.Exists(fileDir))
        {
            Directory.CreateDirectory(fileDir);
        }
        string jsonStr = MakeJson(gameData);
        using (StreamWriter sw = File.CreateText(filePath))
        {
            sw.Write(jsonStr);
        }
        return jsonStr;
    }

    private string UpdateData(object gameData)
    {
        return SaveData(gameData);
    }

    private bool CheckGameData()
    {
        if (File.Exists(filePath))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region High Function


    public bool CreateDefaultJson(bool needCheck = true)
    {
        if (needCheck&&CheckGameData())
        {
            return false;
        }

        Dictionary<PlayerAttribute, int> playerDic = new Dictionary<PlayerAttribute, int>();
        playerDic.Add(PlayerAttribute.Lv, 1);
        playerDic.Add(PlayerAttribute.Hp, 3);
        for(int i=2;i<Enum.GetValues(typeof( PlayerAttribute)).Length;i++)
        {
            playerDic.Add((PlayerAttribute)i, 0);
        }

        SaveData(playerDic);
        return true;
    }

    public Dictionary<PlayerAttribute,int> ReadData()
    {
        return JsonConvert.DeserializeObject<Dictionary<PlayerAttribute, int>>(Read());
    }

    public void UpdateData(PlayerAttribute attr ,int data)
    {
        JObject jo = JObject.Parse(Read());
        jo[attr] = data;
        SaveData(jo);
    }

    public void UpdateData(Dictionary<PlayerAttribute, int> dic)
    {
        SaveData(dic);
    }

    public Dictionary<PlayerAttribute, int> CleanData()
    {
        CreateDefaultJson(false);
        return ReadData();
    }
    #endregion

}