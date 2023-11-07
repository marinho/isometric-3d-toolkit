using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class VisibilitySwitcher : Node3D, IActivable
    {
        // method to hide this node
        public void Activate()
        {
            Show();
        }

        // method to show this node
        public void Deactivate()
        {
            Hide();
        }

        public void ActivatorAction() { }
    }
}
