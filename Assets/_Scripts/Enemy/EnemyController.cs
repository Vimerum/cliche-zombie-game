using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float MovSpeed;
    private Vector3 target;

    void Start() {
        GetNewTarget();
    }

    void Update() {
        Move();
    }

    public void Move() {
        if(transform.position.x == 50 && transform.position.z == 50){
            return;
        }
        if (Vector3.Distance(target, transform.position) < 0.9){
            GetNewTarget();
        }
        transform.LookAt(target);
        transform.position += transform.forward * MovSpeed * Time.deltaTime;
    }

    public void GetNewTarget() {
        Vector2 newPos = EnemyManager.instance.GetTargetDirection(transform.position);
        target = new Vector3(newPos.x, 0, newPos.y) + transform.position;
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Bullet") {
            Destroy(collision.gameObject);
            //TODO
            //definir dano e hit effect
            Debug.Log("Acertou mizeravi");
        }
    }
}
