using Godot;
using System;

namespace Isometric3DEngine
{
    /**
     * BackgroundMusic : AutoLoad
     *
     * This class is responsible for managing the background music.
     */
    public partial class BackgroundMusic : AudioStreamPlayer
    {
        public void OnSongFinished()
        {
            Play();
        }
    }
}
