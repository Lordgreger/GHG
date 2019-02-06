using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BasicBehaviors {
    public class GoTowardsPlayer : Behavior {
        BehaviorUser user;
        NavMeshAgent nav;
        GameObject player = null;

        public GoTowardsPlayer(BehaviorUser user) {
            this.user = user;
            nav = user.GetComponent<NavMeshAgent>();
        }

        float distanceToPlayer() {
            playerCheck();
            return (user.transform.position - player.transform.position).magnitude;
        }

        void updatePlayer() {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void playerCheck() {
            if (player == null) {
                updatePlayer();
            }
        }

        public bool evaluate() {
            if (distanceToPlayer() > 5) {
                return true;
            }
            return false;
        }

        public void start() {
            MonoBehaviour.print("Started moving towards player");
            nav.isStopped = false;
        }

        public void stop() {
            MonoBehaviour.print("Stopped moving towards player");
            nav.SetDestination(user.transform.position);
            nav.isStopped = true;
        }

        public void update() {
            playerCheck();
            nav.SetDestination(player.transform.position);
        }
    }

    public class Idle : Behavior {
        public bool evaluate() {
            return true;
        }

        public void start() {
            MonoBehaviour.print("Started doing nothing");
        }

        public void stop() {
            MonoBehaviour.print("Stopped doing nothing");
        }

        public void update() {

        }
    }


}
