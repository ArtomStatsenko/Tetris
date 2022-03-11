using System;
using UnityEngine;

public sealed class TetrominoController
{
    public event Action OnLandedEvent; 
    public event Action OnGameOverEvent;

    private float _nextDropTime;
    private Transform _transform;
    private RectInt _bounds;
    private Transform[,] _grid;
    private bool _isMoveble;
    private float _dropTimeDelay;

    public TetrominoController(Transform transform, RectInt bounds, Transform[,] grid, float dropTimeDelay)
    {
        _transform = transform;
        _bounds = bounds;
        _grid = grid;
        _dropTimeDelay = dropTimeDelay;
    }

    public void Start()
    {
        _nextDropTime = Time.time;
        _isMoveble = true;

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
            Rotate(new Vector3(0f, 0f, 90f));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(new Vector3(0f, 0f, -90f));
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

        return true;
    }

    private void Rotate(Vector3 eulerAngles)
    {
        _transform.Rotate(eulerAngles);
        if (!IsValidPosition())
        {
            _transform.Rotate(-eulerAngles);
        }
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
        }

        _transform.DetachChildren();
        UnityEngine.Object.Destroy(_transform.gameObject);
    }

    private void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
    }       
}
