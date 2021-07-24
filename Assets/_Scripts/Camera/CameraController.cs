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

    private Transform cam;

    private void Start(){
        cam = transform.GetChild(0);
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

        if (cam.localPosition.y <= zoomYMin) {
            scrool = Mathf.Min(0f, scrool);
        } else if (cam.localPosition.y >= zoomYMax) {
            scrool = Mathf.Max(0f, scrool);
        }

        cam.localPosition += cam.forward * scrool * scroolSpeed;
    }
}
