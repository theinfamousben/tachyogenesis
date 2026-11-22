using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Tachyogenesis.Scripts;

public class ErrorHelper
{
    public readonly List<Error> errors = 
    [
        new("Camera Clamps Not Set", Errors.CameraClampsNotSet),
        new("Outline Material Not Set", Errors.OutlineMaterialNotSet)
    ];
    public void ThrowOnce(Error e, string caller = "[none]")
    {
        if (e.thrown) return;
        Throw(e, caller);
        GetError(e.errorEnum).thrown = true;
    }

    public void ThrowOnce(Errors errorEnum, string caller = "[none]") => ThrowOnce(GetError(errorEnum), caller);

    public void Throw(Error e, string caller = "[none]")
    {
        GD.PrintErr($"{caller}: {e.msg}");
    }

    public void Throw(Errors errorEnum, string caller = "[none]") => Throw(GetError(errorEnum), caller);

    public Error GetError(Errors e) => errors.Find(error => error.errorEnum == e);
}

public class Error
{
    public Error(string msg, Errors errorEnum, bool thrown = false)
    {
        this.msg = msg;
        this.errorEnum = errorEnum;
        this.thrown = thrown;
    }

    public readonly Errors errorEnum;
    public readonly string msg;
    public bool thrown = false;
}
public enum Errors
{
    CameraClampsNotSet,
    OutlineMaterialNotSet
}