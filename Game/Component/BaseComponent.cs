using Godot;

namespace SampleGodotCSharpProject.Game.Component;

public partial class BaseComponent : Node2D
{
    [Export]
    public bool Enabled
    {
        set => SetEnabled(value);
        get => _enabled;
    }

    private bool _enabled = true;

    protected virtual void _EnabledPostProcess()
    {
    }

    public void SetEnabled(bool enabled)
    {
        _enabled = enabled;
        _EnabledPostProcess();
    }
}
