using UnityEngine;
using System.Collections;

public class Grid  {

    private int gridSize;
    private GridBlock[,] grid;

    public Grid (int gridSize) {
        this.gridSize = gridSize;
        grid = new GridBlock[gridSize, gridSize];
    }

    public void SetBlock (GridBlock block) {
        if (block.x < 0 || block.x >= gridSize) {
            return;
        }
        if (block.y < 0 || block.y >= gridSize) {
            return;
        }

        grid[block.x, block.y] = block;
    }

    public GridBlock GetBlock (int x, int y) {
        if (x < 0 || x >= gridSize) {
            return null;
        }
        if (y < 0 || y >= gridSize) {
            return null;
        }

        return grid[x, y];
    }
}
