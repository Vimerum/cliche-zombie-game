using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public float scroolSpeed = 5f;
    public float zoomYMin = 7f;
    public float zoomYMax = 14f;
    [Header("References")]
    public Transform target;
    public Transform mainCam;

    private void Start(){
        mainCam = transform.GetChild(0);
    }

    private void Update(){
        Zoom();
    }

    private void LateUpdate(){
        if (target != null) {
            transform.position = target.position;
        }
    }

    private void Zoom () {
        float scrool = Input.GetAxis("Mouse ScrollWheel");

        if (mainCam.localPosition.y <= zoomYMin) {
            scrool = Mathf.Min(0f, scrool);
        } else if (mainCam.localPosition.y >= zoomYMax) {
            scrool = Mathf.Max(0f, scrool);
        }

        mainCam.localPosition += mainCam.forward * scrool * scroolSpeed;
    }
}
