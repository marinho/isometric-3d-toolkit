using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class GroundButton : Node3D, IActivable
    {
        [Export]
        public string GamePersistenceItemId;

        // property to set gate is open
        [Export]
        public bool IsPressed = false;

        // property to define to unpress when player exits the area
        [Export]
        public bool UnpressWhenPlayerExits = false;

        // property for the button node
        [Export]
        public MeshInstance3D ButtonNode;

        // property for spatial material when it's unpressed
        [Export]
        public Material UnpressedMaterial;

        // property for spatial material when it's pressed
        [Export]
        public Material PressedMaterial;

        // property for offset when it's pressed
        [Export]
        public Vector3 PressedOffset;

        [Export]
        public bool IsPersistent = false;

        [ExportGroup("Sounds")]
        [Export]
        public AudioStream PressSound;

        [Export]
        public AudioStream UnpressSound;

        // signal to be emitted when button is pressed
        [Signal]
        public delegate void ButtonPressedEventHandler();

        [Signal]
        public delegate void ButtonPressedWithObjectEventHandler(GroundButton button);

        // signal to be emitted when button is unpressed
        [Signal]
        public delegate void ButtonUnpressedEventHandler();

        [Signal]
        public delegate void ButtonUnpressedWithObjectEventHandler(GroundButton button);

        AudioStreamPlayer3D AudioPlayer;
        GamePersistence _GamePersistence;

        // public enum with the possible event handlers in this class
        public enum EventHandler
        {
            ButtonPressed,
            ButtonPressedWithObject,
            ButtonUnpressed,
            ButtonUnpressedWithObject,
        }

        bool _PressEffects = false; // used to store that the related effects have been played

        public void Activate()
        {
            // set the button as pressed
            TogglePressed(true);

            // emit signal that button is pressed
            // EmitSignal(EventHandler.ButtonPressed.ToString());
            // EmitSignal(EventHandler.ButtonPressedWithObject.ToString(), this);
        }

        public void Deactivate()
        {
            if (!UnpressWhenPlayerExits)
                return;

            // set the button as unpressed
            TogglePressed(false);

            // emit signal that button is unpressed
            // EmitSignal(EventHandler.ButtonUnpressed.ToString());
            // EmitSignal(EventHandler.ButtonUnpressedWithObject.ToString(), this);
        }

        // method to toggle the button open
        public void TogglePressed(bool isPressed)
        {
            PlaySoundEffects(isPressed);

            IsPressed = isPressed;
            _PressEffects = isPressed;

            if (IsPersistent && GamePersistenceItemId != "")
                _GamePersistence.SetStateBooleanItem(GamePersistenceItemId, isPressed);

            ButtonNode.MaterialOverride = isPressed ? PressedMaterial : UnpressedMaterial;

            ButtonNode.Transform = new Transform3D(
                ButtonNode.Transform.Basis,
                isPressed ? PressedOffset : Vector3.Zero
            );

            if (isPressed)
            {
                EmitSignal(EventHandler.ButtonPressed.ToString());
                EmitSignal(EventHandler.ButtonPressedWithObject.ToString(), this);
            }
            else
            {
                EmitSignal(EventHandler.ButtonUnpressed.ToString());
                EmitSignal(EventHandler.ButtonUnpressedWithObject.ToString(), this);
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

            _GamePersistence = GetNode<GamePersistence>("/root/GamePersistence");
            if (GamePersistenceItemId != "")
                IsPressed = _GamePersistence.GetStateBooleanItem(GamePersistenceItemId);
        }

        // method to process
        public override void _PhysicsProcess(double delta)
        {
            // if (IsPressed ^ _PressEffects)
            //     TogglePressed(IsPressed);
        }

        public void ActivatorAction() { }
    }
}
