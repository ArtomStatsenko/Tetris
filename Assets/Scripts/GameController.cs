using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameController : MonoBehaviour
{
    [SerializeField] private TetrominoData[] _tetraminoes;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private float _dropTimeDelay = 1f;

    private void Start()
    {
        SpawnTetramino();
    }
     
    private void SpawnTetramino()
    {
        int index = Random.Range(0, _tetraminoes.Length - 3); // first mode
        TetrominoData data = _tetraminoes[index];

        TetrominoController tetramino = Instantiate(data.Prefab);
        tetramino.DropTimeDelay = _dropTimeDelay;
        tetramino.transform.position = _spawnPosition;
    }
}