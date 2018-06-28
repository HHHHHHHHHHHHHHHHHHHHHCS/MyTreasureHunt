using System;
using System.Collections.Generic;

public class AStarPathFinding
{
    #region 固定属性
    /// <summary>
    /// 起点
    /// </summary>
    private const char Start = 'S';
    /// <summary>
    /// 终点
    /// </summary>
    private const char End = 'E';
    /// <summary>
    /// 空地0
    /// </summary>
    private const char Space0 = '.';
    /// <summary>
    /// 空地1
    /// </summary>
    private const char Space1 = '+';
    /// <summary>
    /// 墙
    /// </summary>
    private const char Wall = 'W';
    /// <summary>
    /// 被访问过
    /// </summary>
    private const char Visited = '-';
    /// <summary>
    /// 在结果路径上
    /// </summary>
    private const char OnPath = '@';
    /// <summary>
    /// 直线距离
    /// </summary>
    private const double StraightLine = 1.0;
    /// <summary>
    /// 斜线距离
    /// </summary>
    private const double SlantLine = 1.4;
    /// <summary>
    /// 八个方向的苏组
    /// </summary>
    private static readonly int[,] directs = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

    #endregion
    #region 传入属性
    /// <summary>
    /// 地图字符串
    /// </summary>
    public static char[,] Map = null;

    /// <summary>
    /// 地图最大尺寸
    /// </summary>
    public static AStarPoint Max_PNT;

    /// <summary>
    /// 起点
    /// </summary>
    public static AStarPoint Start_Pnt;

    /// <summary>
    /// 终点
    /// </summary>
    public static AStarPoint End_Pnt;
    #endregion
    #region H相关
    /// <summary>
    /// 计算H的抽象方法
    /// </summary>
    /// <param name="pnt"></param>
    /// <returns></returns>
    public static double CalH(AStarPoint pnt)
    {
        return HPowEuclidianDistance(pnt) * Map[pnt.x, pnt.y] == Space0 ? 1 : 10000;
    }

    /// <summary>
    /// 获取曼哈顿距离
    /// </summary>
    /// <param name="pnt"></param>
    /// <returns></returns>
    private static double HManhattanDistance(AStarPoint pnt)
    {
        return Math.Abs(pnt.x - End_Pnt.x) + Math.Abs(pnt.y - End_Pnt.y);
    }

    /// <summary>
    /// 获取曼哈顿距离
    /// </summary>
    /// <param name="pnt"></param>
    /// <returns></returns>
    private static double HEuclidianDistance(AStarPoint pnt)
    {
        return Math.Sqrt(HPowEuclidianDistance(pnt));
    }

    /// <summary>
    /// 获取曼哈顿距离
    /// </summary>
    /// <param name="pnt"></param>
    /// <returns></returns>
    private static double HPowEuclidianDistance(AStarPoint pnt)
    {
        return Math.Pow(pnt.x - End_Pnt.x, 2) + Math.Pow(pnt.y - End_Pnt.y, 2);
    }
    #endregion

    private static bool GenerateMap(AStarPoint s, AStarPoint e)
    {
        if (s.Equals(e))
        {
            return false;
        }

        var main = MainGameManager.Instance;
        var mainMap = main.MapArray;
        Max_PNT = new AStarPoint(main.W, main.H);
        Start_Pnt = s;
        End_Pnt = e;
        Map = new char[Max_PNT.x, Max_PNT.y];

        for (int y = 0; y < Max_PNT.y; y++)
        {
            for (int x = 0; x < Max_PNT.x; x++)
            {
                var item = mainMap[x, y];
                if (item.ElementContent == ElementContent.Door || item.ElementContent == ElementContent.Enemy
                    || item.ElementContent == ElementContent.BigWall || item.ElementContent == ElementContent.SmallWall
                    || item.ElementState == ElementState.Marked
                    || (item.ElementContent == ElementContent.Trap && item.ElementState == ElementState.UnCovered))
                {
                    Map[x, y] = Wall;
                }
                else if (item.ElementState == ElementState.UnCovered
                    || (item.ElementContent == ElementContent.Tool && !(item as ToolElement).isHide )
                    || (item.ElementContent == ElementContent.Gold && !(item as GoldElement).isHide ))
                {
                    Map[x, y] = Space0;
                }
                else
                {
                    Map[x, y] = Space1;
                }
            }
        }
        if (Map[End_Pnt.x, End_Pnt.y] == Wall)
        {
            return false;
        }
        Map[Start_Pnt.x, Start_Pnt.y] = Start;
        Map[End_Pnt.x, End_Pnt.y] = End;
        return true;
    }

