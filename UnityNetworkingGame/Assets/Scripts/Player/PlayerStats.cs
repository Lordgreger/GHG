using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float maxlife;
    float life;

    private void Start() {
        life = maxlife;
    }

    public float getLifePercent() {
        return life/maxlife;
    }

    public float getLife() {
        return life;
    }

}
