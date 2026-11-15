namespace Tachyogenesis.Types;

public class CoordinateRange(float x1, float x2, float y1, float y2)
{
    public CoordinateRange() : this(float.MinValue, float.MaxValue, float.MinValue, float.MaxValue) { }
    
    public float x1 = x1;
    public float x2 = x2;
    public float y1 = y1;
    public float y2 = y2;
}