using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInventoryController : NetworkBehaviour
{
    
    public InventoryItem EquippedItem;
    [SyncVar(hook = nameof(UpdateInventoryContents))] public List<InventoryItem> Items;
    
    [Command]
    private void Cmd_UpdateInventoryContents(List<InventoryItem> NewValue)
    {
        UpdateInventoryContents(Items, NewValue);
    }

    public void UpdateInventoryContents(List<InventoryItem> OldValue, List<InventoryItem> NewValue)
    {
        Debug.Log("Updating Inventory Contents");
        Debug.Log(OldValue + ", " + NewValue);
        if (isServer)
        {
            Debug.Log("isServer");
            Items = NewValue;
        }
        if (isClient && (OldValue != NewValue))
        {
            Debug.Log("isClient");
            _UpdateInventoryContents(NewValue);
        }
    }

    void _UpdateInventoryContents(List<InventoryItem> NewValue) { Items = NewValue; }

    public void ChangeInventoryContents(List<InventoryItem> NewValue) { Cmd_UpdateInventoryContents(NewValue); }

}
