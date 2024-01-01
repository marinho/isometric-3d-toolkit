using Godot;
using System;

namespace Isometric3DEngine
{
    /**
     * MultiParticles : Node3D
     *
     * This class turns possible to control multiple particles in sync.
     * Inspired by https://www.reddit.com/r/godot/comments/181ui9c/comment/kaewcca/?context=3
     */
    public partial class MultiParticles : Node3D
    {
        [Export]
        public bool Emitting = true;

        [Export]
        public bool OneShot = false;

        [Export]
        public Godot.Collections.Array<GpuParticles3D> Particles;

        double _Lifetime = 0.0f;
        public double Lifetime => _Lifetime;

        public override void _Ready()
        {
            UpdateLifetime();

            if (Emitting)
                StartEmitters();
        }

        void UpdateLifetime()
        {
            foreach (var particle in Particles)
            {
                if (particle.Lifetime > _Lifetime)
                {
                    _Lifetime = particle.Lifetime;
                }
            }
        }

        public async void StartEmitters()
        {
            UpdateLifetime();

            foreach (var particle in Particles)
                particle.Emitting = true;

            await ToSignal(GetTree().CreateTimer(_Lifetime), SceneTreeTimer.SignalName.Timeout);
            DisposeOfEmitters();

            if (!OneShot)
            {
                StartEmitters();
            }
        }

        void DisposeOfEmitters()
        {
            foreach (var particle in Particles)
            {
                particle.Emitting = false;
            }
        }
    }
}
