using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [Header("Settings")]
    public int gridSize = 10;
    public float terrainNoiseScale = 1f;
    public float resourcesNoiseScale = 1f;
    public float terrainAmplitude = 2f;
    public float waterThreshold = 0.2f;
    public float forestThreshold = 0.8f;
    public float rockThreshold = 0.2f;
    public float terrainSeed;
    public float resourcesSeed;
    [Header("References")]
    public Transform gridBlockParent;
    [Header("Prefabs")]
    public List<GridBlockTypePrefab> gridBlocksPrefabs;

    private Grid grid;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("[GridManager/Awake]: Singleton already exist");
            return;
        }
        instance = this;

        terrainSeed = Random.Range(-1000f, 1000f);
        resourcesSeed = Random.Range(-1000f, 1000f);

        CreateGrid();
        SpawnGrid();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            terrainSeed = Random.Range(-1000f, 1000f);
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            resourcesSeed = Random.Range(-1000f, 1000f);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            foreach (Transform child in gridBlockParent) {
                Destroy(child.gameObject);
            }

            CreateGrid();
            SpawnGrid();
        }
    }

    private void CreateGrid () {
        grid = new Grid(gridSize);

        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                float xF = (float)x / gridSize;
                float yF = (float)y / gridSize;

                GridBlockType type = GridBlockType.None;

                float terrainNoise = Mathf.PerlinNoise((xF * terrainNoiseScale) + terrainSeed, (yF * terrainNoiseScale) + terrainSeed) * terrainAmplitude;
                if (terrainNoise < waterThreshold) {
                    type = GridBlockType.Water;
                } else {
                    type = GridBlockType.Grass;

                    float resourcesNoise = Mathf.PerlinNoise((xF * resourcesNoiseScale) + resourcesSeed, (yF * resourcesNoiseScale) + resourcesSeed);
                    if (resourcesNoise > forestThreshold) {
                        type = GridBlockType.Forest;
                    } else if (resourcesNoise < rockThreshold) {
                        type = GridBlockType.Rock;
                    }
                }

                GridBlock newBlock = new GridBlock(x, y, type);

                grid.SetBlock(newBlock);
            }
        }
    }

    private void SpawnGrid () {
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                GridBlock block = grid.GetBlock(x, y);
                GameObject prefab = GetPrefabByType(block.type);

                GameObject spawnedGO = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, gridBlockParent);
                block.SetGameObject(spawnedGO);
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

    private GameObject GetPrefabByType (GridBlockType type) {
        foreach(GridBlockTypePrefab obj in gridBlocksPrefabs) {
            if (obj.type == type) {
                return obj.prefab;
            }
        }

        return null;
    }
}
