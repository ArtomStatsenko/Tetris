using UnityEngine;

public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    public static BoundsInt Bounds
    {
        get
        {
            Vector3Int minPosition = new Vector3Int(-Width / 2, -Height / 2, 0);
            Vector3Int size = new Vector3Int(Width, Height, 0);            
            return new BoundsInt(minPosition, size);
        }
    }
}
