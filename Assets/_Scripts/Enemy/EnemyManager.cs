using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public int numZombie;
    public float spawnDelay;
    public GameObject prefab;
    private FlowField flowField;
    private Coroutine waveCO = null;
    
    private void Awake() {
        if (instance != null)
        {
            Debug.LogWarning("[EnemyManager/Awake]: Singleton already exist");
            return;
        }
        instance = this;
    }

    void Start() {
        flowField = new FlowField(GridManager.instance.gridSize);
        flowField.Generate(new Vector2(50,50));
        SpawnWave((GridManager.Direction) Random.Range(0, 4));
    }

    void Update() {
        
    }

    public Vector2 GetTargetDirection(Vector3 pos){
        return flowField.GetTargetDirection(pos);
    }

    public void SpawnWave(GridManager.Direction news) {
        if (waveCO == null) {
            waveCO = StartCoroutine(SpawnEnemyCO(news));
        }
    }
    private IEnumerator SpawnEnemyCO(GridManager.Direction news) {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        
        for (int i = 0; i < numZombie; i++) {
            int rnd = Random.Range(0, GridManager.instance.gridSize);
            switch (news) {
                case GridManager.Direction.North: {
                        while (GridManager.instance.grid.GetBlock(rnd, GridManager.instance.gridSize - 1).type == GridBlockType.Rock) {
                            rnd = Random.Range(0, GridManager.instance.gridSize);
                        }
                        spawnPos = new Vector3(rnd, 0, GridManager.instance.gridSize - 1);
                        break;
                    }
                case GridManager.Direction.East: {
                        while (GridManager.instance.grid.GetBlock(GridManager.instance.gridSize - 1, rnd).type == GridBlockType.Rock) {
                            rnd = Random.Range(0, GridManager.instance.gridSize);
                        }
                        spawnPos = new Vector3(GridManager.instance.gridSize-1, 0, rnd);
                        break;
                    }
                case GridManager.Direction.West: {
                        while (GridManager.instance.grid.GetBlock(0, rnd).type == GridBlockType.Rock) {
                            rnd = Random.Range(0, GridManager.instance.gridSize);
                        }
                        spawnPos = new Vector3(0, 0, rnd);
                        break;
                    }
                case GridManager.Direction.South: {
                        while (GridManager.instance.grid.GetBlock(rnd, 0).type == GridBlockType.Rock) {
                            rnd = Random.Range(0, GridManager.instance.gridSize);
                        }
                        spawnPos = new Vector3(rnd, 0, 0);
                        break;
                    }
            }
            Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            yield return new WaitForSeconds(spawnDelay);
        }
        waveCO = null;
    }
}
