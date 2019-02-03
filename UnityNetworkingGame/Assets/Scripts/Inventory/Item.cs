using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
    int id;
    string name;
    string modelName;

    public Item(int id, string name, string modelName) {
        this.id = id;
        this.name = name;
        this.modelName = modelName;
    }
}
