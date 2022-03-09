using System;
using UnityEngine;

public sealed class Tetromino : MonoBehaviour
{
    public event Action OnLandedEvent;

    [SerializeField] private float _dropTimeDelay = 1f;

    private float _nextDropTime;

    public bool IsMoveble { get; private set; } = true;

    private void Awake()
    {
        OnLandedEvent += () => IsMoveble = false;
    }

    private void Update()
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
        transform.position += direction;
        if (!IsValidPosition())
        {
            transform.position -= direction;
            return false;
        }

        return true;
    }

    private void Rotate(Vector3 eulerAngles)
    {
        transform.Rotate(eulerAngles);
        if (!IsValidPosition())
        {
            transform.Rotate(-eulerAngles);
        }
    }

    private bool IsValidPosition()
    {
        foreach (Transform block in transform)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);

            if (roundedX < Board.Bounds.xMin || roundedX >= Board.Bounds.xMax || roundedY < Board.Bounds.yMin)
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
