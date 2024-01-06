using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class Lever : Node3D, IActivable
    {
        // property to set lever is active
        [Export]
        public bool IsPressed = false;

        [Export]
        public bool CanBeUnpressed = true;

        // property for spatial material when it's unpressed
        [Export]
        public Material UnpressedMaterial;

        // property for spatial material when it's pressed
        [Export]
        public Material PressedMaterial;

        [Export]
        public Vector3 RotationPressed;

        [Export]
        public Vector3 RotationUnpressed;

        [ExportGroup("Game Persistence")]
        [Export]
        public string GamePersistenceItemId;

        [Export]
        public bool IsPersistent = false;

        [ExportGroup("Sounds")]
        [Export]
        public AudioStream PressSound;

        [Export]
        public AudioStream UnpressSound;

        // signal to be emitted when button is pressed
        [Signal]
        public delegate void LeverPressedEventHandler();

        [Signal]
        public delegate void LeverPressedWithObjectEventHandler(Lever lever);

        // signal to be emitted when button is unpressed
        [Signal]
        public delegate void LeverUnpressedEventHandler();

        [Signal]
        public delegate void LeverUnpressedWithObjectEventHandler(Lever lever);

        AudioStreamPlayer3D AudioPlayer;
        GameState _GameState;

        Node3D _RotableContainer;
        MeshInstance3D _Stick;

        // public enum with the possible event handlers in this class
        public enum EventHandler
        {
            LeverPressed,
            LeverPressedWithObject,
            LeverUnpressed,
            LeverUnpressedWithObject,
        }

        bool _PressEffects = false; // used to store that the related effects have been played

        public void Activate()
        {
            // set the button as pressed
            TogglePressed(!IsPressed);
        }

        public void Deactivate() { }

        // method to toggle the button open
        public void TogglePressed(bool isPressed, bool silently = false)
        {
            if (!isPressed && !CanBeUnpressed)
                return;

            if (!silently && IsPressed != isPressed)
                PlaySoundEffects(isPressed);

            IsPressed = isPressed;
            _PressEffects = isPressed;

            if (IsPersistent && GamePersistenceItemId != "")
                _GameState.SetStateBooleanItem(GamePersistenceItemId, isPressed);

            _Stick.MaterialOverride = isPressed ? PressedMaterial : UnpressedMaterial;
            _RotableContainer.Rotation = isPressed ? RotationPressed : RotationUnpressed;

            if (isPressed)
            {
                EmitSignal(EventHandler.LeverPressed.ToString());
                EmitSignal(EventHandler.LeverPressedWithObject.ToString(), this);
            }
            else
            {
                EmitSignal(EventHandler.LeverUnpressed.ToString());
                EmitSignal(EventHandler.LeverUnpressedWithObject.ToString(), this);
            }
        }

        private void PlaySoundEffects(bool isPressed)
        {
            if (isPressed)
                AudioPlayer.Stream = PressSound;
            else
                AudioPlayer.Stream = UnpressSound;

            if (AudioPlayer.Stream != null)
                AudioPlayer.Play();
        }

        // method on ready
        public override void _Ready()
        {
            AudioPlayer = GetNode<SceneManager>("/root/SceneManager").AddAudioPlayerToNode(this);

            _Stick = GetNode<MeshInstance3D>("%Stick");
            _RotableContainer = GetNode<Node3D>("%RotableContainer");

            _GameState = GetNode<GameState>("/root/GameState");
            if (GamePersistenceItemId != "" && IsPersistent)
            {
                bool savedIsPressed = _GameState.GetStateBooleanItem(GamePersistenceItemId);
                TogglePressed(savedIsPressed, true);
            }
        }

        // method to process
        public override void _PhysicsProcess(double delta) { }

        public void ActivatorAction() { }
    }
}
