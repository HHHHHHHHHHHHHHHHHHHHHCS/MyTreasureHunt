using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    private struct CloseAreaInfo
    {
        public int x, y, sx, ex, sy, ey;
        public int doorType;
        public Vector2Int doorPos;
        public Vector2Int toolPos, goldPos;
        public ToolElement t;
        public GoldElement g;
        public int innerCount, goldNumber;
    }


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
    [Header("错误的预制体"), SerializeField]
    private GameObject errorElement;


    [Header("特效"), SerializeField]
    private GameObject smokeEffect;
    public GameObject SmokeEffect { get { return smokeEffect; } }
    [SerializeField]
    private GameObject uncoveredEffect;
    public GameObject UncoveredEffect { get { return uncoveredEffect; } }
    [SerializeField]
    private GameObject goldEffect;
    public GameObject GoldEffect { get { return goldEffect; } }

    [Header("泥土图片资源"), SerializeField]
    private Sprite[] coverTiledSprites;
    public Sprite[] CoverTiledSprites { get { return coverTiledSprites; } }
    [Header("陷阱图片资源"), SerializeField]
    private Sprite[] trapSprites;
    public Sprite[] TrapSprites { get { return trapSprites; } }
    [Header("数字图片资源"), SerializeField]
    private Sprite[] numberSprites;
    public Sprite[] NumberSprites { get { return numberSprites; } }
    [Header("道具图片资源"), SerializeField]
    private Sprite[] toolSprites;
    public Sprite[] ToolSprites { get { return toolSprites; } }
    [Header("金币图片资源"), SerializeField]
    private Sprite[] goldSprites;
    public Sprite[] GoldSprites { get { return goldSprites; } }
    [Header("大墙图片资源"), SerializeField]
    private Sprite[] bigWallSprites;
    public Sprite[] BigWallSprites { get { return bigWallSprites; } }
    [Header("小墙图片资源"), SerializeField]
    private Sprite[] smallWallSprites;
    public Sprite[] SmallWallSprites { get { return smallWallSprites; } }
    [Header("敌人图片资源"), SerializeField]
    private Sprite[] enemyWallSprites;
    public Sprite[] EnemyWallSprites { get { return enemyWallSprites; } }
    [Header("门图片资源"), SerializeField]
    private Sprite doorWallSprite;
    public Sprite DoorWallSprite { get { return doorWallSprite; } }
    [Header("出口图片资源"), SerializeField]
    private Sprite exitSprite;
    public Sprite ExitSprite { get { return exitSprite; } }

    [Header("关卡设置"), SerializeField]
    private int w;
    [SerializeField]
    private int h;
    [SerializeField]
    private float minTrapProbability;
    [SerializeField]
    private float maxTrapProbability;
    [SerializeField]
    private float uncoverProbability;
    public float UncoverProbability { get { return uncoverProbability; } }
    [SerializeField]
    private int standAreaW;
    public float StandAreaW { get { return standAreaW; } }
    [SerializeField]
    private int obstacleAreaW;
    public int ObstacleAreaW { get { return obstacleAreaW; } }
    public float ObstacleAreaNum { get; private set; }


    public BaseElement[,] MapArray { get; private set; }

    private void Awake()
    {
        Instance = this;
        ObstacleAreaNum = (w - (StandAreaW - 3)) / ObstacleAreaW;
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

        MapArray = new BaseElement[w, h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Instantiate(bgElementPrefab, new Vector3(i, j, 0)
                    , Quaternion.identity, bgParent);
                MapArray[i, j] = Instantiate(baseElement, new Vector3(i, j, 0)
                    , Quaternion.identity, coverParent);
                MapArray[i, j].OnInit();
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
        SpawnExit(avaliableIndex);
        SpawnTrap(avaliableIndex);
        SpawnTool(avaliableIndex);
        SpawnGold(avaliableIndex);
        SpawnNumber(avaliableIndex);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_type">0与边界闭合 1自闭和</param>
    /// <param name="_nowArea"></param>
    /// <param name="_closeAreaInfo"></param>
    private CloseAreaInfo GenerateCloseAreaInfo(int _type, int _nowArea)
    {
        CloseAreaInfo closeAreaInfo = new CloseAreaInfo();
        switch (_type)
        {
            case 0:
                closeAreaInfo.x = UnityEngine.Random.Range(3, obstacleAreaW - 3);
                closeAreaInfo.y = UnityEngine.Random.Range(3, h - 3);
                closeAreaInfo.sx = standAreaW + _nowArea * obstacleAreaW + 1;
                closeAreaInfo.ex = closeAreaInfo.sx + closeAreaInfo.x;
                closeAreaInfo.doorType = UnityEngine.Random.Range(4, 8);
                break;
            case 1:
                break;
        }
        return closeAreaInfo;
    }

    private void SpawnExit(List<int> _avaliableIndex)
    {
        if (_avaliableIndex.Count >= 4)
        {
            float x = w - 1.5f;
            float y = UnityEngine.Random.Range(1, h) - 0.5f;

            Vector2[] vec2 = new[] {new Vector2(+0.5f,-0.5f), new Vector2(+0.5f,+0.5f)
            , new Vector2(-0.5f, -0.5f) ,new Vector2(-0.5f, +0.5f)};

            BaseElement exit = SetElement<ExitElement>((int)(x + vec2[0].x), (int)(y + vec2[0].y));
            exit.transform.position = new Vector3(x, y, 0);
            exit.GetComponent<BoxCollider2D>().size = new Vector2(2, 2);
            //Destroy(exit.GetComponent<BoxCollider2D>());
            //exit.gameObject.AddComponent<BoxCollider2D>();

            for (int i = 0; i < vec2.Length; i++)
            {
                _avaliableIndex.Remove(GetIndexByPos((int)(x + vec2[i].x), (int)(y + vec2[i].y)));
                if (i != 0)
                {
                    Destroy(MapArray[(int)(x + vec2[i].x), (int)(y + vec2[i].y)].gameObject);
                    MapArray[(int)(x + vec2[i].x), (int)(y + vec2[i].y)] = exit;
                }
            }
        }
    }

    private void SpawnObstacleArea(List<int> _avaliableIndex)
    {
        for (int i = 0; i < ObstacleAreaNum; i++)
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                CreateCloseArea(i, _avaliableIndex);
            }
            else
            {
                CreateRandomWall(i, _avaliableIndex);
            }
        }
    }

    private void CreateCloseArea(int nowArea, List<int> _avaliableIndex)
    {
        int shape = UnityEngine.Random.Range(0, 2);
        CloseAreaInfo info;
        switch (shape)
        {
            case 0:
                info = GenerateCloseAreaInfo(0, nowArea);
                int dir = UnityEngine.Random.Range(0, 4);
                switch (dir)
                {
                    case 0:
                        info.doorPos = UnityEngine.Random.value < 0.5f
                            ? new Vector2Int(UnityEngine.Random.Range(info.sx + 1, info.ex), info.y)
                            : new Vector2Int(UnityEngine.Random.value < 0.5f ? info.sx : info.ex, UnityEngine.Random.Range(info.y, h));
                        CreateULShapeAreaDoor(info, _avaliableIndex);
                        for(int i=h-1;i>info.y;i--)
                        {
                            int index1 = GetIndexByPos(info.sx, i);
                            if (_avaliableIndex.Contains(index1))
                            {
                                _avaliableIndex.Remove(index1);
                                SetElement<BigWallElement>(index1); 
                            }

                            int index2 = GetIndexByPos(info.ex, i);
                            if (_avaliableIndex.Contains(index1))
                            {
                                _avaliableIndex.Remove(index2);
                                SetElement<BigWallElement>(index2);
                            }
                        }

                        for (int i =info.sx; i <=info.ex; i++)
                        {
                            int index = GetIndexByPos(i, info.y);
                            if (_avaliableIndex.Contains(index))
                            {
                                _avaliableIndex.Remove(index);
                                SetElement<BigWallElement>(index);
                            }
                        }
                        break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                }

                break;
            case 1:
                break;
        }

    }

    private void CreateRandomWall(int nowArea, List<int> _avaliableIndex)
    {

    }

    private void CreateULShapeAreaDoor(CloseAreaInfo _info, List<int> _avaliableIndex)
    {
        int index = GetIndexByPos(_info.doorPos.x, _info.doorPos.y);
        _avaliableIndex.Remove(GetIndexByPos(_info.doorPos.x, _info.doorPos.y));
        SetElement<DoorElement>(index);
    }

    private void CreateCloseAreaRewards(CloseAreaInfo _info, List<int> _avaliableIndex)
    {
        _info.innerCount = _info.x * _info.y;
        _info.goldNumber = UnityEngine.Random.Range(1, UnityEngine.Random.value < 0.5f ? _info.innerCount + 1 : _info.innerCount / 2);
        for(int i = 0;i<_info.goldNumber;i++)
        {

        }
    }

    private void SpawnTrap(List<int> _avaliableIndex)
    {
        float trapPro = UnityEngine.Random.Range(minTrapProbability, maxTrapProbability);
        int trapNum = (int)(_avaliableIndex.Count * trapPro);

        for (int i = 0; i < trapNum && _avaliableIndex.Count > 0; i++)
        {
            int temp = _avaliableIndex[UnityEngine.Random.Range(0, _avaliableIndex.Count)];
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

    private void SpawnTool(List<int> _avaliableIndex)
    {
        for (int i = 0; i < 10 && _avaliableIndex.Count > 0; i++)
        {
            int temp = _avaliableIndex[UnityEngine.Random.Range(0, _avaliableIndex.Count)];
            SetElement<ToolElement>(temp);
            _avaliableIndex.Remove(temp);
        }
    }
    private void SpawnGold(List<int> _avaliableIndex)
    {
        for (int i = 0; i < 10 && _avaliableIndex.Count > 0; i++)
        {
            int temp = _avaliableIndex[UnityEngine.Random.Range(0, _avaliableIndex.Count)];
            SetElement<GoldElement>(temp);
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



    public T SetElement<T>(int index) where T : BaseElement
    {
        int x, y;
        GetPosByIndex(index, out x, out y);
        return SetElement<T>(x, y);
    }

    public T SetElement<T>(int x, int y) where T : BaseElement
    {
        var temp = MapArray[x, y].gameObject.AddComponent<T>();
        temp.OnInit();
        Destroy(MapArray[x, y]);
        MapArray[x, y] = temp;
        return temp;
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
        ForNearElement(x, y,
            (i, j) =>
            {
                if (MapArray[i, j].ElementContent == ElementContent.Trap)
                {
                    count++;
                }
            });
        return count;
    }

    public void FloodFillElement(int x, int y, bool[,] visited = null)
    {
        if (visited == null)
        {
            visited = new bool[w, h];
        }

        if ((x >= 0 && y >= 0) && (x < w && y < h) && !visited[x, y])
        {
            visited[x, y] = true;
            if (MapArray[x, y].ElementType == ElementType.SingleCovered
                && MapArray[x, y].ElementContent == ElementContent.Number)
            {

                ((SingleCoverElement)MapArray[x, y]).UncoveredElement();

                if (CountAdjcentTraps(x, y) > 0) return;

                ForNearElement(x, y,
                (i, j) => FloodFillElement(i, j, visited));
            }
        }
    }

    public void UncoveredAdjacentElements(int x, int y)
    {
        int marked = 0;
        ForNearElement(x, y,
            (i, j) =>
            {
                if (MapArray[i, j].ElementState == ElementState.Marked
                    || (MapArray[i, j].ElementState == ElementState.UnCovered
                    && MapArray[i, j].ElementContent == ElementContent.Trap))
                {
                    marked++;
                }
            });



        if (CountAdjcentTraps(x, y) == marked)
        {
            ForNearElement(x, y,
            (i, j) =>
            {
                if (MapArray[i, j].ElementState != ElementState.Marked)
                {
                    MapArray[i, j].OnPlayerStand();
                }
            });
        }
    }

    public void DisplayAllTraps()
    {
        foreach (BaseElement element in MapArray)
        {
            if (element.ElementState == ElementState.Covered
                && element.ElementContent == ElementContent.Trap)
            {

            }
            else if (element.ElementState == ElementState.Marked
                && element.ElementContent == ElementContent.Trap)
            {
                Instantiate(errorElement, element.transform);
            }
        }
    }

    public void ForNearElement(int x, int y, Action<int, int> act)
    {
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if ((i != x || j != y) && (i >= 0 && j >= 0)
                    && (i < w && j < h))
                {
                    act(i, j);
                }
            }
        }
    }
}
