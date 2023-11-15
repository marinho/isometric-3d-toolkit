using Godot;
using System;

namespace Isometric3DEngine
{
    /**
     * SceneManager : AutoLoad
     *
     * This class is responsible for managing the scene transitions.
     * It is used to load a new scene and to pass parameters to it.
     */
    public partial class SceneManager : Node
    {
        AudioStreamPlayer3D AudioPlayer;

        string PlayerName = "%AudioStreamPlayer3D";

        string ScenePathToLoad;
        Godot.Collections.Dictionary<string, Variant> ParametersForNextScene;

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

        public void LoadScene(string scenePath)
        {
            GD.Print($"Teleporting to {scenePath}");
            ScenePathToLoad = scenePath;
            GetTree().ChangeSceneToFile(scenePath);
        }

        public void LoadSceneWithParams(
            string scenePath,
            Godot.Collections.Dictionary<string, Variant> parameters
        )
        {
            ParametersForNextScene = parameters;
            LoadScene(scenePath);
        }

        public Godot.Collections.Dictionary<string, Variant> GetSceneParams(string scenePath)
        {
            if (scenePath != ScenePathToLoad)
                return null;

            return ParametersForNextScene;
        }
    }
}
