using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlockBehaviour : MonoBehaviour {
    [Header("Rotation")]
    public List<GameObject> objectsToSpin;

    [Header("Position")]
    public List<GameObject> objectsToPosition;

    [Header("Deletion")]
    [Range(0f, 1f)]
    public float chanceToDelete;
    public List<GameObject> objectsToDelete;

    [Header("Pick n")]
    public int numberToKeep;
    public List<GameObject> objectsToPickN;

    private void Awake() {
        SpinObjects();
        PositionObjects();
        DeleteObjects();
        PickN();
    }

    private void SpinObjects () {
        foreach(GameObject obj in objectsToSpin) {
            float randomSpin = Random.Range(0f, 360f);
            Vector3 eulerRotation = obj.transform.eulerAngles;
            Quaternion rotation = Quaternion.Euler(eulerRotation.x, randomSpin, eulerRotation.z);

            obj.transform.rotation = rotation;
        }
    }

    private void PositionObjects () {
        foreach(GameObject obj in objectsToPosition) {
            Vector3 randomPosition = new Vector3(Random.Range(-0.5f, 0.5f), obj.transform.position.y, Random.Range(-0.5f, 0.5f));

            obj.transform.localPosition = randomPosition;
        }
    }

    private void DeleteObjects () {
        foreach(GameObject obj in objectsToDelete) {
            float rnd = Random.Range(0f, 1f);
            if (rnd <= chanceToDelete) {
                Destroy(obj);
            }
        }
    }

    private void PickN () {
        while (objectsToPickN.Count > numberToKeep) {
            int rndIndex = Random.Range(0, objectsToPickN.Count);
            Destroy(objectsToPickN[rndIndex]);
            objectsToPickN.RemoveAt(rndIndex);
        }
    }
}
