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
    [Header("Flag预制体"), SerializeField]
    private GameObject flagElement;
    public GameObject FlagElement { get { return flagElement; } }

    [Header("特效"), SerializeField]
    private GameObject smokeEffect;
    public GameObject SmokeEffect { get { return smokeEffect; } }
    [SerializeField]
    private GameObject uncoveredEffect;
    public GameObject UncoveredEffect { get { return uncoveredEffect; } }

    [Header("泥土图片资源"), SerializeField]
    private Sprite[] coverTiledSprites;
    public Sprite[] CoverTiledSprites { get { return coverTiledSprites; } }
    [Header("陷阱图片资源"), SerializeField]
    private Sprite[] trapSprites;
    public Sprite[] TrapSprites { get { return trapSprites; } }
    [Header("数字图片资源"), SerializeField]
    private Sprite[] numberSprites;
    public Sprite[] NumberSprites { get { return numberSprites; } }

    [Header("关卡设置"), SerializeField]
    private int w;
    [SerializeField]
    private int h;
    [SerializeField]
    private float minTrapProbability;
    [SerializeField]
    private float maxTrapProbability;

    private BaseElement[,] mapArray;

    private void Awake()
    {
        Instance = this;
        CreateMap();
        InitMap();
        ResetCamera();
    }

    private void CreateMap()
    {
        var holder = new GameObject("Element_Holder").transform;
        var bgParent = new GameObject("Backgrounds").transform;
        bgParent.SetParent(holder);
        var coverParent = new GameObject("Covers").transform;
        coverParent.SetParent(holder);

        mapArray = new BaseElement[w, h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Instantiate(bgElementPrefab, new Vector3(i, j, 0)
                    , Quaternion.identity, bgParent);
                mapArray[i, j] = Instantiate(baseElement, new Vector3(i, j, 0)
                    , Quaternion.identity, coverParent);
                mapArray[i, j].OnInit();
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


    private void InitMap()
    {
        List<int> avaliableIndex = new List<int>();
        for (int i = 0; i < w * h; i++)
        {
            avaliableIndex.Add(i);
        }
        SpawnTrap(avaliableIndex);
        SpawnNumber(avaliableIndex);
    }

    private void SpawnTrap(List<int> _avaliableIndex)
    {
        float trapPro = Random.Range(minTrapProbability, maxTrapProbability);
        int trapNum = (int)(_avaliableIndex.Count * trapPro);

        for (int i = 0; i < trapNum && _avaliableIndex.Count > 0; i++)
        {
            int temp = _avaliableIndex[Random.Range(0, _avaliableIndex.Count)];
            SetElement<TrapElement>(temp);
            _avaliableIndex.Remove(temp);
        }
    }

    private void SpawnNumber(List<int> _avaliableIndex)
    {
        for (int i = _avaliableIndex.Count - 1; i >= 0; i--)
        {
            int temp = _avaliableIndex[i];
            SetElement<NumberElement>(temp);
            _avaliableIndex.Remove(temp);
        }

    }

    private void GetPosByIndex(int index, out int x, out int y)
    {
        y = index / w;
        x = index - y * w;
    }

    private int GetIndexByPos(int x, int y)
    {
        return w * y + x;
    }

    private void SetElement<T>(int index) where T : BaseElement
    {
        int x, y;
        GetPosByIndex(index, out x, out y);
        var temp = mapArray[x, y].gameObject.AddComponent<T>();
        temp.OnInit();
        Destroy(mapArray[x, y]);
        mapArray[x, y] = temp;
    }

    private void ResetCamera()
    {
        Camera camera = Camera.main;
        Camera.main.orthographicSize = (h + 3) / 2f;
        camera.transform.position = new Vector3((w - 1) / 2f, (h - 1) / 2f, -10);
    }

    public Sprite GetNumberSpriteByPos(int x, int y)
    {
        return NumberSprites[CountAdjcentTraps(x, y)];
    }

    /// <summary>
    /// 计算周围八个位置的陷阱
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int CountAdjcentTraps(int x, int y)
    {
        int count = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if ((i != x || j != y) && (i >= 0 && j >= 0)
                    && (i < w && j < h))
                {
                    if (mapArray[i, j].ElementContent == ElementContent.Trap)
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    public void FloodFillElement(int x, int y, bool[,] visited = null)
    {
        if (visited == null)
        {
            visited = new bool[w, h];
        }

        if ((x >= 0 && y >= 0) && (x < w && y < h)
            && !visited[x, y])
        {
            visited[x, y] = true;
            if (mapArray[x, y].ElementType == ElementType.CantCovered) return;

            ((SingleCoverElement)mapArray[x, y]).UncoveredElement();

            if (CountAdjcentTraps(x, y) > 0) return;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if ((i != x || j != y) && (i >= 0 && j >= 0)
                        && (i < w && j < h))
                    {
                        FloodFillElement(i, j, visited);
                    }
                }
            }
        }
    }

    public void UncoveredAdjacentElements(int x, int y)
    {
        int marked = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if ((i != x || j != y) && (i >= 0 && j >= 0)
                    && (i < w && j < h))
                {
                    if (mapArray[i, j].ElementState == ElementState.Marked
                        || (mapArray[i, j].ElementState == ElementState.UnCovered
                        && mapArray[i, j].ElementContent == ElementContent.Trap))
                    {
                        marked++;
                    }
                }
            }
        }

        if (CountAdjcentTraps(x, y) == marked)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if ((i != x || j != y) && (i >= 0 && j >= 0)
                        && (i < w && j < h))
                    {
                        if (mapArray[i, j].ElementState != ElementState.Marked)
                        {
                            mapArray[i, j].OnPlayerStand();
                        }
                    }
                }
            }
        }
    }
}
