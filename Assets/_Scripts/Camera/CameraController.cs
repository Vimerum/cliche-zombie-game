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
    public Transform player;

    private Transform cam;
    private PlayerController playerController;

    private void Start(){
        cam = transform.GetChild(0);
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update(){
        Zoom();
        MoveOnClick();
    }

    private void LateUpdate(){
        transform.position = player.position;
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

    private void MoveOnClick(){
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                playerController.SetTarget(hit.point);
            }
        }
    }
}
