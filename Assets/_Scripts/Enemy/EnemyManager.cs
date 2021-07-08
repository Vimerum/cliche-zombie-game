using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public int numZombie;
    public GameObject prefab;
    private FlowField flowField;
    
    private void Awake() {
        if (instance != null)
        {
            Debug.LogWarning("[EnemyManager/Awake]: Singleton already exist");
            return;
        }
        instance = this;
    }

    void Start() {
        for(int i = 0; i < numZombie; i++) {
            int posX = UnityEngine.Random.Range(0, GridManager.instance.gridSize);
            Instantiate(prefab, new Vector3(posX, 0, 0),Quaternion.identity, transform);
        }
        flowField = new FlowField(GridManager.instance.gridSize);
        flowField.Generate(new Vector2(50,50));
    }

    void Update() {
        
    }

    public Vector2 GetTargetDirection(Vector3 pos){
        return flowField.GetTargetDirection(pos);
    }
}
