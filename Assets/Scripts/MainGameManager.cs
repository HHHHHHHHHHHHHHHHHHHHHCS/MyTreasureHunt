using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    private static MainGameManager _instance;

    public static MainGameManager Instance { get { return _instance; } }

    [Header("元素预制体"),SerializeField]
    private GameObject bgElementPrefab;
    [Tooltip("边界预制体,顺序为:"), SerializeField,/*Space(50)*/]
    private GameObject[] borderElementsPrefabs;

    [Header("地图设置")]
    [SerializeField]
    private int w;
    [SerializeField]
    private int  h;

    private void Awake()
    {
        _instance = this;
    }
}
