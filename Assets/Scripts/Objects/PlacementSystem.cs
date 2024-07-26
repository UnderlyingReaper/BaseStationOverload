using System;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public bool isActive;
    public InputManager inputManager;
    public ObjectPlacer objectPlacer;
    public Grid grid;
    public PreviewSystem previewSystem;
    public MoneySystem moneySystem;

    public ObjectDatabase dataBase;
    public GameObject gridVisual;


    GridData _groundData, _buildingData;
    Vector3Int lastDetectedPos = Vector3Int.zero;
    IBuildingState _buildingState;
    public event Action startPlacing;
    public event Action stopPlacing;


    void Start()
    {
        StopPlacement();

        _buildingData = new();
        _groundData = new();
    }
    void Update()
    {
        if(_buildingState == null) return;
        
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        if(lastDetectedPos != gridPos)
        {
            _buildingState.UpdateState(gridPos);
            lastDetectedPos = gridPos;
        }   
    }

    public void StartPlacement(int iD)
    {
        if(dataBase.objectData[iD].price > moneySystem.cash) return;

        StopPlacement();
        gridVisual.SetActive(true);

        _buildingState = new PlacementState(iD, grid, previewSystem, dataBase, _groundData, _buildingData, objectPlacer, moneySystem);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

        startPlacing?.Invoke();
        isActive = true;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisual.SetActive(true);

        _buildingState = new RemovingState(grid, previewSystem, _groundData, _buildingData, objectPlacer, dataBase);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

        startPlacing?.Invoke();
        isActive = true;
    }

    private void StopPlacement()
    {
        gridVisual.SetActive(false);

        if(_buildingState != null) _buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        stopPlacing?.Invoke();
        isActive = false;

        lastDetectedPos = Vector3Int.zero;

        _buildingState = null;
    }

    private void PlaceStructure()
    {
        if(inputManager.isPointOverUI())
        {
            Debug.Log("Returning");
            return;
        }

        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        
        _buildingState.OnAction(gridPos);
        StopPlacement();
    }

    // private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectIndex)
    // {
    //     GridData selectedData = dataBase.objectData[_selectedObjectIndex].iD == 0 ? _groundData : _buildingData;

    //     return selectedData.CanPlacedObjectAt(gridPos, dataBase.objectData[selectedObjectIndex].size);
    // }
}
