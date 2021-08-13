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
    private GridBlock currGridBlock;

    void Start() {
        damageTimeout = -1f;
        GetNewTarget();
    }

    void Update() {
        if (transform.position.y < -10) {
            Destroy(gameObject);
        }

        damageTimeout -= Time.deltaTime;

        Vector2Int currPos = new Vector2(transform.position.x, transform.position.z).TruncateToInt();
        if (currGridBlock == null || currGridBlock.x != currPos.x || currGridBlock.y != currPos.y) {
            currGridBlock = GridManager.instance.grid.GetBlock(currPos.x, currPos.y);
        }
        
        Move();
    }

    public void Move() {
        if(transform.position.x == 50 && transform.position.z == 50){
            return;
        }
        float dist = Vector3.Distance(target, transform.position);
        if (dist < 0.5f || dist > 1.5f){
            GetNewTarget();
        }
        transform.LookAt(target);
        transform.position += transform.forward * speed * (currGridBlock == null ? 1f : currGridBlock.speedModifier) * Time.deltaTime;
    }

    public void GetNewTarget() {
        Vector2 newPos = EnemyManager.instance.GetTargetDirection(transform.position);
        target = new Vector3(newPos.x, 0, newPos.y) + new Vector3(Mathf.RoundToInt(transform.position.x), 0f, Mathf.RoundToInt(transform.position.z));
        //Debug.Log(Vector3.Distance(transform.position, target));
    }

    public float GetDamage () {
        if (damageTimeout > 0) {
            return 0f;
        }

        damageTimeout = damageCooldown;
        return damage;
    }

    private void OnDrawGizmosSelected() {
        float y = 5f;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(target.x, y, target.z), 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(transform.position.x, y, transform.position.z), 0.1f);
        Gizmos.color = Color.yellow;
        Vector2Int p = new Vector2(transform.position.x, transform.position.z).TruncateToInt();
        Gizmos.DrawSphere(new Vector3(p.x, y, p.y), 0.1f);
    }
}
