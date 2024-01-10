using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class MovingPlatform : Node3D, IActivable
    {
        [Export]
        public bool IsActive = false;

        [Export]
        public float AnimationSpeed = 1.0f;

        [Export]
        public string AnimationName = "platform_animation";

        [Export]
        public float WaitToStart = 0.0f;

        AnimationPlayer _AnimationPlayer;

        public override void _Ready()
        {
            _AnimationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");

            if (IsActive)
                Activate();
        }

        public void ActivatorAction() { }

        public async void Activate()
        {
            await ToSignal(GetTree().CreateTimer(WaitToStart), SceneTreeTimer.SignalName.Timeout);

            IsActive = true;
            _AnimationPlayer?.Play(AnimationName, customSpeed: AnimationSpeed);
        }

        public void Deactivate()
        {
            IsActive = false;
            _AnimationPlayer?.Stop();
        }
    }
}
