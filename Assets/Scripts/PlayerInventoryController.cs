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
        if (isServer)
        {
            Items = NewValue;
        }
        if (isClient && (OldValue != NewValue))
        {
            _UpdateInventoryContents(NewValue);
        }
    }

    void _UpdateInventoryContents(List<InventoryItem> NewValue) { Items = NewValue; }

    public void ChangeInventoryContents(List<InventoryItem> NewValue) { Cmd_UpdateInventoryContents(NewValue); }

}
