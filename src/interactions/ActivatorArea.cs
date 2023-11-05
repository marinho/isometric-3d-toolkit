using Godot;
using System;

namespace Isometric3DEngine
{
    public interface IActivable
    {
        void ActivatorAction();
        void Activate();
        void Deactivate();
    }

    public partial class ActivatorArea : Area3D
    {
        [ExportGroup("Activation")]
        [Export]
        public Node Activable;

        [Export]
        public bool CanActivate = true;

        [Export]
        public bool CanDeactivate = true;

        [Export]
        public bool CanInputAction = true;

        [Export]
        public bool ActivatedByPlayer = true;

        [Export]
        public bool ActivatedByEnemy = false;

        [ExportGroup("Action")]
        [Export]
        public string TextForInput = "Input Action";

        [Export]
        public int MaximumInputActions = 0; // 0 for infinite

        [Export]
        public float TimeBeforeNextInputAction = 0.0f;

        [Export]
        public string InputName = "Jump";

        [ExportGroup("Audio")]
        [Export]
        public AudioStream ActivateSound; // when player enters the area

        [Export]
        public AudioStream DeactivateSound; // when player exits the area

        [Export]
        public AudioStream InputActionSound; // when player presses action button

        // signal to be emitted when player enters the area
        [Signal]
        public delegate void ActivateEventHandler();

        [Signal]
        public delegate void ActivateWithBodyEventHandler(Node3D body);

        // signal to be emitted when player exits the area
        [Signal]
        public delegate void DeactivateEventHandler();

        [Signal]
        public delegate void DeactivateWithBodyEventHandler(Node3D body);

        [Signal]
        public delegate void InputActionEventHandler();

        ICharacter Character;
        AudioStreamPlayer3D AudioPlayer;
        int InputActionCounter = 0;
        float TimeToAllowNextAction = 0.0f;
        SignalManager signalManager;
        ActivatorManager activatorManager;

        // public enum with the possible event handlers in this class
        public enum EventHandler
        {
            Activate,
            ActivateWithBody,
            Deactivate,
            DeactivateWithBody,
            InputAction,
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AudioPlayer = GetNode<SceneManager>("/root/SceneManager").AddAudioPlayerToNode(this);
            signalManager = GetNode<SignalManager>("/root/SignalManager");
            activatorManager = GetNode<ActivatorManager>("/root/ActivatorManager");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (Character == null || !CanInputAction)
                return;

            if (MaximumInputActions > 0 && InputActionCounter >= MaximumInputActions)
                return;

            TimeToAllowNextAction -= (float)delta;

            if (TimeToAllowNextAction > 0)
                return;

            if (Input.IsActionJustPressed(InputName))
            {
                InputActionCounter++;
                TimeToAllowNextAction = TimeBeforeNextInputAction;

                if (Activable != null)
                    (Activable as IActivable).ActivatorAction();
                EmitSignal(EventHandler.InputAction.ToString());
            }
        }

        private bool BodyIsAccepted(Node body)
        {
            return (
                ActivatedByPlayer && body.IsInGroup(GameConsts.PlayerGroup)
                || ActivatedByEnemy && body.IsInGroup(GameConsts.EnemyGroup)
            );
        }

        public void _OnBodyEntered(Node3D body)
        {
            if (!BodyIsAccepted(body) || !CanActivate)
                return;

            if (CanInputAction)
                SetTriggerInputAvailable(true);

            if (ActivateSound != null)
                PlaySound(ActivateSound);

            Character = (ICharacter)body;

            if (Activable != null)
                (Activable as IActivable).Activate();
            EmitSignal(EventHandler.Activate.ToString());
            EmitSignal(EventHandler.ActivateWithBody.ToString(), body);
        }

        public void _OnBodyExited(Node3D body)
        {
            if (!BodyIsAccepted(body) || !CanActivate)
                return;

            if (CanInputAction)
                SetTriggerInputAvailable(false);

            if (Activable != null)
                (Activable as IActivable).Deactivate();
            EmitSignal(EventHandler.Deactivate.ToString());
            EmitSignal(EventHandler.DeactivateWithBody.ToString(), body);

            Character = null;

            if (DeactivateSound != null)
                PlaySound(DeactivateSound);
        }

        private void PlaySound(AudioStream sound)
        {
            if (AudioPlayer == null)
            {
                GD.Print("AudioPlayer is null");
                return;
            }
            if (sound == null)
            {
                GD.Print("Sound is null");
                return;
            }
            AudioPlayer.Stream = sound;
            AudioPlayer.Play();
        }

        private void SetTriggerInputAvailable(bool value)
        {
            signalManager.EmitSignal(
                SignalManagerEvent.SetInputControlDownHighlight.ToString(),
                value
            );

            if (value)
            {
                signalManager.EmitSignal(
                    SignalManagerEvent.SetInputControlDownLabel.ToString(),
                    TextForInput
                );
                signalManager.EmitSignal(SignalManagerEvent.SetPlayerCanJump.ToString(), false);
                activatorManager.ShowInputPromptEventHandler(GlobalPosition, TextForInput);
            }
            else
            {
                signalManager.EmitSignal(
                    SignalManagerEvent.SetInputControlDownLabelToDefault.ToString()
                );
                signalManager.EmitSignal(SignalManagerEvent.SetPlayerCanJump.ToString(), true);
                activatorManager.HideInputPromptEventHandler();
            }
        }
    }
}
