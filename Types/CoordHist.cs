using System.Collections.Generic;
using Godot;

namespace Tachyogenesis.Types;

public class CoordHist(Vector2 origin)
{
    private CoordinatePair Origin { get; } = new( x: origin.X, y: origin.Y );
    private readonly List<CoordinatePair> _entries = [];

    public void Add(CoordinatePair entry)
    {
        _entries.Add(entry);
    }

    public CoordinatePair Pop()
    {
        if (_entries.Count < 1) return Origin;
        var entry = _entries[^1];
        _entries.RemoveAt(_entries.Count - 1);
        return entry;
    }

    public CoordinatePair ReadLast() => _entries.Count > 0 ? _entries[^1] : Origin;

    public CoordinatePair? GetLatestAtOrBefore(double time)
    {
        for (var i = _entries.Count - 1; i >= 0; i--)
        {
            if (_entries[i].time <= time) return _entries[i];
        }

        return null;
    }

    public void RemoveAfter(double time)
    {
        while (_entries.Count > 0 && _entries[^1].time > time)
            _entries.RemoveAt(_entries.Count - 1);
    }
}

public record struct CoordinatePair
{
    public CoordinatePair(float x, float y, float time)
    {
        this.x = x;
        this.y = y;
        this.time = time;
    }

    public CoordinatePair(float x, float y, double time)
    {
        this.x = x;
        this.y = y;
        this.time = (float)time;
    }

    public CoordinatePair(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public readonly float x;
    public readonly float y;
    public readonly float time;
}