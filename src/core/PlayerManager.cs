using Godot;
using System;

namespace Isometric3DEngine
{
    /**
     * PlayerManager : AutoLoad
     *
     * This class is responsible for managing the player life.
     */
    public partial class PlayerManager : Node
    {
        [Signal]
        public delegate void StartedOnSceneEventHandler();

        [Signal]
        public delegate void DiedEventHandler();

        // public enum with the possible event handlers in this class
        public enum EventHandler
        {
            StartedOnScene,
            Died,
        }

        // Called when the node enters a scene for the first time. I.e. on game start, after teleport, after died, etc.
        public void StartOnScene()
        {
            EmitSignal(EventHandler.StartedOnScene.ToString());
        }

        // Called when the player dies (so that it can be respawned back to initial position of that scene)
        public void Die()
        {
            EmitSignal(EventHandler.Died.ToString());

            var signalManager = GetNode<SignalManager>("/root/SignalManager");
            signalManager.EmitSignal(
                SignalManagerEvent.ShowMessage.ToString(),
                "You ran out of ink!"
            );
        }
    }
}
