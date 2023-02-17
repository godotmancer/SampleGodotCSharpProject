using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Extension;

namespace SampleGodotCSharpProject.Game.Component
{
    public partial class ScoreAttractorComponent : BaseComponent
    {
        public override void _Ready()
        {
            var node = (Node2D)GetParent();
            var parent = node.GetParent();

            node.GetFirstNodeOfType<QueueFreeComponent>()?.SetEnabled(false);

            parent.RemoveChild(node);
            var newParent = Global.Instance.Camera2D;
            newParent.AddChild(node);
            node.Owner = newParent;
            node.GlobalPosition = newParent.ToLocal(node.GlobalPosition);

            var tween = CreateTween();
            tween.TweenProperty(
                    node,
                    "position",
                    Vector2.Zero,
                    1.5f)
                .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.In);
            tween.Parallel()
                .TweenProperty(
                    node,
                    "modulate",
                    Colors.Transparent,
                    2.0f)
                .SetTrans(Tween.TransitionType.Expo)
                .SetEase(Tween.EaseType.In);
            tween.TweenCallback(Callable.From(QueueFree));

            node.GetFirstNodeOfType<QueueFreeComponent>()?.SetEnabled(true);
        }
    }
}
