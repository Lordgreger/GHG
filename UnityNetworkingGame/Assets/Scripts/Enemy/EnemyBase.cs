using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour {
    FSM fsm = new FSM();
    NavMeshAgent navAgent;
    Enemy enemy;

    public virtual void Start() {
        enemy = GetComponent<Enemy>();
        if (!enemy.networkObject.IsOwner) {
            enabled = false;
        }

        navAgent = GetComponent<NavMeshAgent>();
    }

    public virtual void Update() {
        fsm.Update();
    }
}