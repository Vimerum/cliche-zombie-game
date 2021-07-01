using UnityEngine;
using System.Collections;

public enum GridBlockType {
    None = 0,
    Grass = 1,
    Water = 2,
    Forest = 3,
    Rock = 4,
}

public class GridBlock {

    public int x;
    public int y;
    public GridBlockType type;

    private GameObject spawnedGO;

    public GridBlock (int x, int y, GridBlockType type) {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    public void SetGameObject (GameObject spawnedGO) {
        this.spawnedGO = spawnedGO;
    }
}
