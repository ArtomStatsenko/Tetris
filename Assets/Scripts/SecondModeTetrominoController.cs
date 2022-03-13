using UnityEngine;

public sealed class SecondModeTetrominoController : TetrominoController
{
    private Transform _first;
    private Transform _second;
    private RectInt _bounds;
    private float _horizontalOffset;

    public SecondModeTetrominoController(Transform first, Transform second, RectInt bounds, Transform[,] grid) : base(first, bounds, grid)
    {
        _first = first;
        _second = second;
        _bounds = bounds;
        _horizontalOffset = _bounds.size.x - first.position.x - 1;
    }

    public override bool Move(Vector3Int direction)
    {
        _first.position += direction;
        _second.position += direction;

        if (!IsValidPosition(_first) || !IsValidPosition(_second))
        {
            _first.position -= direction; 
            _second.position -= direction;
            return false;
        }

        SetBlocksEnable(_first);
        SetBlocksEnable(_second);
        TranslateIfPositionFar(_first);
        TranslateIfPositionFar(_second);

        return true;
    }

    public override void Rotate(Vector3Int eulerAngles)
    {
        _first.Rotate(eulerAngles); 
        _second.Rotate(eulerAngles);
        if (!IsValidPosition(_first) || !IsValidPosition(_second))
        {
            _first.Rotate(-eulerAngles);
            _second.Rotate(-eulerAngles);
        }

        SetBlocksEnable(_first);
        SetBlocksEnable(_second);
    }

    public override bool IsPositionFree(int x, int y, bool isBlockOnBoard, Transform[,] grid)
    {
        int indexX = x - _bounds.min.x;
        int indexY = y - _bounds.min.y;

        if (y < _bounds.yMin)
        {
            return false;
        }

        if (isBlockOnBoard && grid[indexX, indexY] != null)
        {
            return false;
        }

        return true;
    }

    public override void AddToGrid(Transform[,] grid)
    {
        AddBlocksToGrid(_first, grid);
        AddBlocksToGrid(_second, grid);
    }

    private void AddBlocksToGrid(Transform tetromino , Transform[,] grid)
    {
        foreach (Transform block in tetromino)
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
            else
            {
                Object.Destroy(block.gameObject);
            }
        }

        tetromino.DetachChildren();
        Object.Destroy(tetromino.gameObject);
    }

    private void SetBlocksEnable(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);
            Vector2Int position = new Vector2Int(roundedX, roundedY);
            bool isBlockOnBoard = _bounds.Contains(position);
            block.gameObject.SetActive(isBlockOnBoard);
        }
    }

    private void TranslateIfPositionFar(Transform tetromino)
    {
        if (tetromino.position.x < -_bounds.size.x)
        {
            tetromino.position = tetromino.position.Change(x: _horizontalOffset);
        }
        else if (tetromino.position.x > _bounds.size.x)
        {
            tetromino.position = tetromino.position.Change(x: -_horizontalOffset);
        }
    }
}