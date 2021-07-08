using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

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
        flowField = new FlowField(GridManager.instance.gridSize);
        flowField.Generate(new Vector2(50,50));
    }

    void Update() {
        
    }

    public Vector2 GetTargetDirection(Vector3 pos){
        return flowField.GetTargetDirection(pos);
    }
}
