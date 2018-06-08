using System;
using System.Collections.Generic;

public class AStartPathFinding
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
    /// 地图字符串
    /// </summary>
    public static char[,] Map = null;

    /// <summary>
    /// 地图最大尺寸
    /// </summary>
    public static AStartPoint Max_PNT;

    /// <summary>
    /// 起点
    /// </summary>
    public static AStartPoint Start_Pnt;

    /// <summary>
    /// 终点
    /// </summary>
    public static AStartPoint End_Pnt;


    public static double CalH(AStartPoint pnt)
    {
        return HPowEuclidianDistance(pnt);
    }


    /// <summary>
    /// 获取曼哈顿距离
    /// </summary>
    /// <param name="pnt"></param>
    /// <returns></returns>
    private static double HManhattanDistance(AStartPoint pnt)
    {
        return Math.Abs(pnt.x - End_Pnt.x) + Math.Abs(pnt.y - End_Pnt.y);
    }

    /// <summary>
    /// 获取曼哈顿距离
    /// </summary>
    /// <param name="pnt"></param>
    /// <returns></returns>
    private static double HEuclidianDistance(AStartPoint pnt)
    {
        return Math.Sqrt(HPowEuclidianDistance(pnt));
    }

    /// <summary>
    /// 获取曼哈顿距离
    /// </summary>
    /// <param name="pnt"></param>
    /// <returns></returns>
    private static double HPowEuclidianDistance(AStartPoint pnt)
    {
        return Math.Pow(pnt.x - End_Pnt.x, 2) + Math.Pow(pnt.y - End_Pnt.y, 2);
    }

    private void Search()
    {
        //用List集合做"开启列表"  来记录扩展的点
        List<AStartPointData> openList = new List<AStartPointData>();
        int[,] directs = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
        //把起点放入开启列表
        openList.Add(new AStartPointData(Start_Pnt, 0, 0, null));

        //最后一个点的数据 用于反推
        AStartPointData endPoint = null;
        //是否完成
        bool isFinish = false;
        while(!isFinish&&openList.Count>0)
        {//找到终点或者"开启列表"为空的时候退出循环
            openList.Sort((x, y) => { return x.F.CompareTo(y.F); });
            AStartPointData data = openList[0];
            openList.RemoveAt(0);
            AStartPoint point = data.pointPos;
            //将取出的点表示为已访问点
            if(Map[point.x,point.y]==Space)
            {
                Map[point.x, point.y] = Visited;
            }
            for(int i=0;i<directs.Rank;i++)
            {

            }
        }
    }
}
