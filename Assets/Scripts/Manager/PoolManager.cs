using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    SmokeEffect,
    UnCoveredEffect,
    GoldEffect,
}


public sealed class PoolManager
{
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PoolManager().OnInit();
            }
            return _instance;
        }
    }

    private Dictionary<EffectType, GameObject> effectGOPrefabDic;
    private Dictionary<EffectType, List<GameObject>> effectPoolDic;
    //private List<GameObject> smokeEffect, unCoveredEffect, goldEffectx;
    private int maxListNumber = 10;


    private PoolManager OnInit()
    {
        //smokeEffect = new List<GameObject>();
        //unCoveredEffect = new List<GameObject>();
        //goldEffect = new List<GameObject>();
        //doorOpenEffect = new List<GameObject>();
        MainGameManager manager = MainGameManager.Instance;
        effectPoolDic = new Dictionary<EffectType, List<GameObject>>
        {
            { EffectType.SmokeEffect, new List<GameObject>() },
            { EffectType.UnCoveredEffect, new List<GameObject>() },
            { EffectType.GoldEffect, new List<GameObject>() },
        };
        effectGOPrefabDic = new Dictionary<EffectType, GameObject>
        {
            { EffectType.SmokeEffect, manager.SmokeEffect },
            { EffectType.UnCoveredEffect, manager.UncoveredEffect },
            { EffectType.GoldEffect, manager.GoldEffect },
        };

        return this;
    }

    public GameObject GetInstance(EffectType _type, Transform parent = null, bool isWorldPos = false)
    {
        lock (effectPoolDic[_type])
        {
            var list = effectPoolDic[_type];
            if (list.Count > 0)
            {
                GameObject tempGO = list[list.Count - 1];
                tempGO.SetActive(true);
                ResetInstance(tempGO);
                tempGO.transform.SetParent(parent, isWorldPos);
                list.Remove(tempGO);
                return tempGO;
            }
            else
            {
                var tempGO = GameObject.Instantiate(effectGOPrefabDic[_type], parent, isWorldPos);
                return tempGO;
            }
        }
    }

    public void ResetInstance(GameObject go)
    {
        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
            ps.Play();
        }

        foreach (Transform t in go.transform)
        {
            ParticleSystem tempPS = t.GetComponent<ParticleSystem>();
            if (tempPS != null)
            {
                tempPS.Stop();
                tempPS.Play();
            }
        }
    }

    public void StoreInstance(EffectType _type, GameObject go)
    {
        List<GameObject> list = effectPoolDic[_type];
        if (list.Count < maxListNumber)
        {
            go.SetActive(false);
            list.Add(go);
        }
        else
        {
            go.SetActive(false);
            GameObject.Destroy(go);
   
        }
    }

    public void DoDestory()
    {
        _instance = null;
    }
}
