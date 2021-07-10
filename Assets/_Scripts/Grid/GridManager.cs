using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [Header("Settings")]
    public int gridSize = 100;
    public Terrain.RiverSettings river;
    public Terrain.ResourceSettings forest;
    public Terrain.ResourceSettings rock;
    [Header("References")]
    public Transform gridBlockParent;
    [Header("Prefabs")]
    public List<GridBlockTypePrefab> gridBlocksPrefabs;

    [HideInInspector]
    public Grid grid;
    private Vector2[] borders;
    private Coroutine resetCO = null;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("[GridManager/Awake]: Singleton already exist");
            return;
        }
        instance = this;

        borders = new Vector2[]{ new Vector2(gridSize / 2, gridSize - 1), new Vector2(gridSize / 2, 0), new Vector2(0, gridSize / 2), new Vector2(gridSize - 1, gridSize / 2)};

        river.GenerateSeed();
        forest.GenerateSeed();
        rock.GenerateSeed();

        CreateGrid();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && resetCO == null) {
            resetCO = StartCoroutine(ResetMap());
        }
    }

    private IEnumerator ResetMap () {
        foreach (Transform child in gridBlockParent) {
            Destroy(child.gameObject);
        }
        Destroy(gridBlockParent.GetComponent<Collider>());

        yield return new WaitForEndOfFrame();

        CreateGrid();

        resetCO = null;
    }

    private void CreateGrid () {
        grid = new Grid(gridSize);

        GenerateGrass();

        SpawnGrid();

        GenerateRiver();

        GenerateResources();

        GenerateCollider();
    }

    private void GenerateGrass() {
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                GridBlock newBlock = new GridBlock(x, y, GridBlockType.Grass);
                grid.SetBlock(newBlock);
            }
        }
    }

    private void GenerateRiver () {
        Vector2 waterInit = new Vector2(UnityEngine.Random.Range(0, gridSize), UnityEngine.Random.Range(0, gridSize));

        float[] dists = { Vector2.Distance(waterInit, borders[0]), Vector2.Distance(waterInit, borders[1]), Vector2.Distance(waterInit, borders[2]), Vector2.Distance(waterInit, borders[3])};
        int borderIndex = Array.IndexOf(dists, dists.Max());

        Vector2 waterEnd = Vector2.zero;
        if (borders[borderIndex].x == 0f || borders[borderIndex].x == gridSize - 1) {
            waterEnd.x = borders[borderIndex].x;
        } else {
            waterEnd.x = UnityEngine.Random.Range(0, gridSize);
        }
        if (borders[borderIndex].y == 0f || borders[borderIndex].y == gridSize - 1) {
            waterEnd.y = borders[borderIndex].y;
        } else {
            waterEnd.y = UnityEngine.Random.Range(0, gridSize);
        }

        if (waterEnd.x < waterInit.x) {
            Vector2 tmp = waterInit;
            waterInit = waterEnd;
            waterEnd = tmp;
        }
        Debug.DrawLine(new Vector3(waterInit.x, 10f, waterInit.y), new Vector3(waterEnd.x, 10f, waterEnd.y), Color.blue, 900000f);

        Equation eq = new Equation(waterInit, waterEnd);

        float step = FindStep(eq, waterInit, waterEnd);

        for (float x = waterInit.x; x <= waterEnd.x; x += step) {
            Vector2 pos = eq.FindYWithNoise(x, river);
            Collider[] hits = Physics.OverlapSphere(new Vector3(pos.x, 0f, pos.y), river.thickness);
            

            foreach(Collider block in hits) {
                GridBlockPosition blockPos = block.gameObject.GetComponent<GridBlockPosition>();
                ChangeGridBlockType(blockPos.gridX, blockPos.gridY, GridBlockType.Water);
            }
        }
    }

    private void GenerateResources () {
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                float xF = (float)x / gridSize;
                float yF = (float)y / gridSize;

                GridBlock currBlock = grid.GetBlock(x, y);

                // Resources generation
                if (currBlock.type == GridBlockType.Grass) {
                    if (rock.IsNoiseGreaterThanThreshold(xF, yF)) {
                        ChangeGridBlockType(currBlock, GridBlockType.Rock);
                    } else if (forest.IsNoiseGreaterThanThreshold(xF, yF)) {
                        ChangeGridBlockType(currBlock, GridBlockType.Forest);
                    }
                }

            }
        }
    }

    private GameObject SpawnBlock (int x, int y, GridBlockType type) {
        GameObject prefab = GetPrefabByType(type);

        GameObject spawnedGO = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, gridBlockParent);
        GridBlockPosition pos = spawnedGO.AddComponent<GridBlockPosition>();
        pos.SetGridPosition(x, y);

        return spawnedGO;
    }

    private void SpawnGrid () {
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                GridBlock block = grid.GetBlock(x, y);
                block.SetGameObject(SpawnBlock(x, y, block.type));
            }
        }

    }

    private void GenerateCollider () {
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                GridBlock block = grid.GetBlock(x, y);

                Destroy(block.GetGameObject().GetComponent<Collider>());
            }
        }

        BoxCollider col = gridBlockParent.gameObject.AddComponent<BoxCollider>();
        Vector3 colSize = col.size;
        colSize.x = gridSize;
        colSize.z = gridSize;
        col.size = colSize;

        Vector3 colCenter = col.center;
        colCenter.x = (gridSize / 2f) - 0.5f;
        colCenter.z = (gridSize / 2f) - 0.5f;
        col.center = colCenter;
    }

    private void ChangeGridBlockType (GridBlock block) {
        ChangeGridBlockType(block, block.type);
    }

    private void ChangeGridBlockType (GridBlock block, GridBlockType type) {
        ChangeGridBlockType(block.x, block.y, type);
    }

    private void ChangeGridBlockType (int x, int y, GridBlockType type) {
        GridBlock currBlock = grid.GetBlock(x, y);

        if (currBlock.type == type) {
            return;
        }

        Destroy(currBlock.GetGameObject());
        currBlock.type = type;

        currBlock.SetGameObject(SpawnBlock(x, y, type));
    }

    private GameObject GetPrefabByType (GridBlockType type) {
        foreach(GridBlockTypePrefab obj in gridBlocksPrefabs) {
            if (obj.type == type) {
                return obj.prefab;
            }
        }

        return null;
    }

    private float FindStep (Equation eq, Vector2 init, Vector2 end) {
        Vector2 dir = (end - init).normalized;

        Vector2 step = init + (dir * river.density);

        return (step.x - init.x);
    }
}
