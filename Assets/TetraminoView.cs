using UnityEngine;

public class TetraminoView : MonoBehaviour
{
    [SerializeField] private float _dropTimeDelay = 1f;
    [SerializeField] private Transform _rig;

    private int _boardWidth = 10;
    private int _boardHeight = 20;
    private float _nextDropTime;
    private BoundsInt _bounds;

    private void Start()
    {
        Vector3Int minPosition = new Vector3Int(-_boardWidth / 2, -_boardHeight / 2, 0);
        Vector3Int size = new Vector3Int(_boardWidth, _boardHeight, 0);
        _bounds = new BoundsInt(minPosition, size);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector3Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector3Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector3Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _rig.Rotate(new Vector3(0f, 0f, 90f));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _rig.Rotate(new Vector3(0f, 0f, -90f));
        }


        if (Time.time > _nextDropTime)
        {
            Move(Vector3Int.down);
            _nextDropTime += _dropTimeDelay;
        }
    }

    private void Move(Vector3Int direction)
    {
        transform.position += direction;
        if (!IsMovingValid())
        {
            transform.position -= direction;
        }
    }

    private bool IsMovingValid()
    {
        foreach (Transform block in transform)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);

            if (roundedX < _bounds.xMin || roundedX >= _bounds.xMax || roundedY < _bounds.yMin || roundedY >= _bounds.yMax)
            {
                return false;
            }
        }

        return true;
    }
}
