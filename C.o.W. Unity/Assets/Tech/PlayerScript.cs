using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public int moveRange;
    [SerializeField] TileScript _startingTile;
    [SerializeField] public TileScript _currentTile;
    [SerializeField] public TileScript _lastTile;

    public TileScript _clickedTile;

    public MeshRenderer meshRenderer;

    public List<TileScript> _tilesInRange = new List<TileScript>();
    public List<TileScript> _allTiles = new List<TileScript>();

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        TileScript[] tempAllTiles = FindObjectsByType<TileScript>(FindObjectsSortMode.None);
        foreach (TileScript tile in tempAllTiles)
        {
            _allTiles.Add(tile);
        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = new Vector3(_startingTile.tilePos.x, this.transform.position.y, _startingTile.tilePos.y);
        _currentTile = _startingTile;
        _lastTile = _currentTile;
        _startingTile.hasPlayer = true;
    }
}
