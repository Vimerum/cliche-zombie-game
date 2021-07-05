using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private PlayerController playerController;

    private void Start(){
        playerController = player.GetComponent<PlayerController>();
    }

    private void LateUpdate(){
        transform.position = player.position;
    }

    private void Update(){
        MoveOnClick();
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
