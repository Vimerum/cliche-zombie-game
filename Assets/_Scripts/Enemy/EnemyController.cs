using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float damageCooldown;
    public float damage;

    private float damageTimeout;
    private Vector3 target;

    void Start() {
        damageTimeout = -1f;
        GetNewTarget();
    }

    void Update() {
        damageTimeout -= Time.deltaTime;
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
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void GetNewTarget() {
        Vector2 newPos = EnemyManager.instance.GetTargetDirection(transform.position);
        target = new Vector3(newPos.x, 0, newPos.y) + transform.position;
    }

    public float GetDamage () {
        if (damageTimeout > 0) {
            return 0f;
        }

        damageTimeout = damageCooldown;
        return damage;
    }
}
