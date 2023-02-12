using System;
using Godot;
using GodotUtilities;

namespace SampleGodotCSharpProject.Game.Extension;

public static class NodeExtension
{
    private static ResourcePreloader _preloader;
    
    public static T InstantiateFromResources<T>(this Node node) where T : Node
    {
        _preloader ??= node.GetNode<ResourcePreloader>("/root/RsPreloader");
        return _preloader.InstanceSceneOrNull<T>();
    }

    public static Color IntensifyColor(this Node node, Color color, float factor)
    {
        var result = new Color(color);
        result *= factor;
        result.A = 1.0f;
        return result;
    }

    public static async void InstantiateChildDeferredWithAction<T>(this Node node, Action<T> action) where T : Node
    {
        T child = node.InstantiateFromResources<T>();
        node.CallDeferred("add_child", child);
        await child.ToSignal(node, Node.SignalName.TreeEntered);
        action(child);
    }

}
