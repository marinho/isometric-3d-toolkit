using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class SceneManager : Node
    {
        AudioStreamPlayer3D AudioPlayer;

        string PlayerName = "%AudioStreamPlayer3D";

        public override void _Ready()
        {
            AudioPlayer = GetNode<AudioStreamPlayer3D>(PlayerName);
        }

        public AudioStreamPlayer3D AddAudioPlayerToNode(Node3D node)
        {
            var audioPlayer = node.GetNodeOrNull<AudioStreamPlayer3D>(PlayerName);
            if (audioPlayer != null)
                return audioPlayer;

            var newAudioPlayer = AudioPlayer.Duplicate() as AudioStreamPlayer3D;
            node.AddChild(newAudioPlayer);
            return newAudioPlayer;
        }
    }
}
