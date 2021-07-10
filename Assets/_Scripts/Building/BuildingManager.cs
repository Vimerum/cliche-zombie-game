using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public enum BuildingMode {
        None,
        Selecting,
        Positioning,
        Building
    }

    [Header("Prefabs")]
    public List<Building> buildings;
    [Header("References")]
    public GameObject buildingMenu;
    public RectTransform buildingMenuRect;

    private BuildingMode mode;
    private Camera cam;

    private void Start() {
        buildingMenuRect = buildingMenu.GetComponent<RectTransform>();

        mode = BuildingMode.None;
        cam = Camera.main;
    }

    private void Update() {
        CheckBuildingMenu();
    }

    private void CheckBuildingMenu () {
        if (Input.GetMouseButtonDown(1)) {
            ActivateMenu();
        }
        if (Input.GetButtonDown("Cancel")) {
            DeactivateMenu();
        }
    }

    private void ActivateMenu () {
        mode = BuildingMode.Selecting;
        buildingMenu.SetActive(true);
        buildingMenuRect.position = Input.mousePosition;
    }

    private void DeactivateMenu () {
        mode = BuildingMode.None;
        buildingMenu.SetActive(false);
    }
}
