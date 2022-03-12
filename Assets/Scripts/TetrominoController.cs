using System;
using UnityEngine;

public abstract class TetrominoController
{ 
    public event Action OnLandedEvent;
    public event Action OnGameOverEvent;

    private Transform _tetromino;
    private float _nextDropTime;
    private float _dropTimeDelay;
    private bool _isMoveble;

    public TetrominoController(Transform tetromino)
    {
        _tetromino = tetromino;
    }

    public virtual void Start(float dropTimeDelay)
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

    public abstract bool Move(Vector3Int direction);

    public abstract void Rotate(Vector3Int eulerAngles);

    public abstract bool IsValidPosition(Transform tetromino);

    public abstract void AddToGrid(Transform[,] grid);

    public void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
    }
}
