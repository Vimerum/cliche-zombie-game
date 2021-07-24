using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bulletSpd;
    public float lifeSpan;
    public float damage;

    private void Start() {
        Destroy(gameObject,lifeSpan);
    }

    private void Update() {
        transform.position += transform.forward * bulletSpd * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
