using Godot;
using System;

namespace Isometric3DEngine
{
    // thanks to: https://www.youtube.com/watch?v=_R2KDcAp1YQ

    public partial class FloatingBody3D : RigidBody3D
    {
        [Export]
        public float FloatForce = 3f;

        [Export]
        public float WaterDrag = 0.05f;

        [Export]
        public float WaterAngularDrag = 0.05f;

        [Export]
        public float WaterHeight = 0f;

        [Export]
        public float LowerHeightUnderBody = .2f;

        [Export]
        public float FloatingHeightVariance = .1f;

        [Export]
        public float FloatingPulse = 1f;

        bool IsSubmerged = false;
        bool IsUnderBody = false;
        float TimeCounter = 0f;

        // Get the gravity from the project settings to be synced with RigidBody nodes.
        public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready() { }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _PhysicsProcess(double delta)
        {
            TimeCounter += (float)delta;
            float SineHeight =
                (float)Math.Sin(TimeCounter * FloatingPulse) * FloatingHeightVariance;

            var depth =
                WaterHeight
                + SineHeight
                - (IsUnderBody ? LowerHeightUnderBody : 0f)
                - GlobalPosition.Y;

            if (depth > 0)
            {
                IsSubmerged = true;
                ApplyCentralForce(Vector3.Up * FloatForce * gravity * depth);
            }
            else
            {
                IsSubmerged = false;
            }
        }

        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
            if (IsSubmerged)
            {
                state.LinearVelocity *= 1 - WaterDrag;
                state.AngularVelocity *= 1 - WaterAngularDrag;
            }
        }

        public void _OnBodyEntered(Node3D body)
        {
            IsUnderBody = true;
        }

        public void _OnBodyExited(Node3D body)
        {
            IsUnderBody = false;
        }
    }
}
