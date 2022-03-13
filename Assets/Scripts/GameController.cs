using UnityEngine;
using UnityEngine.UI;

public sealed class GameController : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private SpriteRenderer _boardSpriteRenderer;
    [SerializeField] private Text _scoreLabel;
    [SerializeField] [Range(0.1f, 1f)] private float _dropTimeDelay;

    private Transform[,] _grid;
    private BoardController _boardController;
    private SpawnController _spawnController;
    private TetrominoController _tetrominoController;
    private int _score;

    private void Start()
    {
        Board board = _gameData.Board;
        _grid = new Transform[board.Width, board.Height];
        _boardSpriteRenderer.size = new Vector2(board.Width, board.Height);

        _boardController = new BoardController(_grid);
        _boardController.OnScoreChangedEvent += ChangeScore;

        _spawnController = new SpawnController(_gameData, board.SpawnPosition, _grid);

        StartNewTetromino();
    }


    private void Update()
    {
        _tetrominoController.Update();        
    }

    private void NextStep()
    {
        _tetrominoController.AddToGrid(_grid);
        _boardController.ClearFullLines(_gameData.Mode);
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
        _score = 0;
        UpdateScoreLabel();
    }

    private void ChangeScore(int score)
    {
        _score += score;
        UpdateScoreLabel();
    }

    private void UpdateScoreLabel()
    {
        _scoreLabel.text = $"Score: {_score}";
    }
}