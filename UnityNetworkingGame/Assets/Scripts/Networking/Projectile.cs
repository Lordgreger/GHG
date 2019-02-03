using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : ProjectileBehavior {
    public float speed;
    public float lifetime;

    private void Start() {
        if (networkObject.IsOwner) {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
            networkObject.Destroy((int)(lifetime * 1000));
        }
    }

    private void Update() {
        if (networkObject.IsOwner) {

        }
        else {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
        }
    }

}
