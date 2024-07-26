using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObj = new();

    public void AddObjectAt(Vector3Int gridPos, Vector2Int objectSize, int iD, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPos, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, iD, placedObjectIndex);

        foreach (var pos in positionToOccupy)
        {
            if(placedObj.ContainsKey(pos))
                throw new Exception($"Dectionary already contauns this cell position {pos}");

            placedObj[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPos, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for(int x = 0; x < objectSize.x; x++)
        {
            for(int y =0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPos + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlacedObjectAt(Vector3Int gridPos, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPos, objectSize);

        foreach(var pos in positionToOccupy)
        {
            if(placedObj.ContainsKey(pos)) return false;
        }

        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPos)
    {
        if(placedObj.ContainsKey(gridPos) == false)
            return -1;

        return placedObj[gridPos].placeObjIndex;
    }

    internal void RemovedObjectAt(Vector3Int gridPos)
    {
        foreach(var pos in placedObj[gridPos].occupiedPositions)
        {
            placedObj.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int iD { get; private set; }
    public int placeObjIndex { get; private set; }

    public PlacementData(List<Vector3Int> _occupiedPositions, int _iD, int _placedObjIndex)
    {
        this.occupiedPositions = _occupiedPositions;
        iD = _iD;
        placeObjIndex = _placedObjIndex;
    }
}
