using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderContainer : MonoBehaviour {

    public string tagToCollect;

    List<GameObject> objs = new List<GameObject>();

    private void OnTriggerEnter(Collider other) {
        if (other.tag == tagToCollect) {
            if (!objs.Contains(other.gameObject)) {
                objs.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            if (objs.Contains(other.gameObject)) {
                objs.Remove(other.gameObject);
            }
        }
    }

    public List<GameObject> getContainedObjects() {
        return objs;
    }
}
