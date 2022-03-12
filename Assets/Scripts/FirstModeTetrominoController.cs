using UnityEngine;

public sealed class FirstModeTetrominoController : TetrominoController
{
    private Transform[,] _grid;
    private Transform _transform;
    private RectInt _bounds;

    public FirstModeTetrominoController(Transform transform, RectInt bounds, Transform[,] grid) : base(transform)
    {
        _transform = transform;
        _bounds = bounds;
        _grid = grid;
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

    public override bool IsValidPosition(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);
            Vector2Int position = new Vector2Int(roundedX, roundedY);
            int gridIndexX = roundedX - _bounds.min.x;
            int gridIndexY = roundedY - _bounds.min.y;
            bool isBlockOnBoard = _bounds.Contains(position);

            if (!isBlockOnBoard || _grid[gridIndexX, gridIndexY] != null)
            {
                return false;
            }
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
