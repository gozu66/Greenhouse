using UnityEngine;
using System.Collections;

public class Planter : Equipment {

    string itemName;

    void Update ()
    {
        _Update();  
    }

    public override void ToggleMoveable()
    {
        isMoveable = !isMoveable;
    }
}
