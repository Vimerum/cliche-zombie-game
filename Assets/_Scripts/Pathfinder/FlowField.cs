using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created based on Leif Erkenbrach's post, available at: https://leifnode.com/2013/12/flow-field-pathfinding/ (accessed on 08/July/2021)
 **/

public class FlowField {
    private const int INITIAL_VALUE = int.MaxValue;

    private int gridSize;
    private int[,] integrationField;
    private Vector2Int[,] flowField;

    public FlowField (int gridSize) {
        this.gridSize = gridSize;
        integrationField = new int[gridSize, gridSize];
        flowField = new Vector2Int[gridSize, gridSize];
    }

    private void ResetField () {
        for(int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                integrationField[x, y] = INITIAL_VALUE;
            }
        }
    }

    private List<Vector2Int> CalculateNeighbors (Vector2Int pos) {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        for (int x = pos.x - 1; x <= pos.x + 1; x++) {
            if (x < 0 || x >= gridSize) {
                continue;
            }
            for (int y = pos.y - 1; y <= pos.y + 1; y++) {
                if (y < 0 || y >= gridSize) {
                    continue;
                }
                if (x == pos.x && y == pos.y) {
                    continue;
                }
                neighbors.Add(new Vector2Int(x, y));
            }
        }

        return neighbors;
    }

    private int GetCost (Vector2Int pos) {
        GridBlock block = GridManager.instance.grid.GetBlock(pos.x, pos.y);

        if (block.building != null) {
            return (int)GridBlockType.Rock;
        }
      
        return (int)block.type;
    }

    private void CalculateIntegrationField (Vector2Int target) {
        ResetField();

        Queue<Vector2Int> openList = new Queue<Vector2Int>();

        integrationField[target.x, target.y] = 0;
        openList.Enqueue(target);

        while (openList.Count > 0) {
            Vector2Int curr = openList.Dequeue();
            List<Vector2Int> neighbors = CalculateNeighbors(curr);

            foreach (Vector2Int neighbor in neighbors) {
                int cost = integrationField[curr.x, curr.y] + GetCost(neighbor);

                if (cost < integrationField[neighbor.x, neighbor.y]) {
                    if (!openList.Contains(neighbor)) {
                        openList.Enqueue(neighbor);
                    }
                    integrationField[neighbor.x, neighbor.y] = cost;
                }
            }
        }
    }

    private int GetIntegrationFieldValue (Vector2Int pos) {
        if (pos.x < 0 || pos.x >= gridSize || pos.y < 0 || pos.y >= gridSize) {
            return int.MaxValue;
        }
        return integrationField[pos.x, pos.y];
    }

    private int SortByIntegrationValue (Vector2Int left, Vector2Int right) {
        int leftValue = GetIntegrationFieldValue(left);
        int rightValue = GetIntegrationFieldValue(right);

        if (leftValue < rightValue) {
            return -1;
        } else if (leftValue > rightValue) {
            return 1;
        } else {
            return 0;
        }
    }

    private void CalculateFlowField () {
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                Vector2Int curr = new Vector2Int(x, y);

                List<Vector2Int> neighbors = CalculateNeighbors(curr);
                neighbors.Sort(SortByIntegrationValue);

                Vector2Int closestNeighbor = neighbors[0];
                float closestDist = Vector2Int.Distance(curr, closestNeighbor);
                for (int i = 1; i < neighbors.Count; i++) {
                    if (GetIntegrationFieldValue(neighbors[i]) != GetIntegrationFieldValue(neighbors[0])) {
                        break;
                    }
                    float newDist = Vector2Int.Distance(curr, neighbors[i]);
                    if (newDist < closestDist) {
                        closestNeighbor = neighbors[i];
                        closestDist = newDist;
                    }
                }

                flowField[x, y] = closestNeighbor - curr;
            }
        }
    }

    public void Generate (Vector2 target) {
        CalculateIntegrationField(target.TruncateToInt());
        CalculateFlowField();
    }
    public Vector2 GetTargetDirection(Vector3 pos) {
        return GetTargetDirection(new Vector2(pos.x,pos.z));
    }

    public Vector2 GetTargetDirection (Vector2 pos) {
        Vector2Int posInt = Vector2Int.RoundToInt(pos);
        return flowField[posInt.x, posInt.y];
    }

    public int GetIntegration (Vector2 pos) {
        Vector2Int posInt = Vector2Int.FloorToInt(pos);
        return GetIntegrationFieldValue(posInt);
    }

    public int GetCostByPos (Vector2 pos) {
        Vector2Int posInt = Vector2Int.FloorToInt(pos);
        return GetCost(posInt);
    }
}
