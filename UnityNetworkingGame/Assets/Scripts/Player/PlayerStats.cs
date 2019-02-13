using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

public class PlayerStats : MonoBehaviour {

    public float maxlife;
    float life;

    Player player;

    private void Start() {
        player = GetComponent<Player>();
    }

    private void Update() {
        if (!player.networkObject.IsOwner) {
            life = player.networkObject.life;
        }
    }

    public float getLifePercent() {
        return life/maxlife;
    }

    public float getLife() {
        return life;
    }

    public void setup() {
        this.life = maxlife;
    }

    public void damage(float damage) {
        if (player.networkObject.IsOwner) {
            applyDamage(damage);
        }
        else {
            player.networkObject.SendRpc(PlayerBehavior.RPC_DAMAGE_R_P_C, Receivers.Owner, damage);
        }
    }

    private void applyDamage(float damage) {
        Debug.Log("Taking " + damage + " damage");
        life -= damage;
        if (life <= 0f) {
            player.networkObject.Destroy();
            Destroy(gameObject);
        }
    }

}
