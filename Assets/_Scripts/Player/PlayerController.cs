using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovSpeed;
    private readonly Vector3 diagonalHorizontal = (Vector3.left + Vector3.forward).normalized;
    private readonly Vector3 diagonalVertical = (Vector3.right + Vector3.forward).normalized;

    void Update()
    {
        Move();
    }

    private void Move() 
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.position += (diagonalHorizontal * horizontal*(-1) + diagonalVertical * vertical) * Time.deltaTime;
    }
}
