using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInventoryController : NetworkBehaviour
{
    
    public InventoryItem EquippedItem;
    [SyncVar(hook = nameof(UpdateInventoryContents))] public List<GameObject> Items;
    
    [Command]
    private void Cmd_UpdateInventoryContents(List<GameObject> NewValue)
    {
        UpdateInventoryContents(Items, NewValue);
    }

    public void UpdateInventoryContents(List<GameObject> OldValue, List<GameObject> NewValue)
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

    void _UpdateInventoryContents(List<GameObject> NewValue) { Items = NewValue; }

    public void ChangeInventoryContents(List<GameObject> NewValue) { Cmd_UpdateInventoryContents(NewValue); }

}
