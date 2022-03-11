using UnityEngine;

[CreateAssetMenu(fileName = "Tetromino", menuName = "Data/Tetromino", order = 1)]
public sealed class TetrominoData : ScriptableObject
{
    [SerializeField] private TetrominoType _type;
    [SerializeField] private TetrominoView _prefab;

    public TetrominoType Type => _type;
    public TetrominoView Prefab => _prefab;
}
