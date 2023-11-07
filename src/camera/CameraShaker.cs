using Godot;

// thanks to https://www.youtube.com/watch?v=LGt-jjVf-ZU
namespace Isometric3DEngine
{
    public partial class CameraShaker : Node
    {
        [Export]
        public Node3D Target;

        [Export]
        public float RandomStrenght = 30f;

        [Export]
        public float ShakeFade = 9f;

        [Signal]
        public delegate void SetCameraDampingEventHandler(bool damping);

        RandomNumberGenerator rnd = new RandomNumberGenerator();
        float shakeStrength = 0.0f;

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (shakeStrength <= 0.0f)
                return;

            EmitSignal("SetCameraDamping", false);

            shakeStrength = Mathf.Lerp(shakeStrength, 0.0f, (float)delta * ShakeFade);
            Target.Transform = new Transform3D(
                Target.Transform.Basis,
                Target.Transform.Origin + RandomOffset()
            );
        }

        public void ApplyShake()
        {
            shakeStrength = RandomStrenght;
        }

        Vector3 RandomOffset()
        {
            return new Vector3(
                rnd.RandfRange(-shakeStrength, shakeStrength),
                rnd.RandfRange(-shakeStrength, shakeStrength),
                rnd.RandfRange(-shakeStrength, shakeStrength)
            );
        }
    }
}
