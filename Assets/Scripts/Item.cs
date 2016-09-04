using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    public int purchaseValue;
    public virtual void ToggleMoveable() { }
    public virtual void SeedInHand() { }
}
