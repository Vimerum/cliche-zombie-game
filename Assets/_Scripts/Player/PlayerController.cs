using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public float bulletTimeOut;
    [Header("References")]
    public GameObject prefab;

    private float nextShot = 0;
    private Rigidbody rb;
    private Vector3 target = new Vector3(0, -200, 0);
    [ReadOnly]public GridBlock currGridBlock;
    private readonly Vector3 diagonalHorizontal = (Vector3.right + Vector3.back).normalized;
    private readonly Vector3 diagonalVertical = (Vector3.right + Vector3.forward).normalized;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update(){
        rb.velocity = Vector3.zero;

        Vector2Int currPos = new Vector2(transform.position.x, transform.position.z).TruncateToInt();
        if (currGridBlock == null || currGridBlock.x != currPos.x || currGridBlock.y != currPos.y) {
            if (currPos.x >= 0 && currPos.x < GridManager.instance.gridSize && currPos.x >= 0 && currPos.x < GridManager.instance.gridSize) {
                currGridBlock = GridManager.instance.grid.GetBlock(currPos.x, currPos.y);
            }
        }

        Attack();
        Move();
    }

    private void Move(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical !=0){
            target = new Vector3(0, -200, 0);
        }
        transform.position += (diagonalHorizontal * horizontal + diagonalVertical * vertical).normalized * speed * (currGridBlock == null ? 1f : currGridBlock.speedModifier) * Time.deltaTime;
    }

    public void SetTarget(Vector3 target){
        this.target = target;
        this.target.y = 0;
    }

    public void Attack() {
        if (nextShot <= 0 && Input.GetMouseButton(0)){
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100)) {
                nextShot = bulletTimeOut;
                
                Vector3 direction = hit.point - transform.position;
                direction.y = 0;
                                
                Vector3 origin = transform.position;
                origin.y = 1;

                Instantiate(prefab, origin, Quaternion.LookRotation(direction.normalized));
            }
        }
        else {
            nextShot -= Time.deltaTime;
        }
    }
}
