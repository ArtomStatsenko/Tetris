using UnityEngine;

public sealed class SpawnController
{
    private Transform[,] _grid;
    private Vector3 _spawnPosition;
    private TetrominoDropChance[] _tetrominoes;
    private Board _board;
    private int[] _dropChances;
    private float _OIpositionOffset = 0.5f;
    private GameMode _mode;

    public SpawnController(GameData data, Vector3 spawnPosition, Transform[,] grid)
    {
        _mode = data.Mode; 
        _dropChances = data.DropChances;
        _tetrominoes = data.Tetrominoes;
        _board = data.Board;
        _spawnPosition = spawnPosition;
        _grid = grid;
    }

    public TetrominoController CreateTetromino()
    {
        TetrominoData data = GetTetrominoData();

        if (_mode == GameMode.First)
        {
            TetrominoView tetromino = NewTetromino(data);
            return new FirstModeTetrominoController(tetromino.transform, _board.Bounds, _grid);
        }
        else
        {
            TetrominoView first = NewTetromino(data);
            TetrominoView second = NewTetromino(data);
            second.transform.position += new Vector3(_board.Width, 0f, 0f);
            return new SecondModeTetrominoController(first.transform, second.transform, _board.Bounds, _grid);
        }
    }

    public TetrominoData GetTetrominoData()
    {
        int value = Random.Range(0, _dropChances.LastValue()) + 1;
        for (int i = 0; i < _dropChances.Length; i++)
        {
            if (value <= _dropChances[i])
            {
                return _tetrominoes[i].Data;
            }
        }

        return null;
    }

    public TetrominoView NewTetromino(TetrominoData data)
    {
        Vector3 position;
        switch (data.Type)
        {
            case TetrominoType.O:
            case TetrominoType.I:
                position = new Vector3(_spawnPosition.x - _OIpositionOffset, _spawnPosition.y + _OIpositionOffset, 0f);
                break;
            default:
                position = _spawnPosition;
                break;
        }

        return Object.Instantiate(data.Prefab, position, Quaternion.identity);
    }
}