using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingItem : MonoBehaviour {

    [Header("References")]
    public TextMeshProUGUI buildingNameLabel;
    public TextMeshProUGUI woodValueLabel;
    public TextMeshProUGUI stoneValueLabel;

    private int buildingIndex;

    public void OnClick () {
        BuildingManager.instance.PositionBuilding(buildingIndex);
    }

    public void SetBuilding (int index, string name, List<Building.Price> prices) {
        this.buildingIndex = index;
        buildingNameLabel.text = name;

        foreach(Building.Price price in prices) {
            if (price.resource == Resource.Wood) {
                woodValueLabel.text = price.value.ToString();
            }
            if (price.resource == Resource.Stone) {
                stoneValueLabel.text = price.value.ToString();
            }
        }
    }
}
