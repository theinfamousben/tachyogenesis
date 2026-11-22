using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Tachyogenesis.Data;

namespace Tachyogenesis.Types;

public static class KeyHint
{
    public static int GetKey(string key, string input)
    {
        if (input == "pc") return (int)PCKeyHint[key];
        if (input == "xbox") return (int)XboxKeyHint[key];
        if (input == "ps") return (int)PSKeyHint[key];
        return 0;
    }

    public static int GetKey(string key, @Input input) => GetKey(key, InputToString[input]);
    
    public static Dictionary<string, PC> PCKeyHint = new()
    {
        { "interact", PC.KeyE },
        { "exit", PC.KeyQ }
    };

    public static Dictionary<string, PS> PSKeyHint = new()
    {
        { "interact", PS.KeyTriangle },
    };

    public static Dictionary<string, Xbox> XboxKeyHint = new()
    {
        { "interact", Xbox.KeyY },
    };

    public static Dictionary<@Input, string> InputToString = new()
    {
        { @Input.PC, "pc" },
        { @Input.PS, "ps" },
        { @Input.Xbox, "xbox" }
    };
    
    public enum PC
    {
        Empty = 0,
        KeyA = 1,
        KeyB = 2,
        KeyC = 3,
        KeyD = 4,
        KeyE = 5,
        KeyF = 6,
        KeyG = 7,
        KeyH = 8,
        KeyI = 9,
        KeyJ = 10,
        KeyK = 11,
        KeyL = 12,
        KeyM = 13,
        KeyN = 14,
        KeyO = 15,
        KeyP = 16,
        KeyQ = 17,
        KeyR = 18,
        KeyS = 19,
        KeyT = 20,
        KeyU = 21,
        KeyV = 22,
        KeyW = 23,
        KeyX = 24,
        KeyY = 25,
        KeyZ = 26,
        
        KeyEnter = 32,
        KeyShift = 33,
        KeyCapsLock = 34,
    }

    public enum PS
    {
        Empty = 0,
        KeyCross = 1,
        KeyCircle = 2,
        KeySquare = 3,
        KeyTriangle = 4,
    }

    public enum Xbox
    {
        Empty = 0,
        KeyA = 1,
        KeyB = 2,
        KeyX = 3,
        KeyY = 4,
    }

    public enum @Input
    {
        PC,
        PS,
        Xbox
    }
}