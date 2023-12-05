using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class MovingPlatform : Node3D, IActivable
    {
        [Export]
        public bool IsActive = false;

        [Export]
        public AnimationPlayer AnimationPlayer;

        [Export]
        public float AnimationSpeed = 1.0f;

        [Export]
        public string AnimationName = "platform_animation";

        public override void _Ready()
        {
            if (IsActive)
                Activate();
        }

        public void ActivatorAction() { }

        public void Activate()
        {
            IsActive = true;
            AnimationPlayer?.Play(AnimationName, customSpeed: AnimationSpeed);
        }

        public void Deactivate()
        {
            IsActive = false;
            AnimationPlayer?.Stop();
        }
    }
}
