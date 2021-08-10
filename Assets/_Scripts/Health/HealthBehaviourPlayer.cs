using UnityEngine;
using System.Collections;

public class HealthBehaviourPlayer : HealthBehaviour {

    [Header("Settings - Player")]
    public float invincibleTime; 

    private void Start() {
        timeout = invincibleTime;
    }

    protected override void Death() {
        base.Death();

        Destroy(gameObject);
        GameManager.instance.RespawnPlayer();
    }

    private void OnCollisionStay(Collision collision) {
        if(collision.gameObject.tag == "Enemy") {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

            float damage = enemyController.GetDamage();

            Damage(damage);
        }
    }
}