    private static bool Search(out List<AStarPoint> pathList)
    {
        //用List集合做"开启列表"  来记录扩展的点
        List<AStarPointData> openList = new List<AStarPointData>();
        //把起点放入开启列表
        openList.Add(new AStarPointData(Start_Pnt, 0, 0, null));

        //最后一个点的数据 用于反推
        AStarPointData endData = null;
        //是否完成
        bool isFinish = false;
        while (!isFinish && openList.Count > 0)
        {//找到终点或者"开启列表"为空的时候退出循环
            openList.Sort((x, y) => { return x.F.CompareTo(y.F); });
            AStarPointData data = openList[0];
            openList.RemoveAt(0);
            AStarPoint point = data.pointPos;
            //将取出的点表示为已访问点
            if (Map[point.x, point.y] == Space0 || Map[point.x, point.y] == Space1)
            {
                Map[point.x, point.y] = Visited;
            }
            for (int i = 0; i < directs.GetLength(0); i++)
            {
                AStarPoint newPoint = new AStarPoint(point.x + directs[i, 0], point.y + directs[i, 1]);
                if (newPoint.x >= 0 && newPoint.x < Max_PNT.x && newPoint.y >= 0 && newPoint.y < Max_PNT.y)
                {
                    char e = Map[newPoint.x, newPoint.y];
                    if (e == End)
                    {
                        endData = data;
                        isFinish = true;
                        break;
                    }
                    if (e != Space0 && e != Space1)
                    {
                        continue;
                    }
                    //查找判断点是否在"开启列表"中
                    AStarPointData tempData = openList.Find(x => x.pointPos.Equals(newPoint));
                    if (tempData != null)
                    {
                        double goffest = Math.Abs(directs[i, 0]) != Math.Abs(directs[i, 1])
                            ? StraightLine : SlantLine;
                        double tempG = data.g + goffest;
                        if (tempData.g > tempG)
                        {
                            tempData.g = tempG;
                            tempData.parent = data;
                        }
                    }
                    else
                    {
                        double goffest = data.g+Math.Abs(directs[i, 0]) != Math.Abs(directs[i, 1])
                            ? StraightLine : SlantLine;
                        double h = CalH(newPoint);
                        AStarPointData newData = new AStarPointData(newPoint, goffest, h, data);
                        openList.Add(newData);
                    }
                }
            }
        }

        //反向查找 找出路径
        pathList = new List<AStarPoint>();
        AStarPointData pointData = endData;
        pathList.Add(End_Pnt);
        while (pointData != null)
        {
            AStarPoint point = pointData.pointPos;
            if (Map[point.x, point.y] == Visited)
            {
                Map[point.x, point.y] = OnPath;
                pathList.Add(point);
            }
            pointData = pointData.parent;
        }
        pathList.Add(Start_Pnt);
        pathList.Reverse();

        if(pathList.Count<=2&&(Math.Abs(Start_Pnt.x-End_Pnt.x)>1
            || Math.Abs(Start_Pnt.y - End_Pnt.y) > 1))
        {
            return false;
        }
        return true;
    }

    public static bool FindPath(AStarPoint s,AStarPoint e, out List<AStarPoint> pathList)
    {
        pathList = null;
        return GenerateMap(s, e) && Search(out pathList);
    }
}
