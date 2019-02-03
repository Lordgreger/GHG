using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerBehavior {

    List<GameObject> destroyIfNotLocalPlayer = new List<GameObject>();

    private void Start() {
        foreach (var g in destroyIfNotLocalPlayer) {
            Destroy(g);
        }
    }
}
