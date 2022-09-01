using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInventoryController : NetworkBehaviour
{
    
    public InventoryItem EquippedItem;
    [SyncVar(hook = nameof(UpdateInventoryContents))] public List<InventoryItemInstance> Items;
    
    [Command]
    private void Cmd_UpdateInventoryContents(List<InventoryItemInstance> NewValue)
    {
        UpdateInventoryContents(Items, NewValue);
    }

    public void UpdateInventoryContents(List<InventoryItemInstance> OldValue, List<InventoryItemInstance> NewValue)
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

    void _UpdateInventoryContents(List<InventoryItemInstance> NewValue) { Items = NewValue; }

    public void ChangeInventoryContents(List<InventoryItemInstance> NewValue) { Cmd_UpdateInventoryContents(NewValue); }

}
