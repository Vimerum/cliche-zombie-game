using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour {
    public enum State {
        InGame = 0,
        GameOver = 1,
    }

    public static GameManager instance;

    [Header("Settings")]
    public float respawnTime;
    [Header("References")]
    public GameObject gameOverCanvas;
    public CameraController cameraController;
    public GameObject minimap;
    [Header("Prefabs")]
    public GameObject playerPrefab;

    [ReadOnly]
    public State state;
    private GameObject player;
    private Coroutine playerRespawnCoroutine;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Time.timeScale = 1;
        instance = this;
        gameOverCanvas.SetActive(false);
    }

    private void Start() {
        state = State.InGame;
        Begin();
    }

    private void Update() {
        if (Input.GetButtonDown("Minimap")) {
            minimap.SetActive(!minimap.activeSelf);
        }
    }

    private void Begin () {
        SpawnPlayer();
    }

    private void SpawnPlayer () {
        player = Instantiate(playerPrefab, GetPlayerSpawnPosition(), Quaternion.identity);
        cameraController.target = player.transform;
    }

    private IEnumerator RespawnPlayerCO () {
        cameraController.target = BuildingManager.instance.GetMainBaseTransform();
        yield return new WaitForSeconds(respawnTime);
        SpawnPlayer();
        playerRespawnCoroutine = null;
    }

    private Vector3 GetPlayerSpawnPosition () {
        if (!BuildingManager.instance.HasMainBase()) {
            Vector3 pos = new Vector3(Random.Range(0, GridManager.instance.gridSize), 0, Random.Range(0, GridManager.instance.gridSize));
            while (GridManager.instance.grid.GetBlock(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)).type == GridBlockType.Rock) {
                pos = new Vector3(Random.Range(0, GridManager.instance.gridSize), 0, Random.Range(0, GridManager.instance.gridSize));
            }
            return pos;
        }

        return BuildingManager.instance.GetMainBaseTransform().position + new Vector3(-0.5f, 0f, -0.5f);
    }

    public void RespawnPlayer () {
        if (playerRespawnCoroutine == null) {
            playerRespawnCoroutine = StartCoroutine(RespawnPlayerCO());
        }
    }

    public void BeginGameOver () {
        state = State.GameOver;
    }

    public void GameOver () {
        minimap.SetActive(false);
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void LoadMainMenu () {
        SceneManager.LoadScene(0);
    }
}
