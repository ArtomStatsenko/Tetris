using UnityEngine;

public sealed class GameController : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private SpriteRenderer _boardSpriteRenderer;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private float _dropTimeDelay = 1f;

    private Transform[,] _grid;
    private BoardController _boardController;
    private SpawnController _spawnController;
    private TetrominoController _tetrominoController;

    private void Start()
    {
        Board board = _gameData.Board;
        _grid = new Transform[board.Width, board.Height];
        _boardSpriteRenderer.size = new Vector2(board.Width, board.Height);
        _boardController = new BoardController(_grid);
        _spawnController = new SpawnController(_gameData, _spawnPosition, _grid);

        StartNewTetromino();
    }


    private void Update()
    {
        _tetrominoController.Update();        
    }

    private void NextStep()
    {
        _tetrominoController.AddToGrid(_grid);
        _boardController.ClearFullLines();
        StartNewTetromino();
    }

    private void StartNewTetromino()
    {
        _tetrominoController = _spawnController.CreateTetromino();
        _tetrominoController.OnLandedEvent += NextStep;
        _tetrominoController.OnGameOverEvent += GameOver;
        _tetrominoController.Start(_dropTimeDelay);
    }

    private void GameOver()
    {
        _boardController.Clear();
    }
}