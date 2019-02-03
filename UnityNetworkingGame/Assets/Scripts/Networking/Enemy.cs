using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;

public class Enemy : EnemyBehavior {

    public float maxLife;
    float life;

    private void Start() {
        if (networkObject.IsOwner) {
            life = maxLife;
        }
    }

    private void Update() {
        if (networkObject.IsOwner) {

        }
        else {
            syncVal();
        }
    }

    void syncVal() {
        transform.position = networkObject.position;
        transform.rotation = networkObject.rotation;
        life = networkObject.life;
    }

    public void Damage(float damage) {
        networkObject.SendRpc(RPC_DAMAGE_R_P_C, Receivers.Owner, damage);
    }

    public override void DamageRPC(RpcArgs args) {
        float damage = args.GetNext<float>();
        Debug.Log("Taking " + damage + " damage");
        life -= damage;
        if (life <= 0f) {
            networkObject.Destroy();
            Destroy(gameObject);
        }
    }
}