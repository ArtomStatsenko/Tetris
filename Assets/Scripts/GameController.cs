using UnityEngine;

public sealed class GameController : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private SpriteRenderer _boardSpriteRenderer;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private float _dropTimeDelay = 1f;

    private Transform[,] _grid;
    private TetrominoController _tetrominoController;
    private TetrominoController _cloneController;
    private TetrominoDropChance[] _tetrominoes;
    private Board _board;
    private int[] _dropChances;
    private float _OIpositionOffset = 0.5f;

    private void Start()
    {
        _tetrominoes = _gameData.Tetrominoes;
        _board = _gameData.Board;
        _grid = new Transform[_board.Width, _board.Height];
        _dropChances = _gameData.DropChances;
        _boardSpriteRenderer.size = new Vector2(_board.Width, _board.Height);

        CreateTetromino();
    }

    private void Update()
    {
        _tetrominoController.Update();

        if (_gameData.Mode == GameMode.Second)
        {
            _cloneController.Update();
        }
    }

    private void CreateTetromino()
    {
        TetrominoData data = GetTetrominoData();
        TetrominoView tetromino = NewTetromino(data);
        _tetrominoController = new TetrominoController(tetromino.transform, _board.Bounds, _grid, _dropTimeDelay, _gameData.Mode);
        _tetrominoController.OnLandedEvent += NextStep;
        _tetrominoController.OnGameOverEvent += GameOver;
        _tetrominoController.Start(tetromino.transform.position.x);

        if (_gameData.Mode == GameMode.Second)
        {
            CreateClone(data);
        }
    }

    private void CreateClone(TetrominoData data)
    {
        TetrominoView tetromino = NewTetromino(data);
        float startPositionX = tetromino.transform.position.x;
        tetromino.transform.position = tetromino.transform.position.Change(x: startPositionX + _board.Width);
        _cloneController = new TetrominoController(tetromino.transform, _board.Bounds, _grid, _dropTimeDelay, _gameData.Mode);
        _cloneController.Start(startPositionX);
    }

    private TetrominoData GetTetrominoData()
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

    private TetrominoView NewTetromino(TetrominoData data)
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

        return Instantiate(data.Prefab, position, Quaternion.identity);
    }

    private void NextStep()
    {
        _tetrominoController.AddToGrid(_grid);

        if (_gameData.Mode == GameMode.Second)
        {
            _cloneController.AddToGrid(_grid);
        }

        ClearLines();
        CreateTetromino();
    }

    private void ClearLines()
    {
        for (int row = _grid.GetUpperBound(1); row >= 0 ; row--)
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

    private void DeleteLine(int row)
    {
        for (int column = 0; column <= _grid.GetUpperBound(0); column++)
        {
            if (_grid[column, row] != null)
            {
                Destroy(_grid[column, row].gameObject);
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

    private void GameOver()
    {
        Clear();
    }

    private void Clear()
    {
        for (int row = 0; row <= _grid.GetUpperBound(1); row++)
        {
            DeleteLine(row);
        }
    }
}