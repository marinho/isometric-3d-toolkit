using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class GroundButton : Node3D, IActivable
    {
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

        // signal to be emitted when button is pressed
        [Signal]
        public delegate void ButtonPressedEventHandler();

        // signal to be emitted when button is unpressed
        [Signal]
        public delegate void ButtonUnpressedEventHandler();

        AudioStreamPlayer3D AudioPlayer;

        // public enum with the possible event handlers in this class
        public enum EventHandler
        {
            ButtonPressed,
            ButtonUnpressed
        }

        bool _PressEffects = false; // used to store that the related effects have been played

        public void Activate()
        {
            // set the button as pressed
            IsPressed = true;

            // emit signal that button is pressed
            EmitSignal(EventHandler.ButtonPressed.ToString());
        }

        public void Deactivate()
        {
            if (!UnpressWhenPlayerExits)
                return;

            // set the button as unpressed
            IsPressed = false;

            // emit signal that button is unpressed
            EmitSignal(EventHandler.ButtonUnpressed.ToString());
        }

        // method to toggle the gate open
        public void TogglePressed(bool isPressed)
        {
            AudioPlayer.Play();

            IsPressed = isPressed;
            _PressEffects = isPressed;

            ButtonNode.MaterialOverride = isPressed ? PressedMaterial : UnpressedMaterial;

            ButtonNode.Transform = new Transform3D(
                ButtonNode.Transform.Basis,
                isPressed ? PressedOffset : Vector3.Zero
            );
        }

        // method on ready
        public override void _Ready()
        {
            AudioPlayer = GetNode<SceneManager>("/root/SceneManager").AddAudioPlayerToNode(this);
        }

        // method to process
        public override void _Process(double delta)
        {
            if (IsPressed ^ _PressEffects)
                TogglePressed(IsPressed);
        }

        public void ActivatorAction() { }
    }
}
