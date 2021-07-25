using UnityEngine;
using System.Collections;

public class TowerBehaviour : MonoBehaviour {

    [Header("Settings")]
    public LayerMask layers;
    public float timeToShoot;
    public float range;
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    private Transform target;
    private float timeout = -1f;

    private void Update() {
        if (timeout <= 0f) {
            if (target == null || Vector3.Distance(transform.position, target.position) > range) {
                AttackTheEnemy();
            } else {
                Attack();
            }
        } else {
            timeout -= Time.deltaTime;
        }
    }

    private void AttackTheEnemy () {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, layers);

        if (hits.Length > 0) {
            Collider closest = hits[0];
            float dist = Vector3.Distance(transform.position, closest.transform.position);

            foreach(Collider col in hits) {
                float newDist = Vector3.Distance(transform.position, col.transform.position);
                if (newDist < dist) {
                    closest = col;
                    dist = newDist;
                }
            }

            target = closest.transform;
            Attack();
        }
    }

    private void Attack () {
        timeout = timeToShoot;
        
        Vector3 direction = target.position - bulletSpawnPoint.position;
        direction.y = 0;
                        
        Vector3 origin = bulletSpawnPoint.position;
        origin.y = 1;

        Instantiate(bulletPrefab, origin, Quaternion.LookRotation(direction.normalized));
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
