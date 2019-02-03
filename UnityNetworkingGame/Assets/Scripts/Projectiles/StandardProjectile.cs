using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class StandardProjectile : MonoBehaviour {

    public float damage;

    Projectile p;

    private void Start() {
        p = GetComponent<Projectile>();
        if (!p.networkObject.IsOwner) {
            enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            Debug.Log("Hit Enemy!");
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
        }
        p.networkObject.Destroy();
        Destroy(gameObject);
    }



}
