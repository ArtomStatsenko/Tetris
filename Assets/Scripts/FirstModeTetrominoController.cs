using UnityEngine;

public sealed class FirstModeTetrominoController : TetrominoController
{
    private Transform _transform;
    private RectInt _bounds;

    public FirstModeTetrominoController(Transform transform, RectInt bounds, Transform[,] grid) : base(transform, bounds, grid)
    {
        _transform = transform;
        _bounds = bounds;
    }

    public override bool Move(Vector3Int direction)
    {
        _transform.position += direction;
        if (!IsValidPosition(_transform))
        {
            _transform.position -= direction;
            return false;
        }

        return true;
    }

    public override void Rotate(Vector3Int eulerAngles)
    {
        _transform.Rotate(eulerAngles);
        if (!IsValidPosition(_transform))
        {
            _transform.Rotate(-eulerAngles);
        }
    }

    public override bool IsPositionFree(int x, int y, bool isBlockOnBoard, Transform[,] grid)
    {
        int indexX = x - _bounds.min.x;
        int indexY = y - _bounds.min.y;

        if (!isBlockOnBoard || grid[indexX, indexY] != null)
        {
            return false;
        }

        return true;
    }

    public override void AddToGrid(Transform[,] grid)
    {
        foreach (Transform block in _transform)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);
            Vector2Int position = new Vector2Int(roundedX, roundedY);

            if (_bounds.Contains(position))
            {
                int gridIndexX = roundedX - _bounds.min.x;
                int gridIndexY = roundedY - _bounds.min.y;

                grid[gridIndexX, gridIndexY] = block;
            }
        }

        _transform.DetachChildren();
        Object.Destroy(_transform.gameObject);
    }
}
