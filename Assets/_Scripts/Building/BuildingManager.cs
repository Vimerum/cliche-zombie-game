using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance;

    public enum BuildingMode {
        None,
        Selecting,
        Positioning,
        Building
    }

    [Header("Settings")]
    public List<GridBlockType> invalidBlockTypes;
    public LayerMask buildingPositionLayers;
    [Header("Prefabs")]
    public Building mainBase;
    public List<Building> buildings;
    public GameObject buildingMenuItem;
    [Header("References")]
    public Transform canvas;
    public GameObject buildingMenu;

    [ReadOnly, SerializeField]
    private BuildingMode mode;

    private int lastBuildingIndex = -1;
    private bool hasMainBasedSpawned = false;
    private RectTransform buildingMenuRect;
    private Camera cam;
    private Coroutine endGameCO = null;
    private BuildingBehaviour mainBaseBehaviour;
    private BuildingBehaviour currBuildingBehaviour;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start() {
        buildingMenuRect = buildingMenu.GetComponent<RectTransform>();

        mode = BuildingMode.None;
        cam = Camera.main;

        InitializeUIBegining();
        DeactivateMenu();
    }

    private void Update() {
        if (hasMainBasedSpawned && mainBaseBehaviour == null) {
            if (endGameCO != null) {
                return;
            }
            DeactivateMenu();
            EnemyController.shouldDance = true;
            endGameCO = StartCoroutine(EndGameCO());
            return;
        }

        switch(mode) {
            case BuildingMode.None: {
                    if (Input.GetMouseButtonDown(1)) {
                        ActivateMenu();
                    }
                    break;
                }
            case BuildingMode.Selecting: {
                    if (Input.GetButtonDown("Cancel") || Input.GetMouseButtonDown(1)) {
                        DeactivateMenu();
                    }
                    break;
                }
            case BuildingMode.Positioning: {
                    if (Input.GetButtonDown("Cancel")) {
                        Destroy(currBuildingBehaviour.gameObject);
                        mode = BuildingMode.None;
                        break;
                    }
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, buildingPositionLayers)) {
                        Vector2Int mousePos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));

                        currBuildingBehaviour.UpdatePreview(mousePos);

                        if (Input.GetMouseButtonDown(0) && currBuildingBehaviour.Spawn()) {
                            if (mainBaseBehaviour == null) {
                                mainBaseBehaviour = currBuildingBehaviour;
                                hasMainBasedSpawned = true;
                                InitializeUIGeneral();
                                EnemyManager.instance.flowField.Generate(mousePos);
                            } else {
                                EnemyManager.instance.flowField.Recalculate();
                            }

                            if (lastBuildingIndex >= 0 && Input.GetButton("QueueBuilding")) {
                                PositionBuilding(lastBuildingIndex);
                            } else {
                                mode = BuildingMode.Building;
                            }
                        }
                    }
                    break;
                }
            case BuildingMode.Building: {
                    mode = BuildingMode.None;
                    break;
                }
        }
    }

    private IEnumerator EndGameCO () {
        GameManager.instance.BeginGameOver();
        yield return new WaitForSeconds(14f);
        GameManager.instance.GameOver();
    }

    private void InitializeUIBegining () {
        GameObject itemGO = Instantiate(buildingMenuItem, buildingMenu.transform);
        BuildingItem item = itemGO.GetComponent<BuildingItem>();

        item.SetBuilding(-1, mainBase.name, mainBase.price);
    }

    private void InitializeUIGeneral () {
        Destroy(buildingMenu.transform.GetChild(0).gameObject);

        for (int i = 0; i < buildings.Count; i++) {
            GameObject itemGO = Instantiate(buildingMenuItem, buildingMenu.transform);
            BuildingItem item = itemGO.GetComponent<BuildingItem>();

            item.SetBuilding(i, buildings[i].name, buildings[i].price);
        }
    }

    private void ToggleMenu () {
        if (mode == BuildingMode.None) {
            ActivateMenu();
        } else {
            DeactivateMenu();
        }
    }

    public void ActivateMenu () {
        if (hasMainBasedSpawned && mainBaseBehaviour == null) {
            return;
        }

        mode = BuildingMode.Selecting;

        Vector3 pos = Input.mousePosition;
        pos.x = Mathf.Min(pos.x, Screen.width - buildingMenuRect.sizeDelta.x);
        pos.y = Mathf.Max(pos.y, buildingMenuRect.sizeDelta.y);

        buildingMenu.SetActive(true);
        buildingMenuRect.position = pos;
    }

    public void DeactivateMenu () {
        if (mode == BuildingMode.Selecting) {
            mode = BuildingMode.None;
        }
        buildingMenu.SetActive(false);
    }

    public void PositionBuilding (int index) {
        mode = BuildingMode.Positioning;
        DeactivateMenu();

        Building currBuilding = mainBase;
        if (index >= 0) {
            currBuilding = buildings[index];
        }
        GameObject buildingGO = Instantiate(currBuilding.prefab, transform);
        currBuildingBehaviour = buildingGO.GetComponent<BuildingBehaviour>();

        currBuildingBehaviour.ShowPreview(currBuilding);
        lastBuildingIndex = index;
    }

    public bool HasMainBase () {
        return mainBaseBehaviour != null;
    }

    public Transform GetMainBaseTransform () {
        return mainBaseBehaviour.transform;
    }
}
