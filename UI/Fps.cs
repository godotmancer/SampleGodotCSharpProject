using Godot;

namespace SampleGodotCSharpProject.UI
{
    public partial class Fps : Label
    {
        public override void _Process(double delta)
        {
            Text = $"{Engine.GetFramesPerSecond()}/s";
        }
    }
}
