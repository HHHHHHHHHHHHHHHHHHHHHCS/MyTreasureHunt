using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class MainGameManager : MonoBehaviour
{
    /*
    private struct CloseAreaInfo
    {
        public int x, y, sx, ex, sy, ey;
        public int tx, ty, gx, gy;
        public int doorType;
        public Vector2Int doorPos;
        public Vector2Int toolPos, goldPos;
        public ToolElement t;
        public GoldElement g;
        public int innerCount, goldNumber;
    }
    */

    public static MainGameManager Instance { get; private set; }


    [Header("角色游戏物体"), SerializeField]
    private GameObject player;
    private Transform playerTarget;
    private int startMoveX, startMoveY;
    private Camera mainCamera;
    public Animator Anim { get; private set; }
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
    public int W { get { return w; } }
    [SerializeField]
    private int h;
    public int H { get { return h; } }
    [SerializeField]
    private float minTrapProbability;
    [SerializeField]
    private float maxTrapProbability;
    [SerializeField]
    private float uncoverProbability;
    public float UncoverProbability { get { return uncoverProbability; } }
    //[SerializeField]
    //private int standAreaW;
    //public float StandAreaW { get { return standAreaW; } }
    //[SerializeField]
    //private int obstacleAreaW;
    //public int ObstacleAreaW { get { return obstacleAreaW; } }
    //public float ObstacleAreaNum { get; private set; }
    [SerializeField]
    private int standAreaW;
    [SerializeField]
    private int obstacleAreaW;
    [SerializeField]
    private float obstacleProbability;
    [SerializeField]
    private int rewardMaxNumber;
    private int obstacleAreaNum;

    public BaseElement[,] MapArray { get; private set; }
    private Tweener pathTweener;
    private Vector2Int prePos, nowPos;
    private Vector2 dir;

    private void Awake()
    {
        Instance = this;
        prePos = Vector2Int.one * 100000;
        Anim = player.GetComponent<Animator>();
        CreateMap();
        InitMap();
        ResetCamera();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetCamera();
        }
        nowPos = player.transform.position.ToVec2Int();
        if (prePos != nowPos)
        {
            dir = new Vector2Int(Mathf.Clamp( nowPos.x-prePos.x,-1,1)
                , Mathf.Clamp(nowPos.y - prePos.y, -1, 1));
            Anim.SetFloat("DirX", dir.x);
            Anim.SetFloat("DirY", dir.y);
            MapArray[nowPos.x, nowPos.y].OnPlayerStand();
            MapArray[nowPos.x, nowPos.y].OnPlayerStand();
            if (MapArray[nowPos.x, nowPos.y].ElementContent == ElementContent.Trap)
            {
                pathTweener.Kill();
                nowPos = prePos;
                player.transform.position = new Vector3(nowPos.x, nowPos.y, player.transform.position.z);
            }
            else
            {
                prePos = nowPos;
            }
        }
    }

    #region InitMap
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

        int standAreaY = GenerateStandArea();
        SpawnExit(avaliableIndex);
        SpawnObstacleArea(avaliableIndex);
        SpawnTrap(standAreaY, avaliableIndex);
        SpawnTool(avaliableIndex);
        SpawnGold(avaliableIndex);
        SpawnNumber(avaliableIndex);
        SpawnStandArea(standAreaY, avaliableIndex);
    }

    private int GenerateStandArea()
    {
        return Random.Range(1, h - 1);
    }

    private int SpawnStandArea(int standAreaY, List<int> _avaliableIndex)
    {
        for (int x = 0; x < standAreaW; x++)
        {
            for (int y = standAreaY - 1; y <= standAreaY + 1; y++)
            {
                (MapArray[x, y] as SingleCoverElement).UncoveredElementSingle();
            }
        }
        player.transform.position = new Vector3(1, standAreaY, 0);
        return standAreaY;
    }

    private int SpawnStandArea(List<int> _avaliableIndex)
    {
        int standAreaY = Random.Range(1, h - 1);
        for (int x = 0; x < standAreaW; x++)
        {
            for (int y = standAreaY - 1; y <= standAreaY + 1; y++)
            {
                (MapArray[x, y] as SingleCoverElement).UncoveredElement();
            }
        }
        return standAreaY;
    }

    private void SpawnExit(List<int> _avaliableIndex)
    {
        if (_avaliableIndex.Count >= 4)
        {
            float x = w - 1.5f;
            float y = Random.Range(1, h) - 0.5f;

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
        obstacleAreaNum = (w - (standAreaW + standAreaW)) / obstacleAreaW;
        for (int i = 0; i < obstacleAreaNum; i++)
        {
            if (Random.value < obstacleProbability)
            {
                ElementContent elementContent;
                int x = CreateObstacle(i, _avaliableIndex, out elementContent);
                CreateTool(i, _avaliableIndex, elementContent);
                CreaterReward(x, i, _avaliableIndex);
            }
        }
    }

    private int CreateObstacle(int area, List<int> _avaliableIndex, out ElementContent obstacleType)
    {
        int sx = standAreaW + area * obstacleAreaW;
        int ex = sx + obstacleAreaW - 1;
        int x = Random.Range(sx, ex);
        int y = Random.Range(0, h);
        obstacleType = (ElementContent)Random.Range(4, 8);
        switch (obstacleType)
        {
            case ElementContent.Enemy:
                SetElement<EnemyElement>(x, y);
                break;
            case ElementContent.Door:
                SetElement<DoorElement>(x, y);
                break;
            case ElementContent.BigWall:
                SetElement<BigWallElement>(x, y);
                break;
            case ElementContent.SmallWall:
                SetElement<SmallWallElement>(x, y);
                break;
        }
        _avaliableIndex.Remove(GetIndexByPos(x, y));
        for (int j = 0; j < h; j++)
        {
            var index = GetIndexByPos(x, j);
            if (_avaliableIndex.Contains(index))
            {
                SetElement<BigWallElement>(x, j);
                _avaliableIndex.Remove(index);
            }
        }
        return x;
    }

    private void CreateTool(int _area, List<int> _avaliableIndex, ElementContent _elementContent)
    {
        int sx, ex;
        _area -= 1;
        if (_area < 0)
        {
            sx = 0;
            ex = standAreaW - 1;
        }
        else
        {
            sx = standAreaW + _area * obstacleAreaW;
            ex = sx + obstacleAreaW - 1;
        }

        int index = -1;
        for (int i = 0; i < 10; i++)
        {
            int ranX = Random.Range(sx, ex + 1);
            int ranY = Random.Range(0, h);
            int tempIndex = GetIndexByPos(ranX, ranY);
            if (_avaliableIndex.Contains(tempIndex))
            {
                index = tempIndex;
                break;
            }
        }

        if (index < 0)
        {
            List<int> randomList = new List<int>();
            for (int y = 0; y < h; y++)
            {
                for (int x = sx; x < ex; x++)
                {
                    int tempIndex = GetIndexByPos(x, y);
                    if (_avaliableIndex.Contains(tempIndex))
                    {
                        randomList.Add(tempIndex);
                    }
                }
            }
            if (randomList.Count <= 0)
            {
                CreateTool(_area, _avaliableIndex, _elementContent);
                return;
            }
            else
            {
                index = randomList[Random.Range(0, randomList.Count)];
            }
        }

        if (index >= 0)
        {
            _avaliableIndex.Remove(index);
            ToolType tt = (ToolType)_elementContent;
            ToolElement tool = SetElement<ToolElement>(index);
            tool.ReOnInit(tt);
        }

    }

    private void CreaterReward(int x, int _area, List<int> _avaliableIndex)
    {
        int sx = x;
        int ex = standAreaW + _area * obstacleAreaW + obstacleAreaW - 1;
        int rewardNumber = Random.Range(0, rewardMaxNumber);
        for (int i = 0; i < rewardNumber; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                int ranX = Random.Range(sx, ex + 1);
                int ranY = Random.Range(0, h);
                int index = GetIndexByPos(ranX, ranY);
                if (_avaliableIndex.Contains(index))
                {
                    _avaliableIndex.Remove(index);
                    SetElement<GoldElement>(index);
                    break;
                }
            }
        }
    }

    /*
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
            closeAreaInfo.x = Random.Range(3, obstacleAreaW - 4);
            closeAreaInfo.y = Random.Range(3, h - 3);
            closeAreaInfo.sx = standAreaW + _nowArea * obstacleAreaW + 1;
            closeAreaInfo.ex = closeAreaInfo.sx + closeAreaInfo.x;
            closeAreaInfo.doorType = Random.Range(4, 8);
            break;
        case 1:
            closeAreaInfo.x = Random.Range(3, obstacleAreaW - 4);
            closeAreaInfo.y = Random.Range(3, closeAreaInfo.x + 1);
            closeAreaInfo.sx = standAreaW + _nowArea * obstacleAreaW + 1;
            closeAreaInfo.ex = closeAreaInfo.sx + closeAreaInfo.x;
            closeAreaInfo.sy = Random.Range(3, h - closeAreaInfo.y - 1);
            closeAreaInfo.ey = closeAreaInfo.sy + closeAreaInfo.y;
            closeAreaInfo.doorType = (int)ElementContent.BigWall;
            break;
    }
    return closeAreaInfo;
}

private void SpawnObstacleArea(List<int> _avaliableIndex)
{
    for (int i = 0; i < ObstacleAreaNum; i++)
    {
        if (Random.value < 0.5f)
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
    int shape = Random.Range(0, 2);
    CloseAreaInfo info;
    switch (shape)
    {
        case 0:
            info = GenerateCloseAreaInfo(0, nowArea);
            int dir = Random.Range(0, 4);
            switch (dir)
            {
                case 0:
                    info.doorPos = Random.value < 0.5f
                        ? new Vector2Int(Random.Range(info.sx + 1, info.ex), info.y)
                        : new Vector2Int(Random.value < 0.5f ? info.sx : info.ex, Random.Range(info.y, h));
                    CreateULShapeAreaDoor(info, _avaliableIndex);
                    for (int i = h - 1; i > info.y; i--)
                    {
                        int index1 = GetIndexByPos(info.sx, i);
                        if (_avaliableIndex.Contains(index1))
                        {
                            _avaliableIndex.Remove(index1);
                            SetElement<BigWallElement>(index1);
                        }

                        int index2 = GetIndexByPos(info.ex, i);
                        if (_avaliableIndex.Contains(index2))
                        {
                            _avaliableIndex.Remove(index2);
                            SetElement<BigWallElement>(index2);
                        }
                    }

                    for (int i = info.sx; i <= info.ex; i++)
                    {
                        int index = GetIndexByPos(i, info.y);
                        if (_avaliableIndex.Contains(index))
                        {
                            _avaliableIndex.Remove(index);
                            SetElement<BigWallElement>(index);
                        }
                    }

                    info.sy = info.y;
                    info.ey = h - 1;
                    info.y = h - 1 - info.y;
                    CreateCloseAreaRewards(info, _avaliableIndex);
                    break;
                case 1:
                    info.doorPos = Random.value < 0.5f
                        ? new Vector2Int(Random.Range(info.sx + 1, info.ex), info.y)
                        : new Vector2Int( info.sx , Random.Range(0, info.y));
                    CreateULShapeAreaDoor(info, _avaliableIndex);
                    for (int i = 0; i < info.y; i++)
                    {
                        int index1 = GetIndexByPos(info.sx, i);
                        if (_avaliableIndex.Contains(index1))
                        {
                            _avaliableIndex.Remove(index1);
                            SetElement<BigWallElement>(index1);
                        }

                        int index2 = GetIndexByPos(info.ex, i);
                        if (_avaliableIndex.Contains(index2))
                        {
                            _avaliableIndex.Remove(index2);
                            SetElement<BigWallElement>(index2);
                        }
                    }

                    for (int i = info.sx; i <= info.ex; i++)
                    {
                        int index = GetIndexByPos(i, info.y);
                        if (_avaliableIndex.Contains(index))
                        {
                            _avaliableIndex.Remove(index);
                            SetElement<BigWallElement>(index);
                        }
                    }

                    info.sy = 0;
                    info.ey = info.y;
                    CreateCloseAreaRewards(info, _avaliableIndex);
                    break;
                case 2:
                    info.doorPos = Random.value < 0.5f
                        ? new Vector2Int(Random.Range(info.sx + 1, info.ex), info.y)
                        : new Vector2Int(Random.value < 0.5f ? info.sx : info.ex - 1, Random.Range(info.y, h));
                    CreateULShapeAreaDoor(info, _avaliableIndex);
                    for (int i = h - 1; i > info.y; i--)
                    {
                        int index1 = GetIndexByPos(info.sx, i);
                        if (_avaliableIndex.Contains(index1))
                        {
                            _avaliableIndex.Remove(index1);
                            SetElement<BigWallElement>(index1);
                        }
                    }

                    for (int i = 0; i < info.y; i++)
                    {
                        int index2 = GetIndexByPos(info.ex, i);
                        if (_avaliableIndex.Contains(index2))
                        {
                            _avaliableIndex.Remove(index2);
                            SetElement<BigWallElement>(index2);
                        }
                    }

                    for (int i = info.sx; i <= info.ex; i++)
                    {
                        int index = GetIndexByPos(i, info.y);
                        if (_avaliableIndex.Contains(index))
                        {
                            _avaliableIndex.Remove(index);
                            SetElement<BigWallElement>(index);
                        }
                    }

                    info.sy = 0;
                    info.ey = info.y;
                    CreateCloseAreaRewards(info, _avaliableIndex);
                    break;
                case 3:
                    info.doorPos = Random.value < 0.5f
                        ? new Vector2Int(Random.Range(info.sx + 1, info.ex), info.y)
                        : new Vector2Int(info.sx , Random.Range(0, info.y));
                    CreateULShapeAreaDoor(info, _avaliableIndex);
                    for (int i = 0; i < info.y; i++)
                    {
                        int index1 = GetIndexByPos(info.sx, i);
                        if (_avaliableIndex.Contains(index1))
                        {
                            _avaliableIndex.Remove(index1);
                            SetElement<BigWallElement>(index1);
                        }
                    }

                    for (int i = h - 1; i >= info.y; i--)
                    {
                        int index2 = GetIndexByPos(info.ex, i);
                        if (_avaliableIndex.Contains(index2))
                        {
                            _avaliableIndex.Remove(index2);
                            SetElement<BigWallElement>(index2);
                        }
                    }

                    for (int i = info.sx; i <= info.ex; i++)
                    {
                        int index = GetIndexByPos(i, info.y);
                        if (_avaliableIndex.Contains(index))
                        {
                            _avaliableIndex.Remove(index);
                            SetElement<BigWallElement>(index);
                        }
                    }

                    info.sy = 0;
                    info.ey = info.y;
                    CreateCloseAreaRewards(info, _avaliableIndex);
                    break;
            }
            CreateCloseTool(info, _avaliableIndex);
            break;
        case 1:
            info = GenerateCloseAreaInfo(1, nowArea);
            for (int i = info.sx; i <= info.ex; i++)
            {
                int index1 = GetIndexByPos(i, info.sy);
                if (_avaliableIndex.Contains(index1))
                {
                    _avaliableIndex.Remove(index1);
                    SetElement<BigWallElement>(index1);
                }

                int index2 = GetIndexByPos(i, info.ey);
                if (_avaliableIndex.Contains(index2))
                {
                    _avaliableIndex.Remove(index2);
                    SetElement<BigWallElement>(index2);
                }
            }

            for (int i = info.sy + 1; i < info.ey; i++)
            {
                int index1 = GetIndexByPos(info.sx, i);
                if (_avaliableIndex.Contains(index1))
                {
                    _avaliableIndex.Remove(index1);
                    SetElement<BigWallElement>(index1);
                }

                int index2 = GetIndexByPos(info.ex, i);
                if (_avaliableIndex.Contains(index2))
                {
                    _avaliableIndex.Remove(index2);
                    SetElement<BigWallElement>(index2);
                }
            }

            CreateCloseTool(info, _avaliableIndex);
            CreateCloseAreaRewards(info, _avaliableIndex);
            break;
    }

}

private void CreateRandomWall(int nowArea, List<int> _avaliableIndex)
{
    int sx = standAreaW + nowArea * obstacleAreaW + 1;
    int ex = sx + obstacleAreaW;
    int wx = Random.Range(sx, ex);
    int wy = Random.Range(0, h);

    List<int> tempList = new List<int>();
    for (int tempX = sx; tempX < ex; tempX++)
    {
        for (int tempY = 0; tempY < h; tempY++)
        {
            var index = GetIndexByPos(tempX, tempY);
            if (_avaliableIndex.Contains(index))
            {
                tempList.Add(index);
            }
        }
    }
    for (int i = 0; i < 5 && tempList.Count > 0; i++)
    {
        int index = tempList[Random.Range(0, tempList.Count)];
        tempList.Remove(index);
        if (Random.value < 0.5f)
        {
            SetElement<SmallWallElement>(index);
        }
        else
        {
            SetElement<BigWallElement>(index);
        }
    }
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
    _info.goldNumber = Random.Range(1, Random.value < 0.5f ? _info.innerCount + 1 : _info.innerCount / 2);
    for (int i = 0; i < _info.goldNumber; i++)
    {
        _info.gy = i / _info.x;
        _info.gx = i - _info.gy * _info.x;
        _info.gx += _info.sx + 1;
        _info.gy += _info.sy + 1;
        int index = GetIndexByPos(_info.gx, _info.gy);
        if (_avaliableIndex.Contains(index))
        {
            _avaliableIndex.Remove(index);
            _info.g = SetElement<GoldElement>(index);
        }
    }
}

private void CreateCloseTool(CloseAreaInfo _info, List<int> _avaliableIndex)
{
    _info.tx = Random.Range(0, _info.sx);
    _info.ty = Random.Range(0, h);
    int index = _avaliableIndex[Random.Range(0, _avaliableIndex.Count)];
    _info.t = SetElement<ToolElement>(index);
}
*/
    private void SpawnTrap(int standAreaY, List<int> _avaliableIndex)
    {
        float trapPro = Random.Range(minTrapProbability, maxTrapProbability);
        int trapNum = (int)(_avaliableIndex.Count * trapPro);

        for (int i = 0; i < trapNum && _avaliableIndex.Count > 0; i++)
        {
            int x, y;
            int temp = _avaliableIndex[Random.Range(0, _avaliableIndex.Count)];
            GetPosByIndex(temp, out x, out y);
            if ((x >= 0 && x < standAreaW) && (y >= standAreaY - 1 && y <= standAreaY + 1))
            {
                i--;
                continue;
            }
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
        for (int i = 0; i < Random.Range(0, 3) && _avaliableIndex.Count > 0; i++)
        {
            int temp = _avaliableIndex[Random.Range(0, _avaliableIndex.Count)];
            SetElement<ToolElement>(temp);
            _avaliableIndex.Remove(temp);
        }
    }

    private void SpawnGold(List<int> _avaliableIndex)
    {
        for (int i = 0; i < Random.Range(0, obstacleAreaNum * 2) && _avaliableIndex.Count > 0; i++)
        {
            int temp = _avaliableIndex[Random.Range(0, _avaliableIndex.Count)];
            SetElement<GoldElement>(temp);
            _avaliableIndex.Remove(temp);
        }
    }

    private void ResetCamera()
    {
        //Camera camera = Camera.main;
        //Camera.main.orthographicSize = (h + 3) / 2f;
        //camera.transform.position = new Vector3((w - 1) / 2f, (h - 1) / 2f, -10);
        var vCamera = GameObject.Find("VCamera").GetComponent<CinemachineVirtualCamera>();
        vCamera.m_Lens.OrthographicSize = (h + 3) / 2f;
        var cft = vCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFramingTransposer;
        cft.m_DeadZoneHeight = (h * 100f) / (300f + h * 100f);
        cft.m_DeadZoneWidth = (w * 100f) / (300f + w * 100f) / h * (16f / 9f);
        transform.Find("CameraCollider").GetComponent<PolygonCollider2D>().SetPath(0, new Vector2[]
        {
            new Vector2(-2,-2),
            new Vector2(-2,h+1),
            new Vector2(w+1,h+1),
            new Vector2(w+1,-2)
        });


    }

    #endregion

    #region CommandTool

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

    public Sprite GetNumberSpriteByPos(int x, int y)
    {
        return NumberSprites[CountAdjcentTraps(x, y)];
    }

    #endregion

    #region Method
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
            Anim.SetTrigger("QuickCheck");
            ForNearElement(x, y,
            (i, j) =>
            {
                if (MapArray[i, j].ElementState != ElementState.Marked)
                {
                    MapArray[i, j].OnPlayerStand();
                }
            });
        }
        else
        {
            Anim.SetTrigger("Why");
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

    public void FindPath(AStarPoint e)
    {
        if (pathTweener != null)
        {
            pathTweener.Kill();
        }
        AStarPoint s = new AStarPoint((int)player.transform.position.x, (int)player.transform.position.y);
        List<AStarPoint> pathList;
        if (!AStarPathFinding.FindPath(s, e, out pathList))
        {
            Anim.SetTrigger("Why");
            return;
        }
        else
        {
            pathTweener = player.transform.DOPath(pathList.ToVector3Array(), pathList.Count * 0.15f);
            pathTweener.SetEase(Ease.Linear);
            pathTweener.OnComplete(() => pathTweener = null).OnKill(() => pathTweener = null);
        }
    }

    #endregion

    private void OnMouseOver()
    {
        if (!playerTarget)
        {
            playerTarget = player.transform.Find("VCameraTarget");
            mainCamera = Camera.main;
            startMoveX = (int)(Screen.width * 0.8f);
            startMoveY = Screen.height / 2;

        }

        if (Input.GetMouseButton(0))
        {
            playerTarget.transform.position += new Vector3(Input.GetAxis("Mouse X"), 0, 0);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            playerTarget.position = mainCamera.ScreenToWorldPoint(new Vector3(startMoveX, startMoveY, 0)) + new Vector3(0, 0, 10);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ResetPlayerTarget();
        }
    }

    private void ResetPlayerTarget()
    {
        playerTarget.localPosition = Vector3.zero;
    }
}
