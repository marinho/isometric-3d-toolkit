using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class CharacterHealth : Node
    {
        [Export]
        public bool HealthIsInfinite = false;

        [Export]
        public float MaximumHealth = 10f;

        [Export]
        public float MinimumHealth = 0f;

        [Export]
        public float CurrentHealth = 8f;

        [Export]
        public float HealthEndThreshold = 0.05f;

        [Signal]
        public delegate void HealthUpdatedEventHandler(float maximumHealth, float currentHealth);

        [Signal]
        public delegate void DiedEventHandler();

        // public enum with the possible event handlers in this class
        public enum EventHandler
        {
            HealthUpdated,
            Died,
        }

        public void DecreaseHealth(float value)
        {
            if (HealthIsInfinite)
                return;

            CurrentHealth -= value;
            UpdateHealthEvent();

            // if the player has no ink left, lose the game
            if (CurrentHealth <= MinimumHealth)
                Die();
        }

        public bool IsFullyCharged()
        {
            return CurrentHealth >= MaximumHealth;
        }

        public void IncreaseHealth(float value, bool dieIfOverflow = false)
        {
            SetCurrentHealth(CurrentHealth + value, dieIfOverflow);
        }

        public void SetCurrentHealth(float value, bool dieIfOverflow = false)
        {
            if (HealthIsInfinite)
                return;

            // increase the ink volume
            CurrentHealth = value;
            UpdateHealthEvent();

            // if the player has more ink than the maximum, lose the game
            if (CurrentHealth >= MaximumHealth)
            {
                if (dieIfOverflow)
                {
                    Die();
                }
                else
                {
                    CurrentHealth = MaximumHealth;
                }
            }
        }

        public void UpdateMaximumHealth(float value)
        {
            MaximumHealth = value;
            UpdateHealthEvent();
        }

        public void Die()
        {
            EmitSignal(EventHandler.Died.ToString());
        }

        public bool HealthIsEnding()
        {
            return (CurrentHealth / MaximumHealth) < HealthEndThreshold;
        }

        private void UpdateHealthEvent()
        {
            EmitSignal(EventHandler.HealthUpdated.ToString(), MaximumHealth, CurrentHealth);
        }
    }
}
