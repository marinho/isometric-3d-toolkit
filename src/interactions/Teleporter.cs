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

        public void ActivatorAction()
        {
            // TODO do a fancy loading animation

            switch (DestinationType)
            {
                case TeleporterDestinationType.Scene:
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
    }
}
