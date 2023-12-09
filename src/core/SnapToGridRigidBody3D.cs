using Godot;
using System;

namespace Isometric3DEngine
{
    /**
     * SnapToGridRigidBody3D
     *
     * to be used in an object that can be pushed by the player and should snap to a grid when pushed
     */
    public partial class SnapToGridRigidBody3D : RigidBody3D
    {
        [Export]
        public float GridBreakPoint = 1.0f; // so that when pushed, the object will move to the next multiple of this number

        public enum MovementAxisTypes
        {
            X,
            Z
        }

        [Export]
        public MovementAxisTypes MovementAxis;

        Vector3 PreviousPosition;
        Vector3 TargetPosition;
        bool IsMoving = false;
        bool CanBePushed = false;
        float TimeBetweenMoves = 1.0f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AxisLockLinearX = MovementAxis == MovementAxisTypes.Z;
            AxisLockLinearZ = MovementAxis == MovementAxisTypes.X;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _PhysicsProcess(double delta)
        {
            if (IsMoving)
            {
                if (GlobalPosition.DistanceTo(TargetPosition) < 0.1f)
                {
                    GlobalPosition = TargetPosition;
                    LinearVelocity = Vector3.Zero;
                    IsMoving = false;
                }
                else if (GlobalPosition.DistanceTo(PreviousPosition) < 0.01f)
                {
                    IsMoving = false;
                }
                else
                {
                    PreviousPosition = GlobalPosition;
                }
            }
        }

        public void MoveToSnap(Vector3 impulse)
        {
            if (!CanBePushed)
                return;

            if (IsMoving)
                return;

            float newX = Mathf.Clamp(impulse.X, -GridBreakPoint, GridBreakPoint);
            float newZ = Mathf.Clamp(impulse.Z, -GridBreakPoint, GridBreakPoint);
            var direction = new Vector3(newX, 0, newZ);

            TargetPosition = new Vector3(
                Mathf.Round(GlobalPosition.X / GridBreakPoint) * GridBreakPoint + direction.X,
                GlobalPosition.Y,
                Mathf.Round(GlobalPosition.Z / GridBreakPoint) * GridBreakPoint + direction.Z
            );

            IsMoving = true;

            ApplyCentralImpulse(impulse);
        }

        // To be connected from an Area3D.body_entered signal to ensure the player is at a position where it can be pushed
        public void ActivateCanBePushed(Node3D body)
        {
            CanBePushed = true;
        }

        // To be connected from an Area3D.body_exited signal to ensure the player is at a position where it can be pushed
        public void DeactivateCanBePushed(Node3D body)
        {
            CanBePushed = false;
        }
    }
}
