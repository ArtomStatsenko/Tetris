using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class SecondModeTetrominoController
{
    public event Action OnLandedEvent;
    public event Action OnGameOverEvent;

    private Transform[,] _grid;
    private Transform _transform;
    private RectInt _bounds;
    private float _nextDropTime;
    private float _dropTimeDelay;
    private float _horizontalOffset;
    private GameMode _mode;
    private bool _isMoveble;

    public SecondModeTetrominoController(Transform transform, RectInt bounds, Transform[,] grid, float dropTimeDelay, GameMode mode)
    {
        _transform = transform;
        _bounds = bounds;
        _grid = grid;
        _dropTimeDelay = dropTimeDelay;
        _mode = mode;
    }

    public void Start(float startPositionX)
    {
        _nextDropTime = Time.time;
        _isMoveble = true;
        _horizontalOffset = _bounds.size.x - startPositionX - 1;
        OnLandedEvent += () => _isMoveble = false;

        if (!IsValidPosition())
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

    private bool Move(Vector3Int direction)
    {
        _transform.position += direction;
        if (!IsValidPosition())
        {
            _transform.position -= direction;
            return false;
        }

        if (_mode == GameMode.Second)
        {
            SetBlocksEnable();
            TranslateIfPositionFar();
        }

        return true;
    }

    private void Rotate(Vector3Int eulerAngles)
    {
        _transform.Rotate(eulerAngles);
        if (!IsValidPosition())
        {
            _transform.Rotate(-eulerAngles);
        }

        if (_mode == GameMode.Second)
        {
            SetBlocksEnable();
        }
    }

    private bool IsValidPosition()
    {
        foreach (Transform block in _transform)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);
            Vector2Int position = new Vector2Int(roundedX, roundedY);

            int gridIndexX = roundedX - _bounds.min.x;
            int gridIndexY = roundedY - _bounds.min.y;

            bool isBlockOnBoard = _bounds.Contains(position);

            if (_mode == GameMode.First)
            {
                if (!isBlockOnBoard || _grid[gridIndexX, gridIndexY] != null)
                {
                    return false;
                }
            }
            else if (_mode == GameMode.Second)
            {
                if (roundedY < _bounds.yMin)
                {
                    return false;
                }

                if (isBlockOnBoard && _grid[gridIndexX, gridIndexY] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void AddToGrid(Transform[,] grid)
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
            else
            {
                Object.Destroy(block.gameObject);
            }
        }

        _transform.DetachChildren();
        Object.Destroy(_transform.gameObject);
    }

    private void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
    }

    private void SetBlocksEnable()
    {
        foreach (Transform block in _transform)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);
            Vector2Int position = new Vector2Int(roundedX, roundedY);
            bool isBlockOnBoard = _bounds.Contains(position);
            block.gameObject.SetActive(isBlockOnBoard);
        }
    }

    private void TranslateIfPositionFar()
    {
        if (_transform.position.x < -_bounds.size.x)
        {
            _transform.position = _transform.position.Change(x: _horizontalOffset);
        }
        else if (_transform.position.x > _bounds.size.x)
        {
            _transform.position = _transform.position.Change(x: -_horizontalOffset);
        }
    }
}