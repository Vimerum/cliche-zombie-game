using UnityEngine;
using System.Collections.Generic;

public enum PathfindBuildingType {
    Ignore = 1,
    NotIngnore = 255,
}

[System.Serializable]
public class Building {

    [System.Serializable]
    public struct Price {
        public Resource resource;
        public int value;
    }

    [Header("Settings")]
    public string name;
    public Vector2 size;
    public List<Price> price;
    public PathfindBuildingType pathfindBuildingType;
    public GameObject prefab;
    [Header("Preview")]
    public GameObject previewPrefab;

    public bool HaveResources () {
        bool haveResources = true;

        foreach (Price p in price) {
            haveResources &= ResourcesManager.instance.CheckWithdraw(p.resource, p.value);
        }

        return haveResources;
    }
}
