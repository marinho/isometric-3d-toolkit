using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class ActivableLight : SpotLight3D, IActivable
    {
        [Export]
        public float ActivatedEnergy = 5f;

        [Export]
        public float DeactivatedEnergy = 0f;

        public void ActivatorAction() { }

        public void Activate()
        {
            LightEnergy = ActivatedEnergy;
        }

        public void Deactivate()
        {
            LightEnergy = DeactivatedEnergy;
        }
    }
}
