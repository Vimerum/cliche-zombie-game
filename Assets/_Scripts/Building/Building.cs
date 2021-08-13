using UnityEngine;
using System.Collections;

public enum PathfindBuildingType {
    Ignore = 1,
    NotIngnore = 255,
}

[System.Serializable]
public class Building {

    [Header("Settings")]
    public string name;
    public Vector2 size;
    public PathfindBuildingType pathfindBuildingType;
    public GameObject prefab;
    [Header("Preview")]
    public GameObject previewPrefab;

}
