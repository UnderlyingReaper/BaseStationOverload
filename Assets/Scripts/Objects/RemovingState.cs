using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    int _gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabase database;
    GridData groundData, buildingData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData groundData, GridData buildingData, ObjectPlacer objectPlacer, ObjectDatabase database)
    {
        this.grid = grid;
        this.database = database;
        this.previewSystem = previewSystem;
        this.groundData = groundData;
        this.buildingData = buildingData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovingPreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos)
    {
        GridData selectedData = null;

        if(buildingData.CanPlacedObjectAt(gridPos, Vector2Int.one) == false)
        {
            selectedData = buildingData;
        }
        else if(groundData.CanPlacedObjectAt(gridPos, Vector2Int.one) == false)
        {
            selectedData = groundData;
        }

        if(selectedData == null)
        {
            // play some sound idk
        }
        else
        {
            _gameObjectIndex = selectedData.GetRepresentationIndex(gridPos);
            if(_gameObjectIndex == -1) return;

            selectedData.RemovedObjectAt(gridPos);
            objectPlacer.RemoveObjectAt(_gameObjectIndex);
        }

        Vector3 cellPos = grid.CellToWorld(gridPos);
        previewSystem.UpdatePosition(cellPos, CheckIfSelectionIsValid(gridPos));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPos)
    {
        return buildingData.CanPlacedObjectAt(gridPos, Vector2Int.one) && buildingData.CanPlacedObjectAt(gridPos, Vector2Int.one);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool validity = CheckIfSelectionIsValid(gridPos);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), validity);
    }
}
