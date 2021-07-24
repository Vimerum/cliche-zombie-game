using UnityEngine;
using System.Collections.Generic;

public class BuildingPreviewBehaviour : MonoBehaviour {

    [Header("References")]
    public List<Renderer> renderers;

    public void SetPreviewColor (Color color) {
        foreach(Renderer rend in renderers) {
            Material[] materials = rend.materials;
            foreach(Material mat in materials) {
                mat.color = color;
            }
        }
    }
}
