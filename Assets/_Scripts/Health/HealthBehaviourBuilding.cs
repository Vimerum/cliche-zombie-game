using UnityEngine;
using System.Collections;

public class HealthBehaviourBuilding : HealthBehaviour {

    protected override void Death() {
        base.Death();

        Destroy(gameObject);

        Vector2Int pos = new Vector2(transform.position.x, transform.position.z).TruncateToInt();
        Building building = GetComponent<BuildingBehaviour>().building;

        for (int x = pos.x; x < pos.x + building.size.x; x++) {
            for (int y = pos.y; y < pos.y + building.size.y; y++) {
                GridBlock gridBlock = GridManager.instance.grid.GetBlock(x, y);
                gridBlock.SetBuilding(null);
            }
        }

        EnemyManager.instance.flowField.Recalculate();
    }

    private void OnCollisionStay(Collision collision) {
        if(collision.gameObject.tag == "Enemy") {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

            Damage(enemyController.GetDamage());
        }
    }
}
