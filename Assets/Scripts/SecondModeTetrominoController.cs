using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class SecondModeTetrominoController : ITetrominoController
{
    public event Action OnLandedEvent;
    public event Action OnGameOverEvent;

    private Transform[,] _grid;
    private Transform _first;
    private Transform _second;
    private RectInt _bounds;
    private float _nextDropTime;
    private float _dropTimeDelay;
    private float _horizontalOffset;
    private bool _isMoveble;

    public SecondModeTetrominoController(Transform first, Transform second, RectInt bounds, Transform[,] grid)
    {
        _first = first;
        _second = second;
        _bounds = bounds;
        _grid = grid;
        _horizontalOffset = _bounds.size.x - first.position.x - 1;
    }

    public void Start(float dropTimeDelay)
    {
        _nextDropTime = Time.time;
        _isMoveble = true; 
        _dropTimeDelay = dropTimeDelay;
        OnLandedEvent += () => _isMoveble = false;

        if (!IsValidPosition(_first))
        {
            OnGameOverEvent?.Invoke();
        }
    }

    public void Update()
    {
        if (!_isMoveble)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector3Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector3Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector3Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(new Vector3Int(0, 0, 90));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(new Vector3Int(0, 0, -90));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        if (Time.time > _nextDropTime)
        {
            if (Move(Vector3Int.down))
            {
                _nextDropTime += _dropTimeDelay;
            }
            else
            {
                OnLandedEvent?.Invoke();
            }
        }
    }

    public bool Move(Vector3Int direction)
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

    public void Rotate(Vector3Int eulerAngles)
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

    private bool IsValidPosition(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);
            Vector2Int position = new Vector2Int(roundedX, roundedY);
            int gridIndexX = roundedX - _bounds.min.x;
            int gridIndexY = roundedY - _bounds.min.y;
            bool isBlockOnBoard = _bounds.Contains(position);

            if (roundedY < _bounds.yMin)
            {
                return false;
            }

            if (isBlockOnBoard && _grid[gridIndexX, gridIndexY] != null)
            {
                return false;
            }
        }

        return true;
    }

    public void AddToGrid(Transform[,] grid)
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

    private void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
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