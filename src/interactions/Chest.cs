using Godot;
using System;

namespace Isometric3DEngine
{
    public interface ICollectable
    {
        void Collect();
    }

    public partial class Chest : VisibleAndProcessTogether, IActivable
    {
        // property to set chest open
        [Export]
        public bool IsOpen = false;

        // property for the chest top mesh instance
        [Export]
        public Node3D ChestTopMeshInstance;

        // property for the rotation angles of the chest top mesh instance
        [Export]
        public Vector3 OpenChestTopRotationAngles;

        [Export]
        public Vector3 ClosedChestTopRotationAngles;

        // property of the chest contents node
        [Export]
        public Node3D ChestContents;

        [Export]
        public string GamePersistenceItemId;

        [Signal]
        public delegate void ContentCollectedEventHandler();

        public new enum EventHandler
        {
            ContentCollected,
        }

        bool _PlayerIsNear = false;

        bool _OpenEffects = false; // used to store that the related effects have been played
        AudioStreamPlayer3D AudioPlayer;
        GamePersistence _GamePersistence;

        // method to toggle the gate open
        public void ToggleOpen(bool isOpen)
        {
            IsOpen = isOpen;
            _OpenEffects = isOpen;

            var rotationAngles = isOpen ? OpenChestTopRotationAngles : ClosedChestTopRotationAngles;

            // rotate the scissors
            ChestTopMeshInstance.RotationDegrees = new Vector3(
                rotationAngles.X,
                rotationAngles.Y,
                rotationAngles.Z
            );
        }

        public void SetOpen()
        {
            ToggleOpen(true);
        }

        public void SetClosed()
        {
            ToggleOpen(false);
        }

        public override void _Ready()
        {
            // get sceneManager from root node
            AudioPlayer = GetNode<SceneManager>("/root/SceneManager").AddAudioPlayerToNode(this);

            _GamePersistence = GetNode<GamePersistence>("/root/GamePersistence");
            IsOpen = _GamePersistence.GetStateBooleanItem(GamePersistenceItemId);

            if (IsOpen && ChestContents != null)
                ChestContents.Hide();
        }

        // method to process
        public override void _Process(double delta)
        {
            if (IsOpen ^ _OpenEffects)
                ToggleOpen(IsOpen);
        }

        public void ActivatorAction()
        {
            if (!_PlayerIsNear || IsOpen)
                return;

            SetOpen();

            _GamePersistence.SetStateBooleanItem(GamePersistenceItemId, true);

            (ChestContents as ICollectable).Collect();

            EmitSignal(EventHandler.ContentCollected.ToString());

            GetTree().CreateTimer(.5f).Timeout += () => ChestContents.Hide();
        }

        public void Activate()
        {
            if (!IsOpen)
                _PlayerIsNear = true;
        }

        public void Deactivate()
        {
            _PlayerIsNear = false;
        }
    }
}
