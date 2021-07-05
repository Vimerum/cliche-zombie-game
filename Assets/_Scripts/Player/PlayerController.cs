using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float MovSpeed;
    private readonly Vector3 diagonalHorizontal = (Vector3.right + Vector3.back).normalized;
    private readonly Vector3 diagonalVertical = (Vector3.right + Vector3.forward).normalized;
    private Vector3 target = new Vector3(0, -200, 0);

    private void Update(){
        KeyboardMove();
        if (target.y > -100)
        {
            TargetMove();
        }
    }

    private void KeyboardMove(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical !=0){
            target = new Vector3(0, -200, 0);
        }
        transform.position += (diagonalHorizontal * horizontal + diagonalVertical * vertical).normalized* MovSpeed * Time.deltaTime;
    }

    private void TargetMove(){
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * MovSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position,target) < 0.01){
            target = new Vector3(0, -200, 0);
        }
    }
    public void SetTarget(Vector3 target){
        this.target = target;
        this.target.y = 0;
    }
}
