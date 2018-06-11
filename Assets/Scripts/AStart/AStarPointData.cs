public struct AStarPoint
{
    public int x, y;

    public AStarPoint(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public bool Equals(AStarPoint obj)
    {
        return x == obj.x && y == obj.y;
    }
}

public class AStarPointData
{
    public AStarPoint pointPos;
    public double g, h;
    public AStarPointData parent;

    public double F { get { return g + h; } }


    public AStarPointData(AStarPoint _AStartPoint, double _g, double _h, AStarPointData _parent)
    {
        pointPos = _AStartPoint;
        g = _g;
        h = _h;
        parent = _parent;
    }


}

