using UnityEngine;
using System.Collections.Generic;

public class BuildingPreviewBehaviour : MonoBehaviour {

    [Header("References")]
    public List<GameObject> validPreview;
    public List<GameObject> invalidPreview;

    public void SetPreviewColor (bool isValidPosition) {
        foreach(GameObject preview in validPreview) {
            preview.SetActive(isValidPosition);
        }
        foreach(GameObject preview in invalidPreview) {
            preview.SetActive(!isValidPosition);
        }
    }
}
