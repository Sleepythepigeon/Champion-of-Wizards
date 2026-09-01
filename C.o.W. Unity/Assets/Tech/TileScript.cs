using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject _objectPlayer;
    public PlayerScript _player;
    public HoldingScript _holdingScript;
    private EnemyBehaviour _enemy;

    public List<TileScript> _neighbors = new List<TileScript>();
    public bool isWall;
    public bool hasPlayer;
    public bool hasEnemy;

    public Vector2 tilePos;
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        tilePos = new Vector2(this.transform.position.x, this.transform.position.z);
        meshRenderer = GetComponent<MeshRenderer>();
        _objectPlayer = GameObject.Find("Player");
        _player = FindFirstObjectByType<PlayerScript>();
        _holdingScript = FindFirstObjectByType<HoldingScript>();
        _enemy = FindFirstObjectByType<EnemyBehaviour>();

        if(isWall)
        {
            meshRenderer.material.color = Color.black;
        }
    }

    private void Start()
    {
        foreach(TileScript tile in _player._allTiles)
        {
            if(tile.tilePos == new Vector2(tilePos.x + 1, tilePos.y) || tile.tilePos == new Vector2(tilePos.x - 1, tilePos.y) || tile.tilePos == new Vector2(tilePos.x , tilePos.y + 1) || tile.tilePos == new Vector2(tilePos.x, tilePos.y - 1))
            {
                _neighbors.Add(tile);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!_holdingScript.isHoldingPlayer && !isWall)
        {
            meshRenderer.material.color = Color.green;
            //Debug.Log("Hello");
        }

        if (hasEnemy)
        {
            foreach (var tile in _enemy._tilesInRange)
            {
                tile.meshRenderer.material.color = Color.darkOrange;
            }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_holdingScript.isHoldingPlayer && !isWall)
        {
            meshRenderer.material.color = Color.white;
            //Debug.Log("Goodbye");
        }

        if (hasEnemy)
        {
            foreach (var tile in _enemy._tilesInRange)
            {
                tile.meshRenderer.material.color = Color.white;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _player._clickedTile = this;

        if (hasPlayer && !_holdingScript.isHoldingPlayer)
        {
            _holdingScript.isHoldingPlayer = true;
            hasPlayer = false;

            _player._tilesInRange.Add(this);

            for (int playerMove = _player.moveRange; playerMove > 0; playerMove --)
            {
                int amountTilesInRange = _player._tilesInRange.Count;

                for (int i = 0; i < amountTilesInRange; i++)
                {
                    foreach (TileScript neigh in _player._tilesInRange[i]._neighbors)
                    {
                        bool isInList = false;
                        for(int p = 0; p < _player._tilesInRange.Count; p++)
                        {
                            if (_player._tilesInRange[p].tilePos == neigh.tilePos)
                            {
                                isInList = true;
                            }
                        }
                         
                        if(!isInList && !neigh.isWall && !neigh.hasEnemy)
                        {
                            _player._tilesInRange.Add(neigh);
                        }
                        
                    }
                }

            }

            foreach (TileScript tile in _player._tilesInRange)
            {
                tile.meshRenderer.material.color = Color.green;
            }

            //Collider[] hitTiles = Physics.OverlapSphere(this.transform.position, _player.moveRange);
            //foreach (Collider col in hitTiles)
            //{
            //    //Debug.Log(col);
            //    col.GetComponentInParent<TileScript>().meshRenderer.material.color = Color.green;
            //    _player._tilesInRange.Add(col.GetComponentInParent<TileScript>());
            //}
            _player.meshRenderer.enabled = false;
            _player._lastTile = this;

        }
        else if (!hasPlayer && _holdingScript.isHoldingPlayer)
        {
            bool isInRange = false;
            for (int i = 0; i < _player._tilesInRange.Count; i++)
            {
                if (_player._tilesInRange[i].tilePos == this.tilePos)
                {
                    isInRange = true;
                }
            }

            if (isInRange)
            {
                _holdingScript.isHoldingPlayer = false;
                _player.transform.position = new Vector3(tilePos.x, _objectPlayer.transform.position.y, tilePos.y);
                _player.meshRenderer.enabled = true;
                hasPlayer = true;
                _player._currentTile = this;

                foreach (TileScript tile in _player._tilesInRange)
                {
                    tile.meshRenderer.material.color = Color.white;
                }

                ResetPlayer();
                _enemy.EnemyTurn();
            }

            foreach(TileScript tile in _player._allTiles)
            {
                if(tile.hasPlayer)
                {
                    Debug.Log(tile + " has player");
                }
            }
            //Debug.Log("Clicked tile = " + tilePos);
        }
        else if(hasEnemy)
        {
            foreach(TileScript tile in _enemy._tilesInRange)
            {
                tile.meshRenderer.material.color = Color.darkOrange;
            }
        }
    }

    private void ResetPlayer()
    {
        int size = _player._tilesInRange.Count -1;
        for (int i = size; i >= 0; i--)
        {
            _player._tilesInRange.RemoveAt(i);
        }

        //foreach(TileScript tile in _player._tilesInRange)
        //{
        //    Debug.Log(tile);
        //}
    }

    
}
