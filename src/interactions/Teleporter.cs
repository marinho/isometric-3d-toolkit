using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class Teleporter : Node3D
    {
        public enum TeleporterDestinationType
        {
            AnotherScene,
            ThisScene
        }

        [Export]
        public TeleporterDestinationType DestinationType;

        [Export(PropertyHint.File)]
        public string DestinationScene;

        [Export]
        public string DestinationTransformPath; // can be either on current scene or the destination scene

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
                case TeleporterDestinationType.AnotherScene:
                    TeleportToAnotherScene();
                    break;
                case TeleporterDestinationType.ThisScene:
                    TeleportInCurrentScene();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TeleportToAnotherScene()
        {
            var sceneManager = GetTree().Root.GetNode<SceneManager>("/root/SceneManager");
            var parameters = new Godot.Collections.Dictionary<string, Variant>
            {
                { "destinationPath", DestinationTransformPath }
            };
            sceneManager.LoadSceneWithParams(DestinationScene, parameters);
        }

        private void TeleportInCurrentScene()
        {
            GlobalPosition = DestinationTransform.GlobalPosition;
            GlobalRotation = DestinationTransform.GlobalRotation;
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
