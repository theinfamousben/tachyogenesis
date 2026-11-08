using System.Threading.Tasks;
using Godot;

namespace Tachyogenesis.Scripts;

public abstract class Cutscene
{
    public abstract Task Play(Node host);
}
