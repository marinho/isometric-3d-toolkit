using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class Teleporter : Node3D
    {
        public enum TeleporterDestinationType
        {
            Scene,
            Transform
        }

        [Export]
        public TeleporterDestinationType DestinationType;

        [Export(PropertyHint.File)]
        public string DestinationScene;

        [Export]
        public Node3D DestinationTransform;

        [Export]
        public bool ActionOnEnter = false;

        ActivatorArea _Activator;

        public override void _Ready()
        {
            _Activator = GetNode<ActivatorArea>("%activator");
            _Activator.CanInputAction = !ActionOnEnter;
        }

        public void ActivatorAction()
        {
            // TODO do a fancy loading animation

            switch (DestinationType)
            {
                case TeleporterDestinationType.Scene:
                    GD.Print($"Teleporting to {DestinationScene}");
                    GetTree().ChangeSceneToFile(DestinationScene);
                    break;
                case TeleporterDestinationType.Transform:
                    GlobalPosition = DestinationTransform.GlobalPosition;
                    GlobalRotation = DestinationTransform.GlobalRotation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void _Activator_Body_Entered()
        {
            if (ActionOnEnter)
            {
                ActivatorAction();
            }
        }
    }
}
