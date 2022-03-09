using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameController : MonoBehaviour
{
    [SerializeField] private Tetromino[] _tetraminoes;
    [SerializeField] private Vector3 _spawnPosition;

    private void Start()
    {
        SpawnTetramino();
    }
     
    private void SpawnTetramino()
    {
        int index = Random.Range(0, _tetraminoes.Length - 3); // first mode

        Tetromino tetramino = Instantiate(_tetraminoes[index]);
        tetramino.transform.position = _spawnPosition;
    }
}