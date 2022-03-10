using System;
using UnityEngine;

public sealed class TetrominoController
{
    public event Action OnLandedEvent;

    private float _nextDropTime;
    private Transform _transform;
    private RectInt _bounds;

    public bool IsMoveble { get; private set; } = true;
    public float DropTimeDelay { get; set; } = 1f;

    public TetrominoController(Transform transform, RectInt bounds)
    {
        _transform = transform;
        _bounds = bounds;
        _nextDropTime = Time.time;
        OnLandedEvent += () => IsMoveble = false;
    }

    public void Update()
    {
        if (!IsMoveble)
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
                _nextDropTime += DropTimeDelay;
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
        }

        return true;
    }

    private void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
    }       
}
