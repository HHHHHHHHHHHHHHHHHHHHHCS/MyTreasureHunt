public struct AStartPoint
{
    public int x, y;

    public AStartPoint(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public bool Equals(AStartPoint obj)
    {
        return x == obj.x && y == obj.y;
    }
}

public class AStartPointData
{
    public AStartPoint pointPos;
    public double g, h;
    public AStartPointData parent;

    public double F { get { return g + h; } }


    public AStartPointData(AStartPoint _AStartPoint, double _g, double _h, AStartPointData _parent)
    {
        pointPos = _AStartPoint;
        g = _g;
        h = _h;
        parent = _parent;
    }


}

