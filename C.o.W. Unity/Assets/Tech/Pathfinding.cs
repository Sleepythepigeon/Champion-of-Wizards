using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int moveStraightCost = 10;
    private const int moveDiagonalCost = 14;

    private List<TileScript> openList;
    private List<TileScript> closedList;
    private PlayerScript playerAccess;

    private void Awake()
    {
        playerAccess = FindAnyObjectByType<PlayerScript>();
    }




    public List<TileScript> FindPath(TileScript startingTile, TileScript endingTile)
    {
        openList = new List<TileScript>() { startingTile};
        closedList = new List<TileScript>();

        foreach(TileScript tile in playerAccess._allTiles)
        {
            tile.gCost = int.MaxValue;
            tile.CalculateFCost();
            tile.cameFromTile = null;
        }

        startingTile.gCost = 0;
        startingTile.hCost = CalculateDistanceCost(startingTile, endingTile);
        startingTile.CalculateFCost();

        while (openList.Count > 0)
        {
            TileScript currentTile = GetLowestFCostTile(openList);
            if(currentTile == endingTile)
            {
                //Debug.Log("Should Have returned Path");
                return CalculatePath(endingTile);
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach (TileScript neigh in currentTile._neighbors)
            {
                if (closedList.Contains(neigh))
                {
                    continue;
                }

                int tentativeGCost = currentTile.gCost + CalculateDistanceCost(currentTile, neigh);
                if (tentativeGCost < neigh.gCost)
                {
                    neigh.cameFromTile = currentTile;
                    neigh.gCost = tentativeGCost;
                    neigh.hCost = CalculateDistanceCost(neigh, endingTile);
                    neigh.CalculateFCost();

                    if (!openList.Contains(neigh))
                    {
                        openList.Add(neigh);
                        //Debug.Log("Added " + neigh);
                    }
                }
            }

        }

        //Out of nodes on the openList

        //Debug.Log("Returned Nothing");
        return null;
    }

    private List<TileScript> CalculatePath(TileScript endTile)
    {
        List<TileScript> path = new List<TileScript>();
        path.Add(endTile);
        TileScript currentTile = endTile;
        while (currentTile.cameFromTile != null)
        {
            path.Add(currentTile.cameFromTile);
            currentTile = currentTile.cameFromTile;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(TileScript a, TileScript b)
    {
        int xDistance = (int)Mathf.Abs(a.tilePos.x - b.tilePos.x);
        int yDistance = (int)Mathf.Abs(a.tilePos.y - b.tilePos.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return moveDiagonalCost * Mathf.Min(xDistance, yDistance) + moveStraightCost * remaining;
    }

    private TileScript GetLowestFCostTile(List<TileScript> tileList)
    {
        TileScript lowestFCostTile = tileList[0];
        for (int i = 1; i < tileList.Count; i++)
        {
            if (tileList[i].fCost < lowestFCostTile.fCost)
            {
                lowestFCostTile = tileList[i];
            }
        }

        return lowestFCostTile;
    }
}
