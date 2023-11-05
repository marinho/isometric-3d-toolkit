using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Isometric3DEngine
{
    public partial class EnemyAreaTrigger : Node3D
    {
        public enum ActionTypes
        {
            DoNothing,
            SetUp,
            SetDown,
            SetActive,
            SetInactive,
        }

        public enum TriggerTypes
        {
            OnEnemy,
            OnPlayer,
        }

        public enum ActionTargetType
        {
            AllEnemiesInside,
            OnlyEnemyThatTriggered,
        }

        [Export]
        public ActionTypes ActionTypeOnEnter;

        [Export]
        public ActionTypes ActionTypeOnExit;

        [Export]
        public TriggerTypes TriggerType;

        [Export]
        public ActionTargetType ActionTarget;

        [Export]
        public Area3D ChildrenArea;

        IPlayer Player;

        public void CharacterEntered(Node3D body)
        {
            if (body is IPlayer)
                Player = (IPlayer)body;

            if (TriggerType == TriggerTypes.OnEnemy && body is IEnemy)
                TriggerAction(body, ActionTypeOnEnter);
            else if (TriggerType == TriggerTypes.OnPlayer && body is IPlayer)
                TriggerAction(body, ActionTypeOnEnter);
        }

        public void CharacterExited(Node3D body)
        {
            if (body is IPlayer)
                Player = null;

            if (TriggerType == TriggerTypes.OnEnemy && body is IEnemy)
                TriggerAction(body, ActionTypeOnExit);
            else if (TriggerType == TriggerTypes.OnPlayer && body is IPlayer)
                TriggerAction(body, ActionTypeOnExit);
        }

        public void TriggerAction(Node3D body, ActionTypes actionType)
        {
            if (ActionTarget == ActionTargetType.AllEnemiesInside)
            {
                var enemies = GetAllEnemiesInside();
                foreach (var enemy in enemies)
                {
                    TriggerActionOnTarget(enemy, actionType);
                }
            }
            else if (ActionTarget == ActionTargetType.OnlyEnemyThatTriggered && body is IEnemy)
            {
                TriggerActionOnTarget((IEnemy)body, actionType);
            }
        }

        List<IEnemy> GetAllEnemiesInside()
        {
            if (ChildrenArea == null)
                return new List<IEnemy>();

            var children = ChildrenArea.GetOverlappingBodies().Cast<Node3D>().ToList();
            return children.Where(child => child is IEnemy).Cast<IEnemy>().ToList();
        }

        public void TriggerActionOnTarget(IEnemy enemy, ActionTypes actionType)
        {
            switch (actionType)
            {
                case ActionTypes.SetUp:
                    enemy.SetUp();
                    break;
                case ActionTypes.SetDown:
                    enemy.SetDown();
                    break;
                case ActionTypes.SetActive:
                    enemy.SetActive();
                    break;
                case ActionTypes.SetInactive:
                    enemy.SetInactive();
                    break;
                default:
                    break;
            }
        }
    }
}
