using System.Collections.Generic;
using Tachyogenesis.Types;

namespace Tachyogenesis.Data;

public static class Constants
{
    public static readonly Dictionary<string, CoordinateRange> MAX_CAMERA_CLAMPS = new()
    {
        { "facility_0", new CoordinateRange{ x1 = 0, y1 = 16, x2 = 640, y2 = 1000 }},
        { "facility_1", new CoordinateRange{ x1 = 0, y1 = 0, x2 = float.MaxValue, y2 = float.MaxValue }}
    };
}