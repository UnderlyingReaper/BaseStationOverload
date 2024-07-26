using UnityEngine;

public class CanvasLookCamera : MonoBehaviour
{
    Transform mainCam;


    void Start()
    {
        mainCam = Camera.main.transform;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
    }
}
