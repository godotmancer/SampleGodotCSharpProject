using Godot;

namespace SampleGodotCSharpProject.Game.Component;

public partial class QueueFreeComponent : BaseComponent
{
    private Node _parent;
    private int _exitCount;

    public override void _Ready()
    {
        _parent = GetParent();
    }

    public void AddWaitForNodeExit(Node node)
    {
        if (Enabled)
        {
            _exitCount++;
            node.TreeExited += _NodeExitedTree;
        }
    }

    public void _NodeExitedTree()
    {
        if (Enabled)
        {
            _exitCount--;
            if (_exitCount <= 0)
            {
                QueueFree();
            }
        }
    }
}
