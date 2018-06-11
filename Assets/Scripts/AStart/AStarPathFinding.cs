using System;
using System.Collections.Generic;

public class AStarPathFinding
{
    /// <summary>
    /// 起点
    /// </summary>
    private const char Start = 'S';
    /// <summary>
    /// 终点
    /// </summary>
    private const char End = 'E';
    /// <summary>
    /// 空地
    /// </summary>
    private const char Space = '.';
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


    public static double CalH(AStarPoint pnt)
    {
        return HPowEuclidianDistance(pnt);
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

    private void Search()
    {
        //用List集合做"开启列表"  来记录扩展的点
        List<AStarPointData> openList = new List<AStarPointData>();
        int[,] directs = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
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
            if (Map[point.x, point.y] == Space)
            {
                Map[point.x, point.y] = Visited;
            }
            for (int i = 0; i < directs.Rank; i++)
            {
                AStarPoint newPoint = new AStarPoint(point.x + directs[i, 0], point.y + directs[i, 1]);
                if (newPoint.x >= 0 && newPoint.x < Max_PNT.x && newPoint.y >= 0 && newPoint.y <= Max_PNT.y)
                {
                    char e = Map[newPoint.x, newPoint.y];
                    if (e == End)
                    {
                        endData = data;
                        isFinish = true;
                        break;
                    }
                    if (e != Space)
                    {
                        continue;
                    }
                    //查找判断点是否在"开启列表"中
                    AStarPointData tempData = openList.Find(x => x.pointPos.Equals(newPoint));
                    if(tempData!=null)
                    {
                        double goffest= Math.Abs(directs[i, 0]) != Math.Abs(directs[i, 1])
                            ?StraightLine:SlantLine;
                        double tempG = data.g + goffest;
                        if (tempData.g>data.g+goffest)
                        {
                            tempData.g = tempG;
                            tempData.parent = data;
                        }
                    }
                    else
                    {
                        double goffest = Math.Abs(directs[i, 0]) != Math.Abs(directs[i, 1])
                            ? StraightLine : SlantLine;
                        double h = CalH(newPoint);
                        AStarPointData newData = new AStarPointData(newPoint, goffest, h, data);
                        openList.Add(newData);
                    }
                }
            }
        }

        //反向查找 找出路径
        AStarPointData pointData = endData;
        while(pointData!=null)
        {
            AStarPoint point = pointData.pointPos;
            if(Map[point.x,point.y]==Visited)
            {
                Map[point.x, point.y] = OnPath;
            }
            pointData = pointData.parent;
        }
    }
}
