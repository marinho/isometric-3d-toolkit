using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class IsometricCamera3D : Camera3D
    {
        CameraShaker _CameraShaker;
        PlayerManager _PlayerManager;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _CameraShaker = GetNode<CameraShaker>("%CameraShaker");

            _PlayerManager = GetNode<PlayerManager>("/root/PlayerManager");
            _PlayerManager.Died += ShakeCamera;
        }

        public void ShakeCamera()
        {
            _CameraShaker.ApplyShake();
        }
    }
}
