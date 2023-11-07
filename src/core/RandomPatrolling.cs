using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class RandomPatrolling : Node3D
    {
        public enum MovementTypes
        {
            Idle,
            Move,
            Jump,
        }

        [Export]
        public MovementTypes MovementType;

        [Export]
        public float RandomDirection = 1f;

        [Export]
        public double TimeToChangeDirection = 3f;

        [Export]
        public double TimeToNextJump = 3f;

        [Export]
        public float JumpVelocity = 4.5f;

        [Export]
        public float FallingVelocity = 1f;

        [Export]
        public bool SeekTheTarget;

        [Signal]
        public delegate void OnMoveEventHandler();

        [Signal]
        public delegate void OnJumpEventHandler();

        [Signal]
        public delegate void OnLandingEventHandler();

        bool _AlreadyOnFloor = false;
        double _TimeToChangeDirectionCounter = 0f;
        double _TimeToNextJumpCounter = 0f;
        float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        public Node3D Target;
        bool ShouldMoveToTarget => SeekTheTarget && Target != null;
        Vector3 CurrentDirection;

        private Vector3 GetRandomDirection()
        {
            var randomX = new RandomNumberGenerator();
            randomX.Randomize();
            var directionX = randomX.RandfRange(-RandomDirection, RandomDirection);
            var randomY = new RandomNumberGenerator();
            randomY.Randomize();
            var directionY = randomY.RandfRange(-RandomDirection, RandomDirection);
            var simpleDirection = new Vector3(directionX, 0, directionY);
            return simpleDirection;
        }

        public void ApplyMovementOnProcess(CharacterBody3D body, float speed, double delta)
        {
            Vector3 velocity = body.Velocity;

            _TimeToChangeDirectionCounter += delta;
            _TimeToNextJumpCounter += delta;

            if (MovementType == MovementTypes.Jump && !_AlreadyOnFloor && body.IsOnFloor())
            {
                _AlreadyOnFloor = true;
                velocity = Vector3.Zero;
                EmitSignal("OnLanding");
            }
            else if (!body.IsOnFloor())
            {
                velocity.Y -= gravity * (float)delta * FallingVelocity;
            }
            else if (MovementType != MovementTypes.Idle)
            {
                ChangeDirectionIfItsTime(body);

                if (MovementType == MovementTypes.Move)
                    velocity = ApplyMovementForMove(body, speed, velocity);
                else if (MovementType == MovementTypes.Jump)
                    velocity = ApplyMovementForJump(body, speed, velocity);
            }

            body.Velocity = velocity;
            body.MoveAndSlide();
        }

        private Vector3 ApplyMovementForMove(CharacterBody3D body, float speed, Vector3 velocity)
        {
            LookAtDirection(body, CurrentDirection);
            velocity.X = CurrentDirection.X * speed;
            velocity.Z = CurrentDirection.Z * speed;

            EmitSignal("OnMove");

            return velocity;
        }

        private Vector3 ApplyMovementForJump(CharacterBody3D body, float speed, Vector3 velocity)
        {
            if (_TimeToNextJumpCounter < TimeToNextJump)
                return velocity;

            LookAtDirection(body, CurrentDirection);
            velocity.X = CurrentDirection.X * speed;
            velocity.Z = CurrentDirection.Z * speed;
            velocity.Y = JumpVelocity;

            _AlreadyOnFloor = false;
            _TimeToNextJumpCounter = 0f;
            EmitSignal("OnJump");

            return velocity;
        }

        private void ChangeDirectionIfItsTime(CharacterBody3D body)
        {
            if (_TimeToChangeDirectionCounter < TimeToChangeDirection)
                return;

            _TimeToChangeDirectionCounter = 0f;
            CurrentDirection = (
                ShouldMoveToTarget
                    ? (Target.GlobalPosition - body.GlobalPosition)
                    : (body.Transform.Basis * GetRandomDirection())
            ).Normalized();
        }

        private void LookAtDirection(CharacterBody3D body, Vector3 direction)
        {
            if (ShouldMoveToTarget)
                body.LookAt(Target.GlobalPosition, Vector3.Up);
            else if (direction != Vector3.Zero)
                body.LookAt(body.GlobalPosition + direction, Vector3.Up);
        }

        public void SetTarget(Node3D target)
        {
            Target = target;
        }
    }
}
