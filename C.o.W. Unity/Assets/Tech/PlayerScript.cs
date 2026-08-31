using UnityEngine;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] int movementDistance;
    [SerializeField] TileScript _startingTile;
    [SerializeField] TileScript _currentTile;
    [SerializeField] TileScript _lastTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = new Vector3(_startingTile.tilePos.x, this.transform.position.y, _startingTile.tilePos.y);
    }
}
