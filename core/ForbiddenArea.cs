using Godot;

namespace Isometric3DEngine
{
    public partial class ForbiddenArea : Area3D
    {
        public void _OnBodyEntered(Node3D body)
        {
            if (!body.IsInGroup(GameConsts.PlayerGroup))
                return;

            var player = body as IPlayer;
            player.BackToLastGroundPosition();
        }
    }
}
