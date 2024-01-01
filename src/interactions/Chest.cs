using Godot;
using System;

namespace Isometric3DEngine
{
    public interface ICollectable
    {
        void Collect();
        string GetGamePersistenceItemId();
    }

    public partial class Chest : VisibleAndProcessTogether, IActivable
    {
        // property to set chest open
        [Export]
        public bool IsOpen = false;

        // property for the rotation angles of the chest top mesh instance
        [Export]
        public Vector3 OpenChestTopRotationAngles;

        [Export]
        public Vector3 ClosedChestTopRotationAngles;

        // property of the chest contents node
        [Export]
        public Node3D ChestContents;

        [ExportGroup("Game Persistence")]
        [Export]
        public string GamePersistenceItemId;

        [ExportGroup("Sounds")]
        [Export]
        public AudioStream OpenSound;

        [Export]
        public AudioStream CloseSound;

        [Signal]
        public delegate void ContentCollectedEventHandler(string chestItemId, string contentItemId);

        public new enum EventHandler
        {
            ContentCollected,
        }

        bool _PlayerIsNear = false;

        bool _OpenEffects = false; // used to store that the related effects have been played
        AudioStreamPlayer3D AudioPlayer;
        GamePersistence _GamePersistence;
        ActivatorArea _ActivatorArea;

        public override void _Ready()
        {
            // get sceneManager from root node
            AudioPlayer = GetNode<SceneManager>("/root/SceneManager").AddAudioPlayerToNode(this);
            _ActivatorArea = GetNode<ActivatorArea>("%Activator");

            _GamePersistence = GetNode<GamePersistence>("/root/GamePersistence");
            if (GamePersistenceItemId != "")
                IsOpen = _GamePersistence.GetStateBooleanItem(GamePersistenceItemId);
            ToggleOpen(IsOpen);

            if (IsOpen && ChestContents != null)
                ChestContents.Hide();
        }

        // method to toggle the chest open
        public void ToggleOpen(bool isOpen)
        {
            IsOpen = isOpen;
            _OpenEffects = isOpen;

            // disable activatro, so that the input balloon is not shown
            _ActivatorArea.CanDeactivate = _ActivatorArea.CanActivate = !isOpen;
            if (isOpen)
                _ActivatorArea.SetTriggerInputAvailable(false);

            // rotate the chest top
            var rotationAngles = isOpen ? OpenChestTopRotationAngles : ClosedChestTopRotationAngles;
            var chestTop = GetNode<Node3D>("%ChestTop");
            chestTop.RotationDegrees = rotationAngles;
        }

        public void SetOpen()
        {
            ToggleOpen(true);
            PlaySoundEffects(IsOpen);
        }

        public void SetClosed()
        {
            ToggleOpen(false);
            PlaySoundEffects(IsOpen);
        }

        public void ActivatorAction()
        {
            if (!_PlayerIsNear || IsOpen)
                return;

            SetOpen();

            _GamePersistence.SetStateBooleanItem(GamePersistenceItemId, true);

            var colectable = ChestContents as ICollectable;
            colectable.Collect();

            EmitSignal(
                EventHandler.ContentCollected.ToString(),
                GamePersistenceItemId,
                colectable.GetGamePersistenceItemId()
            );
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

        private void PlaySoundEffects(bool isOpen)
        {
            if (isOpen)
                AudioPlayer.Stream = OpenSound;
            else
                AudioPlayer.Stream = CloseSound;

            if (AudioPlayer.Stream != null)
                AudioPlayer.Play();
        }
    }
}
