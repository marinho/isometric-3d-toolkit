using Godot;

namespace Isometric3DEngine
{
    public interface ICharacter { }

    public interface IPlayer : ICharacter
    {
        void BackToLastGroundPosition();
        void SetLastGroundPosition();
        void SetGlobalPosition(Vector3 newGlobalPosition);
    }

    public interface IEnemy : ICharacter
    {
        void SetUp();
        void SetDown();
        void SetActive();
        void SetInactive();
    }

    public interface INpc : ICharacter { }
}
