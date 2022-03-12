using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class FirstModeTetrominoController : ITetrominoController
{
    public event Action OnLandedEvent;
    public event Action OnGameOverEvent;

    private Transform[,] _grid;
    private Transform _transform;
    private RectInt _bounds;
    private float _nextDropTime;
    private float _dropTimeDelay;
    private bool _isMoveble;

    public FirstModeTetrominoController(Transform transform, RectInt bounds, Transform[,] grid)
    {
        _transform = transform;
        _bounds = bounds;
        _grid = grid;
    }

    public void Start(float dropTimeDelay)
    {
        _nextDropTime = Time.time;
        _isMoveble = true;
        _dropTimeDelay = dropTimeDelay;
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

    public bool Move(Vector3Int direction)
    {
        _transform.position += direction;
        if (!IsValidPosition())
        {
            _transform.position -= direction;
            return false;
        }

        return true;
    }

    public void Rotate(Vector3Int eulerAngles)
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
        Object.Destroy(_transform.gameObject);
    }

    private void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
    }
}
