using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class ActivatorManager : Control
    {
        // property for the hint node
        [Export]
        public Control InputPromptNode;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready() { }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta) { }

        public void ShowInputPromptEventHandler(Vector3 globalPosition, string actionLabel)
        {
            var player = GetTree().GetFirstNodeInGroup("Player") as PenPlayer;
            var playerGuide = player.InputPromptGuide;
            var camera = GetViewport().GetCamera3D();
            Vector2 viewportPosition = camera.UnprojectPosition(playerGuide.GlobalPosition);
            GD.Print("ActivatorManager.ShowInputPromptEventHandler: ", viewportPosition); // XXX
            InputPromptNode.SetPosition(viewportPosition - InputPromptNode.Size / 2);
            InputPromptNode.Show();
        }

        public void HideInputPromptEventHandler()
        {
            InputPromptNode.Hide();
        }
    }
}
