using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class PlayerShooting : MonoBehaviour {

    public Transform spawnPos;
    public float delay;
    bool canShoot;
    

    private void Start() {
        if (!GetComponent<Player>().networkObject.IsOwner) {
            this.enabled = false;
            return;
        }
        canShoot = true;
    }

    private void Update() {
        if (canShoot) {
            if (Input.GetMouseButton(0)) {
                NetworkManager.Instance.InstantiateProjectile(0, spawnPos.position, spawnPos.rotation);
                canShoot = false;
                StartCoroutine(shootingDelay());
            }
        }

    }

    IEnumerator shootingDelay() {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

}
