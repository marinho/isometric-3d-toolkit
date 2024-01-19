using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Isometric3DEngine
{
    /**
     * Search for all children of type MeshInstance3D and set the material override to the one specified in the LeavesMaterial property.
     */
    public partial class SwaysOnWind : Node3D
    {
        [Export]
        public NodePath RootNodePath;

        [Export]
        public ShaderMaterial LeavesMaterial;

        [Export]
        public bool SetMaterialOverride = true;

        [Export]
        public string GroupName = "SwaysOnWind";

        [Export]
        public float TimeToWaitBeforeApply = 1.0f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (SetMaterialOverride)
                ApplyMaterialOverride();
        }

        async void ApplyMaterialOverride()
        {
            // wait a bit before applying the material override
            var timer = GetTree().CreateTimer(TimeToWaitBeforeApply);
            await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

            var rootNode = GetNode<Node3D>(RootNodePath);
            var nodesWithMeshes = GetAllNodesInGroup(rootNode, GroupName);

            foreach (var nodeWithMeshes in nodesWithMeshes)
            {
                var meshes = nodeWithMeshes.GetChildren().OfType<MeshInstance3D>();
                foreach (var mesh in meshes)
                {
                    mesh.MaterialOverride = LeavesMaterial;
                }
            }
        }

        List<Node3D> GetAllNodesInGroup(Node3D root, string groupName)
        {
            var nodes = new List<Node3D>();
            if (root.IsInGroup(groupName))
                nodes.Add(root);

            foreach (var child in root.GetChildren())
            {
                if (child is Node3D childNode)
                {
                    nodes.AddRange(GetAllNodesInGroup(childNode, groupName));
                }
            }

            return nodes;
        }
    }
}
