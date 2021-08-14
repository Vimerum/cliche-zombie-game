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

        public void UpdatePreviewColor (bool isValid) {
            if (previewBehaviour == null) {
                previewBehaviour = previewGameObject.GetComponent<BuildingPreviewBehaviour>();
            }

            previewBehaviour.SetPreviewColor(isValid);
        }
    }

    [Header("Settings")]
    public bool overrideInvalidBlockTypes;
    public List<GridBlockType> invalidBlockTypes;
    [Header("References")]
    public GameObject buildingModel;
    public ResourcesBehaviour resourcesBehaviour;

    [HideInInspector]
    public Building building;
    private BuildingPreviewStatus previewStatus;

    public void ShowPreview (Building building) {
        if (resourcesBehaviour != null) {
            resourcesBehaviour.enabled = false;
        }
        this.building = building;

        buildingModel.SetActive(false);
        previewStatus.previewGameObject = Instantiate(this.building.previewPrefab, transform);
    }

    public void UpdatePreview (Vector2Int pos) {
        previewStatus.SetPos(pos, IsPositionValid(pos));
        previewStatus.UpdatePreviewColor(previewStatus.IsPositionValid && building.HaveResources() && building.Validate(previewStatus.Pos));
    }

    public bool Spawn () {
        if (!building.HaveResources()) {
            return false;
        }
        if (!previewStatus.IsPositionValid) {
            return false;
        }
        if (!building.Validate(previewStatus.Pos)) {
            Debug.Log("Invalid position " + previewStatus.Pos);
            return false;
        }

        building.price.ForEach((item) => {
            ResourcesManager.instance.Withdraw(item.resource, item.value);
        });

        for (int x = previewStatus.Pos.x; x < previewStatus.Pos.x + building.size.x; x++) {
            for (int y = previewStatus.Pos.y; y < previewStatus.Pos.y + building.size.y; y++) {
                GridBlock gridBlock = GridManager.instance.grid.GetBlock(x, y);
                gridBlock.SetBuilding(this);
            }
        }

        transform.position = new Vector3(previewStatus.Pos.x, 0, previewStatus.Pos.y);
        buildingModel.SetActive(true);

        if (resourcesBehaviour != null) {
            resourcesBehaviour.enabled = true;
        }
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
                if (currBlock.buildingBehaviour != null) {
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
}
