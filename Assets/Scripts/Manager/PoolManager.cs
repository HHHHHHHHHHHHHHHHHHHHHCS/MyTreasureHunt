using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    SmokeEffect,
    UnCoveredEffect,
    GoldEffect,
    DoorOopenEffect

}


public sealed class PoolManager 
{
    private static PoolManager _instance;
    public static PoolManager Instance {
        get
        {
            if(_instance==null)
            {
                _instance = new PoolManager().OnInit();
            }
            return _instance;
        }
    }

    private Dictionary<EffectType, GameObject> effectGOPrefabDic;
    private Dictionary<EffectType, List<GameObject>> effectPoolDic ;
    private List<GameObject> smokeEffect, unCoveredEffect, goldEffect, doorOpenEffect;
    private int maxListNumber = 10;


    private PoolManager OnInit()
    {
        //smokeEffect = new List<GameObject>();
        //unCoveredEffect = new List<GameObject>();
        //goldEffect = new List<GameObject>();
        //doorOpenEffect = new List<GameObject>();
        effectPoolDic = new Dictionary<EffectType, List<GameObject>>
        {
            { EffectType.SmokeEffect, new List<GameObject>() },
            { EffectType.UnCoveredEffect, new List<GameObject>() },
            { EffectType.GoldEffect, new List<GameObject>() },
            { EffectType.DoorOopenEffect, new List<GameObject>() }
        };
        effectGOPrefabDic = new Dictionary<EffectType, GameObject>();
        return this;
    }
}
