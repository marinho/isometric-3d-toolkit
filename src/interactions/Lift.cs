using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class Lift : Node3D, IActivable
    {
        [Export]
        public Vector3 LiftedPosition;

        [Export]
        public Vector3 RestingPosition;

        public void ActivatorAction() { }

        public void Activate()
        {
            Position = LiftedPosition;
        }

        public void Deactivate()
        {
            Position = RestingPosition;
        }
    }
}
