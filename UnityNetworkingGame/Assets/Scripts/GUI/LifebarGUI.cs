using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarGUI : MonoBehaviour {

    public PlayerStats ps;

    public RectTransform mask;
    public Text numbers;

    float barMaxLenght;

    private void Start() {
        barMaxLenght = mask.rect.width;
    }

    void Update () {
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ps.getLifePercent() * barMaxLenght);
        numbers.text = ps.getLife() + " / " + ps.maxlife;
	}
}
