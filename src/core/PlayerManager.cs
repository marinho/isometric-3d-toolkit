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
        public delegate void DiedEventHandler();

        // public enum with the possible event handlers in this class
        public enum EventHandler
        {
            Died
        }

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
