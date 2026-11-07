using System.Collections.Generic;
using Tachyogenesis.Types;

namespace Tachyogenesis.Data;

public static class Constants
{
    public static readonly Dictionary<string, CoordinateRange> MAX_CAMERA_CLAMPS = new()
    {
        { "facility_0", new CoordinateRange{ x1 = 0, x2 = 10000, y1 = 16, y2 = 10000 }}
        
    };
}