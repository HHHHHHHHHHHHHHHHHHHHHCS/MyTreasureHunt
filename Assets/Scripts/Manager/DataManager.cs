using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DataManager
{
    private static DataManager _instance;

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataManager().OnInit();

            }
            return _instance;
        }
    }

    private Dictionary<PlayerAttribute, int> dataDic;
    private int w, h;


    private DataManager OnInit()
    {
        dataDic = JsonManager.Instance.ReadData();
        w = 20 + dataDic[PlayerAttribute.Lv] * 3;
        h = Random.Range(9, 13);
        return this;
    }

    public void UpdateData()
    {
        JsonManager.Instance.UpdateData(dataDic);
    }

    public int ReadData(PlayerAttribute attr)
    {
        return dataDic[attr];
    }

    public void CleanData()
    {
        dataDic= JsonManager.Instance.CleanData();
    }
}
