using System;
using UnityEngine;

public interface ITetrominoController
{
    event Action OnLandedEvent;
    event Action OnGameOverEvent;

    void Start(float dropTimeDelay);

    void Update();

    void AddToGrid(Transform[,] grid);

    bool Move(Vector3Int direction);

    void Rotate(Vector3Int direction);
}