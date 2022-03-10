using UnityEngine;

[CreateAssetMenu(fileName = "Tetromino", menuName = "Data/Tetromino", order = 1)]
public sealed class TetrominoData : ScriptableObject
{
    [SerializeField] private TetrominoType _type;
    [SerializeField] private int _chance;
    [SerializeField] private TetrominoController _prefab;

    public TetrominoType Type => _type;
    public int Chance => _chance;
    public TetrominoController Prefab => _prefab;
}
