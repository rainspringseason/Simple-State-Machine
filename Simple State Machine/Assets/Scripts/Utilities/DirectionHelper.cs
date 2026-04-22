using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionHelper
{
    public static Direction Vector2ToDirection(Vector2 vector)
    {
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
        {
            return vector.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return vector.y > 0 ? Direction.Up : Direction.Down;
        }
    }

    public static Vector2 DirectionToVector2(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return Vector2.up;
            case Direction.Down: return Vector2.down;
            case Direction.Left: return Vector2.left;
            case Direction.Right: return Vector2.right;
            default: return Vector2.down;
        }
    }

    public static string DirectionToString(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return "Up";
            case Direction.Down: return "Down";
            case Direction.Left: return "Left";
            case Direction.Right: return "Right";
            default: return "Down";
        }
    }
}
