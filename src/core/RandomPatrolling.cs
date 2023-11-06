using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class RandomPatrolling : Node3D
    {
        public enum MovementTypes
        {
            Move,
            Jump,
        }

        [Export]
        public MovementTypes MovementType;

        [Export]
        public float RandomDirection = 1f;

        [Export]
        public double TimeToJump = 3f;

        [Export]
        public float JumpVelocity = 4.5f;

        [Export]
        public bool SeekTheTarget;

        [Signal]
        public delegate void OnJumpEventHandler();

        [Signal]
        public delegate void OnLandingEventHandler();

        bool _AlreadyOnFloor = false;
        double _TimeToChangeDirectionCounter = 0f;
        float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        public Node3D Target;
        bool ShouldMoveToTarget => SeekTheTarget && Target != null;

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
            var itsTimeToChangeDirection = _TimeToChangeDirectionCounter >= TimeToJump;

            if (MovementType == MovementTypes.Jump && !_AlreadyOnFloor && body.IsOnFloor())
            {
                _AlreadyOnFloor = true;
                EmitSignal("OnLanding");
            }

            if (!body.IsOnFloor())
                velocity.Y -= gravity * (float)delta;
            else if (itsTimeToChangeDirection)
            {
                if (MovementType == MovementTypes.Jump)
                    velocity.Y = JumpVelocity;

                _TimeToChangeDirectionCounter = 0f;

                Vector3 simpleDirection = ShouldMoveToTarget
                    ? Target.Position
                    : GetRandomDirection();
                Vector3 direction = (body.Transform.Basis * simpleDirection).Normalized();

                velocity.X = direction.X * speed;
                velocity.Z = direction.Z * speed;

                if (ShouldMoveToTarget)
                {
                    LookAt(Target.Position, Vector3.Up);
                }
                else
                {
                    float magicUnexplainableNumber = 5f;
                    LookAt(
                        body.Position + (simpleDirection * magicUnexplainableNumber),
                        Vector3.Up
                    );
                }

                EmitSignal("OnJump");
            }
            else
            {
                velocity.X = Mathf.MoveToward(body.Velocity.X, 0, speed);
                velocity.Z = Mathf.MoveToward(body.Velocity.Z, 0, speed);
            }

            body.Velocity = velocity;
            body.MoveAndSlide();
        }

        public void SetTarget(Node3D target)
        {
            Target = target;
        }
    }
}
