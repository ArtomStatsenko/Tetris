using UnityEngine;

public sealed class GhostTetrominoController
{
    private Transform _transform;
    private RectInt _bounds;
    private Transform[,] _grid;
    private Transform _parentTransform;

    public GhostTetrominoController(Transform transform, RectInt bounds, Transform[,] grid, Transform parentTransform)
    {
        _transform = transform;
        _bounds = bounds;
        _grid = grid;
        _parentTransform = parentTransform;
    }

    public void LateUpdate()
    {
        _transform.position = _parentTransform.position;
        _transform.rotation = _parentTransform.rotation;
        HardDrop();
    }

    private void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
    }

    private bool Move(Vector3Int direction)
    {
        _transform.position += direction;
        if (!IsValidPosition())
        {
            _transform.position -= direction;
            return false;
        }

        return true;
    }

    private bool IsValidPosition()
    {
        foreach (Transform block in _transform)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);
            Vector2Int position = new Vector2Int(roundedX, roundedY);

            if (!_bounds.Contains(position))
            {
                return false;
            }

            int gridIndexX = roundedX - _bounds.min.x;
            int gridIndexY = roundedY - _bounds.min.y;

            if (_grid[gridIndexX, gridIndexY] != null)
            {
                return false;
            }
        }

        return true;
    }
}