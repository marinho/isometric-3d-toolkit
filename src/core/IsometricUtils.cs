using Godot;

namespace Isometric3DEngine
{
    public static class IsometricUtils
    {
        static float angleDegrees = -45;
        static float cosAngle = Mathf.Cos(Mathf.DegToRad(angleDegrees));
        static float sinAngle = Mathf.Sin(Mathf.DegToRad(angleDegrees));

        // DEPRECATED
        public static Vector2 InputDirectionToIsometric(Vector2 point)
        {
            return new Vector2(
                point.X * cosAngle - point.Y * sinAngle,
                point.X * sinAngle + point.Y * cosAngle
            );
        }
    }
}
