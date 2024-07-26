using UnityEngine;

public class PlacementState: IBuildingState
{
    int _selectedObjectIndex = -1;
    int iD;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabase dataBase;
    GridData groundData, buildingData;
    ObjectPlacer objectPlacer;
    MoneySystem moneySystem;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectDatabase dataBase, GridData groundData, GridData buildingData, ObjectPlacer objectPlacer, MoneySystem moneySystem)
    {
        this.iD = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.dataBase = dataBase;
        this.groundData = groundData;
        this.buildingData = buildingData;
        this.objectPlacer = objectPlacer;
        this.moneySystem = moneySystem;
        
        _selectedObjectIndex = dataBase.objectData.FindIndex(data => data.iD == iD);

        if(_selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(dataBase.objectData[_selectedObjectIndex].Prefab,
                                                       dataBase.objectData[_selectedObjectIndex].size);
        }
        else
            throw new System.Exception($"No Object with ID {iD}");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);
        if(!placementValidity) return;
        
        int index = objectPlacer.PlaceObject(dataBase.objectData[_selectedObjectIndex].Prefab, grid.CellToWorld(gridPos));

        GridData selectedData = dataBase.objectData[_selectedObjectIndex].iD == 0 ? groundData : buildingData;
        selectedData.AddObjectAt(gridPos, dataBase.objectData[_selectedObjectIndex].size,
                                          dataBase.objectData[_selectedObjectIndex].iD,
                                          index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), false);

        moneySystem.RemoveCash(dataBase.objectData[iD].price);
    }

    private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectIndex)
    {
        GridData selectedData = dataBase.objectData[_selectedObjectIndex].iD == 0 ? groundData : buildingData;

        return selectedData.CanPlacedObjectAt(gridPos, dataBase.objectData[selectedObjectIndex].size);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), placementValidity);
    }
}
