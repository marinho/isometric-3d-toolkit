using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class AudioPlayInLoop : AudioStreamPlayer3D
    {
        [Export]
        public bool PlayInLoop = true;

        public override void _Ready()
        {
            Finished += () =>
            {
                if (PlayInLoop)
                    Play();
            };
        }
    }
}
