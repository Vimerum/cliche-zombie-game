using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingItem : MonoBehaviour {

    [Header("References")]
    public TextMeshProUGUI buildingNameLabel;

    private int buildingIndex;

    public void OnClick () {
        BuildingManager.instance.PositionBuilding(buildingIndex);
    }

    public void SetBuilding (int index, string name) {
        this.buildingIndex = index;
        buildingNameLabel.text = name;
    }
}
