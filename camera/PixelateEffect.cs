using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class PixelateEffect : MeshInstance3D
    {
        [Export]
        public Curve TransitionCurve;

        // property for the time it takes to complete the transition
        [Export]
        public float TransitionTimeInSeconds = 3.0f;

        // properties for minimum and maximum pixel size
        [Export]
        public float MinPixelSize = 0.0f;

        [Export]
        public float MaxPixelSize = 16.0f;

        // property for minimum and maximum alpha
        [Export]
        public float MinAlpha = 0.0f;

        [Export]
        public float MaxAlpha = 1.0f;

        bool IsPlaying = false;
        float CurrentTime = 0.0f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready() { }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (!IsPlaying)
                return;
            if (CurrentTime >= TransitionTimeInSeconds)
            {
                IsPlaying = false;
                return;
            }

            CurrentTime += (float)delta;

            // delta between the minimum and maximum pixel size
            float deltaPixelSize = MaxPixelSize - MinPixelSize;

            float curveMultiplier = TransitionCurve.Sample(CurrentTime / TransitionTimeInSeconds);

            // calculate the pixel size by interpolating the curve with the current time
            float pixelSize = (curveMultiplier * deltaPixelSize) + MinPixelSize;

            // calculate the alpha by interpolating the curve with the current time
            float alpha = (curveMultiplier * (MaxAlpha - MinAlpha)) + MinAlpha;

            var shaderMaterial = (ShaderMaterial)Mesh.SurfaceGetMaterial(0);
            shaderMaterial.SetShaderParameter("pixel_size", pixelSize);
            shaderMaterial.SetShaderParameter("alpha", alpha);
        }

        // method to play the pixelate animation
        public void Play()
        {
            IsPlaying = true;
            CurrentTime = 0.0f;
        }
    }
}
