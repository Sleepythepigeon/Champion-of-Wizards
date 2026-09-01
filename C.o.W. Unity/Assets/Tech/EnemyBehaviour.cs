using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private PlayerScript _player;
    [SerializeField] public int moveRange;
    [SerializeField] TileScript _startingTile;
    [SerializeField] public TileScript _currentTile;
    [SerializeField] public TileScript _lastTile;

    public TileScript _targetTile;
    public TileScript _closestTile;

    public List<TileScript> _tilesInRange = new List<TileScript>();

    private void Awake()
    {
        _player = FindAnyObjectByType<PlayerScript>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = new Vector3(_startingTile.tilePos.x, this.transform.position.y, _startingTile.tilePos.y);
        _currentTile = _startingTile;
        _lastTile = _currentTile;
        _startingTile.hasEnemy = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyTurn()
    {
        ResetEnemy();
        //moves enemy to closest accessable tile
        GetTilesInRange(moveRange);
        FindClosestTileToPlayer();
        MoveEnemy();
        ResetEnemy();

        GetTilesInRange(moveRange);
    }

    private void GetTilesInRange(int range)
    {
        this._tilesInRange.Add(_currentTile);

        for (int enemyRange = range; enemyRange > 0; enemyRange--)
        {
            int amountTilesInRange = _tilesInRange.Count;

            for (int i = 0; i < amountTilesInRange; i++)
            {
                foreach (TileScript neigh in _tilesInRange[i]._neighbors)
                {
                    bool isInList = false;
                    for (int p = 0; p < _tilesInRange.Count; p++)
                    {
                        if (_tilesInRange[p].tilePos == neigh.tilePos)
                        {
                            isInList = true;
                        }
                    }

                    if (!isInList && !neigh.isWall)
                    {
                        _tilesInRange.Add(neigh);
                    }

                }
            }

        }
    }

    private void FindClosestTileToPlayer()
    {
        _targetTile = _player._currentTile;
        _closestTile = _currentTile;

        foreach(TileScript tile in _tilesInRange)
        {
            if(Vector2.Distance(_closestTile.tilePos, _targetTile.tilePos) > Vector2.Distance(tile.tilePos, _targetTile.tilePos))
            {
                _closestTile = tile;
            }
        }
    }

    private void MoveEnemy()
    {
        _currentTile.hasEnemy = false;
        this.transform.position = new Vector3(_closestTile.tilePos.x, this.transform.position.y, _closestTile.tilePos.y);
        _lastTile = _currentTile;
        _currentTile = _closestTile;
        _currentTile.hasEnemy = true;
    }

    private void ResetEnemy()
    {
        int size = _tilesInRange.Count - 1;
        for (int i = size; i >= 0; i--)
        {
            _tilesInRange.RemoveAt(i);
        }
    }
}
