using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class ShaderMaterialParamSwitcher : Node
    {
        [Export]
        public MeshInstance3D MeshInstance;

        [Export]
        public string ParameterName;

        [Export]
        public int MaterialIndex = 0;

        public void SwitchOn()
        {
            var spiralMaterial = (ShaderMaterial)MeshInstance.GetActiveMaterial(MaterialIndex);
            spiralMaterial.SetShaderParameter(ParameterName, true);
        }

        public void SwitchOff()
        {
            var spiralMaterial = (ShaderMaterial)MeshInstance.GetActiveMaterial(MaterialIndex);
            spiralMaterial.SetShaderParameter(ParameterName, false);
        }
    }
}
