using UnityEngine;

public interface IBuildingState
{
    public void EndState();
    public void OnAction(Vector3Int pos);
    public void UpdateState(Vector3Int gridPos);
}
