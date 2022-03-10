using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
public sealed class GameData : ScriptableObject
{
    [SerializeField] private GameMode _mode;
    [SerializeField] private Board _board;
    [SerializeField] private TetrominoData[] _tetrominoes;

    public GameMode Mode => _mode;
    public Board Board => _board;
    public TetrominoData[] Tetrominoes => _tetrominoes;
}
