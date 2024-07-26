using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Camera sceneCamera;
    public LayerMask placementMask;
    public float range;

    public event Action OnClicked, OnExit;


    Vector3 _lastPos;
    bool _isPointOverUI;

    

    void Update()
    {
        _isPointOverUI = EventSystem.current.IsPointerOverGameObject();
        if(_isPointOverUI) Debug.Log(EventSystem.current.gameObject);
        
        if(Input.GetMouseButtonDown(0)) OnClicked?.Invoke();

        if(Input.GetKeyDown(KeyCode.Escape)) OnExit?.Invoke();
    }

    public bool isPointOverUI()
    {
        _isPointOverUI = EventSystem.current.IsPointerOverGameObject();
        if(_isPointOverUI) Debug.Log(EventSystem.current.gameObject);
        return _isPointOverUI;
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hit, range, placementMask))
        {
            _lastPos = hit.point;
        }
        return _lastPos;
    }
}
