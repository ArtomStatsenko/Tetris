using UnityEngine;

public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    public static RectInt Bounds
    {
        get
        {
            Vector2Int minPosition = new Vector2Int(-Width / 2, -Height / 2);
            Vector2Int size = new Vector2Int(Width, Height);
            return new RectInt(minPosition, size);
        }
    }
}
