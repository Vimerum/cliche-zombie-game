using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource {
    Wood = 0,
    Stone = 1,
}

public class ResourcesManager : MonoBehaviour {

    public static ResourcesManager instance;

    [Header("Resources")]
    public List<ResourceParameter> resources;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start() {
        Initialize();
    }

    private void Initialize () {
        foreach (ResourceParameter param in resources) {
            param.Initialize();
        }
    }

    public void Deposit (Resource resource, int value) {
        resources.GetParameter(resource).Deposit(value);
    }

    public bool CheckWithdraw(Resource resource, int value) {
        return resources.GetParameter(resource).CheckWithdraw(value);
    }

    public void Withdraw(Resource resource, int value) {
        resources.GetParameter(resource).Withdraw(value);
    }
}
