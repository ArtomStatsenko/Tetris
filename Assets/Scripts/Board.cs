using UnityEngine;

[System.Serializable]
public sealed class Board
{
    public int Width = 10;
    public int Height = 20;

    public RectInt Bounds
    {
        get
        {
            Vector2Int minPosition = new Vector2Int(-Width / 2, -Height / 2);
            Vector2Int size = new Vector2Int(Width, Height);
            return new RectInt(minPosition, size);
        }
    }
}
