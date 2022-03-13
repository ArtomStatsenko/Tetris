using System;
using UnityEngine;

public abstract class TetrominoController
{
    public event Action OnLandedEvent;
    public event Action OnGameOverEvent;

    private Transform[,] _grid;
    private Transform _tetromino;
    private RectInt _bounds;
    private float _nextDropTime;
    private float _dropTimeDelay;
    private bool _isMoveble;

    public TetrominoController(Transform tetromino, RectInt bounds, Transform[,] grid)
    {
        _tetromino = tetromino;
        _bounds = bounds;
        _grid = grid;
    }

    public void Start(float dropTimeDelay)
    {
        _nextDropTime = Time.time;
        _isMoveble = true;
        _dropTimeDelay = dropTimeDelay;
        OnLandedEvent += () => _isMoveble = false;

        if (!IsValidPosition(_tetromino))
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

    public bool IsValidPosition(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);
            Vector2Int position = new Vector2Int(roundedX, roundedY);
            bool isBlockOnBoard = _bounds.Contains(position);

            if (!IsPositionFree(roundedX, roundedY, isBlockOnBoard, _grid))
            {
                return false;
            }
        }

        return true;
    }

    public void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
    }

    public abstract bool Move(Vector3Int direction);

    public abstract void Rotate(Vector3Int eulerAngles);

    public abstract bool IsPositionFree(int x, int y, bool isBlockOnBoard, Transform[,] grid);

    public abstract void AddToGrid(Transform[,] grid);
}
