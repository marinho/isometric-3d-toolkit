using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class ShaderMaterialParamSwitcher : Node, IActivable
    {
        [Export]
        public MeshInstance3D MeshInstance;

        [Export]
        public string ParameterName;

        [Export]
        public int MaterialIndex = 0;

        public void Activate()
        {
            var spiralMaterial = (ShaderMaterial)MeshInstance.GetActiveMaterial(MaterialIndex);
            spiralMaterial.SetShaderParameter(ParameterName, true);
        }

        public void Deactivate()
        {
            var spiralMaterial = (ShaderMaterial)MeshInstance.GetActiveMaterial(MaterialIndex);
            spiralMaterial.SetShaderParameter(ParameterName, false);
        }

        public void ActivatorAction() { }
    }
}
