using UnityEngine;
using System.Collections;

public class GridBlockPosition : MonoBehaviour {

    public int gridX;
    public int gridY;

    public void SetGridPosition (int x, int y) {
        gridX = x;
        gridY = y;
    }
}
