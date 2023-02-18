using Godot;
using SampleGodotCSharpProject.Game.Component.Element;
using SampleGodotCSharpProject.Game.Entity.Enemy;

namespace SampleGodotCSharpProject.Game.Autoload
{
    public partial class GameEvents : Node
    {
        [Signal]
        public delegate void CollisionEventHandler(KinematicCollision2D collision2D);

        [Signal]
        public delegate void ElementIntensityDepletedEventHandler(ElementComponent element);

        [Signal]
        public delegate void ElementIntensityMaxedEventHandler(ElementComponent element);

        [Signal]
        public delegate void ZombieKilledEventHandler(Zombie zombie);

        [Signal]
        public delegate void ZombieSpawnedEventHandler(Zombie zombie);

        [Signal]
        public delegate void PlayerHitEventHandler();

        public static GameEvents Instance { get; private set; }

        public override void _Notification(int what)
        {
            if (what == NotificationEnterTree) Instance = this;
        }

        public static void EmitElementIntensityMaxed(ElementComponent element)
        {
            Instance.EmitSignal(SignalName.ElementIntensityMaxed, element);
        }

        public static void EmitElementIntensityDepleted(ElementComponent element)
        {
            Instance.EmitSignal(SignalName.ElementIntensityDepleted, element);
        }

        public static void EmitZombieKilled(Zombie zombie)
        {
            Instance.EmitSignal(SignalName.ZombieKilled, zombie);
        }

        public static void EmitCollision(KinematicCollision2D collision2D)
        {
            Instance.EmitSignal(SignalName.Collision, collision2D);
        }

        public static void EmitSpawnZombie(Zombie zombie)
        {
            Instance.EmitSignal(SignalName.ZombieSpawned, zombie);
        }
        public static void EmitPlayerHit()
        {
            Instance.EmitSignal(SignalName.PlayerHit);
        }
    }
}
