using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class VisibilitySwitcher : Node3D
    {
        // method to hide this node
        public void SetVisible()
        {
            Show();
        }

        // method to show this node
        public void SetHidden()
        {
            Hide();
        }
    }
}
