using System;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    public float previewYOffset = 0.06f;
    public GameObject cellIndicator;
    public Material _previewMaterialPrefab;

    GameObject _previewObject;
    Material _previewMaterialInstance;
    Renderer _cellIndicatorRenderer;


    public void Start()
    {
        _cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();

        _previewMaterialInstance = new Material(_previewMaterialPrefab);
        cellIndicator.SetActive(false);
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        _previewObject = Instantiate(prefab);
        PreparePreview(_previewObject);
        PrepareCursor(size);

        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicator.GetComponentInChildren<Renderer>().material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for(int i = 0; i < materials.Length; i++)
            {
                materials[i] = _previewMaterialInstance;
            }

            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if(_previewObject != null) Destroy(_previewObject);
    }

    public void UpdatePosition(Vector3 pos, bool validity)
    {
        if(_previewObject != null)
        {
            MovePreview(pos);
            ApplyFeedbackToPreview(validity);
        }
        
        MoveCursor(pos);
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        if(_cellIndicatorRenderer != null) _cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;

        if(_previewMaterialInstance != null) _previewMaterialInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
    }

    private void MoveCursor(Vector3 pos)
    {
        cellIndicator.transform.position = pos;
    }

    private void MovePreview(Vector3 pos)
    {
        _previewObject.transform.position = new Vector3(pos.x, pos.y + previewYOffset, pos.z);
    }

    internal void StartShowingRemovingPreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}
