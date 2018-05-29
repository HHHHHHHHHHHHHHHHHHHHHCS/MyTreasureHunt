using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance { get; private set; }

    [Header("背景预制体"), SerializeField]
    private GameObject bgElementPrefab;
    [Tooltip("边界预制体,顺序为:\n上,下,左,右,左上,右上,左下,右下")
        , SerializeField,/*Space(50)*/]
    private GameObject[] borderElementsPrefabs;
    [Header("泥土预制体"), SerializeField]
    private BaseElement baseElement;

    [Header("图片资源"), SerializeField]
    private Sprite[] coverTiledSprites;
    public Sprite[] CoverTiledSprites { get { return coverTiledSprites; } }

    [Header("关卡设置"), SerializeField]
    private int w;
    [SerializeField]
    private int h;

    private BaseElement[,] mapArray;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateMap();
        ResetCamera();
    }

    private void CreateMap()
    {
        var holder = new GameObject("Element_Holder").transform;
        var bgParent = new GameObject("Backgrounds").transform;
        bgParent.SetParent(holder);
        var coverParent = new GameObject("Covers").transform;
        coverParent.SetParent(holder);

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Instantiate(bgElementPrefab, new Vector3(i, j, 0)
                    , Quaternion.identity, bgParent);
                mapArray[i, j] = Instantiate(baseElement, new Vector3(i, j, 0)
                    , Quaternion.identity, coverParent);
            }
        }

        for (int i = 0; i < w; i++)
        {
            Instantiate(borderElementsPrefabs[0], new Vector3(i, h + 0.25f, 0), Quaternion.identity, bgParent);
            Instantiate(borderElementsPrefabs[1], new Vector3(i, -1.25f, 0), Quaternion.identity, bgParent);
        }

        for (int i = 0; i < h; i++)
        {
            Instantiate(borderElementsPrefabs[2], new Vector3(-1.25f, i, 0), Quaternion.identity, bgParent);
            Instantiate(borderElementsPrefabs[3], new Vector3(w + 0.25f, i, 0), Quaternion.identity, bgParent);
        }
        Instantiate(borderElementsPrefabs[4], new Vector3(-1.25f, h + 0.25f, 0), Quaternion.identity, bgParent);
        Instantiate(borderElementsPrefabs[5], new Vector3(w + 0.25f, h + 0.25f, 0), Quaternion.identity, bgParent);
        Instantiate(borderElementsPrefabs[6], new Vector3(-1.25f, -1.25f, 0), Quaternion.identity, bgParent);
        Instantiate(borderElementsPrefabs[7], new Vector3(w + 0.25f, -1.25f, 0), Quaternion.identity, bgParent);
    }

    private void ResetCamera()
    {
        Camera camera = Camera.main;
        Camera.main.orthographicSize = (h + 3) / 2f;
        camera.transform.position = new Vector3((w - 1) / 2f, (h - 1) / 2f, -10);
    }
}
