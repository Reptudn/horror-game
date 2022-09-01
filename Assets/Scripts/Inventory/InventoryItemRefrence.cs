using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemRefrence : MonoBehaviour
{
    public InventoryItemInstance Item;

    private void Awake() {
        
        Item.Awake();

    }

}
