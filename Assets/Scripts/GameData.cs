using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
public sealed class GameData : ScriptableObject
{
    [SerializeField] private GameMode _mode;
    [SerializeField] private Board _board;
    [SerializeField] private TetrominoDropChance[] _tetrominoes;

    public GameMode Mode => _mode;
    public Board Board => _board;
    public TetrominoDropChance[] Tetrominoes => _tetrominoes;
    public int[] DropChances
    {
        get
        {
            int[] chances = new int[_tetrominoes.Length];
            int currentChance = 0;
            for (int i = 0; i < chances.Length; i++)
            {
                currentChance += _tetrominoes[i].DropChance;
                chances[i] = currentChance;
            }
            return chances;
        }
    }
}
