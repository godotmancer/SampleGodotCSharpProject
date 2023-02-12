using Godot;

namespace SampleGodotCSharpProject.Game.Component;

public partial class BaseComponent : Node2D
{
    public bool Enabled { set; get; } = true;

    public void SetEnabled(bool enabled)
    {
        Enabled = enabled;
    }
}