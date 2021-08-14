using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BuildingValidationBase {

    [System.Serializable]
    public struct ValidationRule {
        public int distance;
        public GridBlockType type;
    }

    [Header("Settings")]
    public List<ValidationRule> rules;

    public bool Validate(Building building, Vector2Int position) {
        foreach (ValidationRule rule in rules) {
            bool foundValidBlock = false;
            for (int x = position.x - rule.distance; !foundValidBlock && x < position.x + building.size.x + rule.distance; x++) {
                for (int y = position.y - rule.distance; !foundValidBlock && y < position.y + building.size.y + rule.distance; y++) {
                    GridBlock currBlock = GridManager.instance.grid.GetBlock(x, y);
                    if (currBlock.type == rule.type) {
                        foundValidBlock = true;
                    }
                }
            }
            if (!foundValidBlock) {
                return false;
            }
        }

        return true;
    }
}
