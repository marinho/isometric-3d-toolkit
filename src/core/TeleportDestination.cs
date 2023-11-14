using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class TeleportDestination : Node3D
    {
        public override void _Ready()
        {
            string scenePath = GetTree().CurrentScene.SceneFilePath;
            var sceneManager = GetNode<SceneManager>("/root/SceneManager");
            var parameters = sceneManager.GetSceneParams(scenePath);

            if (parameters != null && parameters["destinationPath"].ToString() == $"%{Name}")
            {
                var player = GetTree().GetFirstNodeInGroup(GameConsts.PlayerGroup) as IPlayer;
                if (player != null)
                {
                    player.SetGlobalPosition(GlobalPosition);
                }
            }
        }
    }
}
