using UnityEngine;
using System.Collections;

public class ResourcesBehaviour : MonoBehaviour {

    [Header("Settings")]
    public int rate;
    public Resource resource;

    private float cooldown = -1f;

    private void Update() {
        if (cooldown <= 0) {
            ResourcesManager.instance.Deposit(resource, rate);
            cooldown = 1f;
        } else {
            cooldown -= Time.deltaTime;
        }
    }
}
