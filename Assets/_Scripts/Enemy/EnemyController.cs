using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static bool shouldDance = false;
    [Header("Settings")]
    public float speed;
    public float damageCooldown;
    public float damage;
    [Header("References")]
    public Animator anim;

    private Rigidbody rb;
    private Vector3 target;
    private GridBlock currGridBlock;
    private Vector3 lastPos;
    private float damageTimeout;
    private bool setDance = false;
    private bool shouldDanceLocal = false;

    void Start() {
        damageTimeout = -1f;
        lastPos = transform.position;
        rb = GetComponent<Rigidbody>();
        GetNewTarget();
    }

    void Update() {
        if (shouldDance && !setDance) {
            shouldDanceLocal = true;
            setDance = true;
        }
        if (shouldDance && shouldDanceLocal) {
            anim.SetTrigger("Thriller");
            shouldDanceLocal = false;
        }
        if (shouldDance) {
            rb.freezeRotation = true;
            return;
        }
        
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

        float speedModifier = (currGridBlock == null ? 1f : currGridBlock.speedModifier);
        transform.LookAt(target);
        transform.position += transform.forward * speed * speedModifier * Time.deltaTime;

        bool isRunning = false;
        float lastPosDist = Vector3.Distance(transform.position, lastPos);
        if (lastPosDist >= (speed * speedModifier * Time.deltaTime)) {
            isRunning = true;
        }

        anim.SetBool("Running", isRunning);
        anim.SetFloat("Speed Modifier", speedModifier);
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

        anim.SetTrigger("Attack");
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
