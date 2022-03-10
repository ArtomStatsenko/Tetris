using UnityEngine;

public sealed class GameController : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private float _dropTimeDelay = 1f;

    private TetrominoController _tetrominoController;
    private TetrominoData[] _tetrominoes;
    private Board _board;
    private float _OIpositionOffset = 0.5f;

    private void Start()
    {
        _tetrominoes = _gameData.Tetrominoes;
        _board = _gameData.Board;

        SpawnTetramino();
    }

    private void Update()
    {
        _tetrominoController.Update();
    }

    private void SpawnTetramino()
    {
        int index = Random.Range(0, _tetrominoes.Length);
        TetrominoData data = _tetrominoes[index];

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

        TetrominoView tetromino = Instantiate(data.Prefab, position, Quaternion.identity);

        _tetrominoController = new TetrominoController(tetromino.transform, _board.Bounds);
        _tetrominoController.DropTimeDelay = _dropTimeDelay;
        _tetrominoController.OnLandedEvent += SpawnTetramino;
    }
}