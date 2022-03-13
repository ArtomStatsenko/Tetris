using UnityEngine;
using System;
using Object = UnityEngine.Object;

public sealed class BoardController
{
    public event Action<int> OnScoreChangedEvent;

    private Transform[,] _grid;

    public BoardController(Transform[,] grid)
    {
        _grid = grid;
    }

    public void ClearFullLines(GameMode mode)
    {
        int deletedLines = 0;

        if (mode == GameMode.First)
        {
            FindSingleFullLine(ref deletedLines);
        }
        else if (mode == GameMode.Second)
        {
            FindDoubleFullLines(ref deletedLines);
        }

        if (deletedLines > 0)
        {
            OnScoreChangedEvent?.Invoke(deletedLines);
        }
    }

    private void FindSingleFullLine(ref int deletedLines)
    {
        for (int row = _grid.GetUpperBound(1); row >= 0; row--)
        {
            if (IsLineFull(row))
            {
                DeleteLine(row);
                MoveLinesDown(row);
                deletedLines++;
            }
        }
    }

    private void FindDoubleFullLines(ref int deletedLines)
    {
        for (int row = _grid.GetUpperBound(1) - 1; row >= 0; row--)
        {
            if (IsLineFull(row) && IsLineFull(row + 1))
            {
                DeleteLine(row + 1);
                MoveLinesDown(row + 1);
                deletedLines++;

                DeleteLine(row);
                MoveLinesDown(row);
                deletedLines++;
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