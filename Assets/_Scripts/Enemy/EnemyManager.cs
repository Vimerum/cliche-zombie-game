using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager instance;

    [Header("Settings")]
    public float waveTimeout;
    public int minNumZombies;
    public int maxNumZombies;
    public int numZombiesStep;
    public float spawnDelay;
    [Header("References")]
    public GameObject prefab;
    public TextMeshProUGUI SpawnAlert;

    [HideInInspector]
    public FlowField flowField;

    private int numZombies;
    private Coroutine waveCO = null;
    private Coroutine waveSpawnCO = null;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("[EnemyManager/Awake]: Singleton already exist");
            return;
        }
        instance = this;

        numZombies = minNumZombies;
    }

    void Start() {
        flowField = new FlowField(GridManager.instance.gridSize);
        SpawnAlert.transform.parent.gameObject.SetActive(false);
    }

    private void Update() {
        if (GameManager.instance.state == GameManager.State.GameOver) {
            StopAllCoroutines();
            SpawnAlert.transform.parent.gameObject.SetActive(false);
            return;
        }
        if (waveCO == null) {
            if (BuildingManager.instance.HasMainBase()) {
                waveCO = StartCoroutine(WaveCO());
            }
        }
    }

    public Vector2 GetTargetDirection(Vector3 pos) {
        return flowField.GetTargetDirection(pos);
    }

    private IEnumerator WaveCO() {
        while (true) {
            GridManager.Direction rndSpawn = (GridManager.Direction)Random.Range(0, 4);
            yield return new WaitUntil(() => waveSpawnCO == null);
            yield return new WaitForSeconds(waveTimeout / 2);
            EnemySpawnAlert(rndSpawn);
            yield return new WaitForSeconds(waveTimeout / 2);
            waveSpawnCO = StartCoroutine(SpawnEnemyCO(rndSpawn));
        }
    }

    private IEnumerator SpawnAlertTimerCO() {
        SpawnAlert.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        SpawnAlert.transform.parent.gameObject.SetActive(false);
    }

    private void EnemySpawnAlert(GridManager.Direction news) {
        SpawnAlert.text = ("Uma onda aparecer? do " + DirectionToPortuguese(news.ToString()));
        StartCoroutine(SpawnAlertTimerCO());
    }

    private IEnumerator SpawnEnemyCO(GridManager.Direction news) {
        Vector3 spawnPos = new Vector3(0, 0, 0);

        for (int i = 0; i < numZombies; i++) {
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
                        spawnPos = new Vector3(GridManager.instance.gridSize - 1, 0, rnd);
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
        numZombies += numZombiesStep;
        numZombies = Mathf.Min(numZombies, maxNumZombies);

        waveSpawnCO = null;
    }

    private string DirectionToPortuguese(string news) {
        if(news == "North") {
            return "Norte";
        }else if(news == "West"){
            return "Oeste";
        }else if(news == "East"){
            return "Leste";
        }else {
            return "Sul";
        }
    }
}
