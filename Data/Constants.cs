using System.Collections.Generic;
using Tachyogenesis.Types;

namespace Tachyogenesis.Data;

public static class Constants
{
    // ReSharper disable once InconsistentNaming
    public static readonly Dictionary<string, CoordinateRange> MAX_CAMERA_CLAMPS = new()
    {
        { "facility_0", new CoordinateRange( 0, 640,16, 1000 )},
        { "facility_1", new CoordinateRange( 0, float.MaxValue, 16, float.MaxValue )}
    };

    public const bool IS_PLAYSTATION = false;
}