namespace Isometric3DEngine
{
    public interface ICharacter { }

    public interface IPlayer : ICharacter
    {
        void BackToLastGroundPosition();
        void SetLastGroundPosition();
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
