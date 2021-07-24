using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour {

    private struct BuildingPreviewStatus {
        public Vector2Int Pos { get; private set; }
        public bool IsPositionValid { get; private set; } 
        public GameObject previewGameObject;

        private BuildingPreviewBehaviour previewBehaviour;

        public void SetPos (Vector2Int pos, bool isPositionValid) {
            Pos = pos;
            previewGameObject.transform.position = new Vector3(Pos.x, 0, Pos.y);

            IsPositionValid = isPositionValid;
        }

        public void UpdatePreviewColor (Color color) {
            if (previewBehaviour == null) {
                previewBehaviour = previewGameObject.GetComponent<BuildingPreviewBehaviour>();
            }

            previewBehaviour.SetPreviewColor(color);
        }
    }

    [Header("Settings")]
    public bool overrideInvalidBlockTypes;
    public List<GridBlockType> invalidBlockTypes;
    [Header("UI")]
    public GameObject prefabBuildingMenu;
    [Header("References")]
    public GameObject buildingModel;

    private Building building;
    private BuildingPreviewStatus previewStatus;
    private GameObject buildingMenu;

    public void ShowPreview (Building building) {
        this.building = building;

        buildingModel.SetActive(false);
        previewStatus.previewGameObject = Instantiate(this.building.previewPrefab, transform);
    }

    public void UpdatePreview (Vector2Int pos) {
        previewStatus.SetPos(pos, IsPositionValid(pos));
        previewStatus.UpdatePreviewColor(building.GetColor(previewStatus.IsPositionValid));
    }

    public bool Spawn () {
        if (!previewStatus.IsPositionValid) {
            return false;
        }

        GridBlock gridBlock = GridManager.instance.grid.GetBlock(previewStatus.Pos.x, previewStatus.Pos.y);
        gridBlock.SetBuilding(this);

        transform.position = new Vector3(previewStatus.Pos.x, 0, previewStatus.Pos.y);
        buildingModel.SetActive(true);

        Destroy(previewStatus.previewGameObject);
        return true;
    }

    public bool IsPositionValid(Vector2Int pos) {
        for (int x = pos.x; x < pos.x + building.size.x; x++) {
            if (x < 0 || x >= GridManager.instance.gridSize) {
                return false;
            }
            for (int y = pos.y; y < pos.y + building.size.y; y++) {
                if (y < 0 || y >= GridManager.instance.gridSize) {
                    return false;
                }

                GridBlock currBlock = GridManager.instance.grid.GetBlock(x, y);
                if (currBlock.building != null) {
                    return false;
                }
                if (overrideInvalidBlockTypes) {
                    if (invalidBlockTypes.Contains(currBlock.type)) {
                        return false;
                    }
                } else {
                    if (BuildingManager.instance.invalidBlockTypes.Contains(currBlock.type)) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public void OnClick() {
        buildingMenu = Instantiate(prefabBuildingMenu, BuildingManager.instance.canvas);
    }

    public void CloseMenu () {
        if (buildingMenu != null) {
            Destroy(buildingMenu);
        }
    }
}
