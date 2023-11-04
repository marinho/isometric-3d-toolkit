using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class BackgroundMusic : AudioStreamPlayer
    {
        public void OnSongFinished()
        {
            Play();
        }
    }
}
