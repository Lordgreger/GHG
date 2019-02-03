using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackableItem : Item {
    int maxStackSize;
    int amount;

    public StackableItem(int id, string name, string modelName, int maxStackSize, int amount) : base(id, name, modelName) {
        this.maxStackSize = maxStackSize;
        this.amount = amount;
    }
}
