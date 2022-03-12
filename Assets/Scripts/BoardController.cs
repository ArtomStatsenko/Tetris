using UnityEngine;

public sealed class BoardController
{
    private Transform[,] _grid;

    public Transform[,] Grid
    {
        get => _grid;
        set => _grid = value;
    }

    public BoardController(Transform[,] grid)
    {
        _grid = grid;
    }

    public void ClearFullLines()
    {
        for (int row = _grid.GetUpperBound(1); row >= 0; row--)
        {
            if (IsLineFull(row))
            {
                DeleteLine(row);
                MoveLinesDown(row);
            }
        }
    }

    private bool IsLineFull(int row)
    {
        for (int column = 0; column <= _grid.GetUpperBound(0); column++)
        {
            if (_grid[column, row] == null)
            {
                return false;
            }
        }

        return true;
    }

    public void DeleteLine(int row)
    {
        for (int column = 0; column <= _grid.GetUpperBound(0); column++)
        {
            if (_grid[column, row] != null)
            {
                Object.Destroy(_grid[column, row].gameObject);
                _grid[column, row] = null;
            }
        }
    }

    private void MoveLinesDown(int startRow)
    {
        for (int row = startRow; row <= _grid.GetUpperBound(1); row++)
        {
            for (int column = 0; column <= _grid.GetUpperBound(0); column++)
            {
                if (_grid[column, row] != null)
                {
                    _grid[column, row - 1] = _grid[column, row];
                    _grid[column, row] = null;
                    _grid[column, row - 1].position += Vector3Int.down;
                }
            }
        }
    }

    public void Clear()
    {
        for (int row = 0; row <= _grid.GetUpperBound(1); row++)
        {
            DeleteLine(row);
        }
    }
}