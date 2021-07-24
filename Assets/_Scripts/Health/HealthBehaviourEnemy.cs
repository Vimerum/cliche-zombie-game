using UnityEngine;
using System.Collections;

public class HealthBehaviourEnemy : HealthBehaviour {

    protected override void Death() {
        base.Death();

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Bullet") {
            BulletBehavior bulletBehavior = collision.gameObject.GetComponent<BulletBehavior>();

            Damage(bulletBehavior.damage);
        }
    }
}
