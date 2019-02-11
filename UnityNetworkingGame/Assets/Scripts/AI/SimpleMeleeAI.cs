using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BasicBehaviors;

public class SimpleMeleeAI : BehaviorUser {

    private void Start() {
        SortedList<int, Behavior> behaviors = new SortedList<int, Behavior>();
        behaviors.Add(1, new GoTowardsPlayer(this, 4));
        behaviors.Add(2, new Idle());
        behaviorController = new BehaviorControllerPriority(behaviors);
    }

    private void Update() {
        behaviorController.update();
    }

    #region CustomBehaviors
    public class Attack : Behavior {
        BehaviorUser user;
        float attackDistance;

        public Attack(BehaviorUser user, float attackDistance) {
            this.user = user;
            this.attackDistance = attackDistance;
        }

        public bool evaluate() {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return false;

            float dist = FunctionLib.distanceTo(user.transform.position, player.transform.position);
            if (dist < attackDistance) {
                return true;
            }
            return false;
        }

        public void start() {
            (user.behaviorController as BehaviorControllerPriority).behaviorLocked = true;
        }

        public void stop() {
            throw new System.NotImplementedException();
        }

        public void update() {
            throw new System.NotImplementedException();
        }
    }


    #endregion

}
