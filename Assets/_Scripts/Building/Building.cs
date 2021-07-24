using UnityEngine;
using System.Collections;

[System.Serializable]
public class Building {

    [Header("Settings")]
    public string name;
    public Vector2 size;
    public GameObject prefab;
    [Header("Preview")]
    public GameObject previewPrefab;
    public Color previewNormalColor;
    public Color previewWrongColor;

    public Color GetColor (bool isPositionValid) {
        if (isPositionValid) {
            return previewNormalColor;
        } else {
            return previewWrongColor;
        }
    }

}
