using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerBehavior {
    [HideInInspector]
    public PlayerStats stats;

    private void Start() {
        stats = GetComponent<PlayerStats>();
        if (networkObject.IsOwner) {
            stats.setup();
            networkObject.life = stats.getLife();
        }
    }

    #region RPC
    public override void DamageRPC(RpcArgs args) {
        float damage = args.GetNext<float>();
        stats.damage(damage);
    }
    #endregion

}