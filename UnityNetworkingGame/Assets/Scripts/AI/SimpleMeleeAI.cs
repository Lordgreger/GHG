using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BasicBehaviors;

public class SimpleMeleeAI : BehaviorUser {

    private void Start() {
        SortedList<int, Behavior> behaviors = new SortedList<int, Behavior>();
        behaviors.Add(1, new GoTowardsPlayer(this));
        behaviors.Add(2, new Idle());
        behaviorController = new BehaviorControllerPriority(behaviors);
    }

    private void Update() {
        behaviorController.update();
    }

    #region CustomBehaviors

    #endregion

}
