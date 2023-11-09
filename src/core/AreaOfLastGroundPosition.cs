using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class AreaOfLastGroundPosition : Area3D
    {
        public void _OnBodyExited(Node3D body)
        {
            // get the player
            var player = (IPlayer)body;
            player.SetLastGroundPosition();
        }
    }
}
