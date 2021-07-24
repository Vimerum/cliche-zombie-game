using UnityEngine;
using System.Collections;

public enum GridBlockType {
    None = 0,
    Grass = 1,
    Forest = 6,
    Water = 10,
    Rock = 255,
}

[System.Serializable]
public class GridBlock {

    public int x;
    public int y;
    public float speedModifier;
    public GridBlockType type;
    public BuildingBehaviour buildingBehaviour;

    private GameObject spawnedGO;

    public GridBlock (int x, int y, GridBlockType type, float speedModifier) {
        this.x = x;
        this.y = y;
        this.type = type;
        this.speedModifier = speedModifier;
        buildingBehaviour = null;
    }

    public void SetGameObject (GameObject spawnedGO) {
        this.spawnedGO = spawnedGO;
    }

    public GameObject GetGameObject () {
        return spawnedGO;
    }

    public void SetBuilding (BuildingBehaviour building) {
        this.buildingBehaviour = building;
    }
}
