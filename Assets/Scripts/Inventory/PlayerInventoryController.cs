using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInventoryController : NetworkBehaviour
{
    
    public int EquippedIndex = -1;
    public InventoryItem EquippedItem;

    [SyncVar(hook = nameof(UpdateInventoryContents))] public List<InventoryItemInstance> Items;
    
    public void AddItem (InventoryItemInstance Item) 
    {

        List<InventoryItemInstance> Items = new List<InventoryItemInstance>(this.Items);
        Items.Add(Item);
        ChangeInventoryContents(Items);

    }

    public void RemoveItem (InventoryItemInstance Item) 
    {

        this.Items.Remove(Item);

    }

    private void Update() {
        
        if (hasAuthority)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (EquippedIndex < Items.Count - 1) EquippedIndex++;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (EquippedIndex > -1) EquippedIndex--;
            }
        }

    }

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
