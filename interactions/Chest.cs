using Godot;
using System;

namespace Isometric3DEngine
{
    public interface ICollectable
    {
        void Collect();
    }

    public partial class Chest : Node3D, IActivable
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

        bool _PlayerIsNear = false;

        bool _OpenEffects = false; // used to store that the related effects have been played
        AudioStreamPlayer3D AudioPlayer;

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

            // hide the chest contents
            ChestContents.Visible = false;

            (ChestContents as ICollectable).Collect();
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
