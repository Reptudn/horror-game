using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInventoryController : NetworkBehaviour
{
    
    [Header("Equipped")]
    [ReadOnlyProperty()]
    [SyncVar(hook = nameof(UpdateEquippedItemIndex))] public int EquippedIndex = -1;
    public InventoryItemInstance EquippedItem;

    [Header("Settings")]
    public int InventorySize = 5;
    public KeyCode PickupKey = KeyCode.E;
    [Range(1,10)]
    public float MaxPickupDistance = 6f;
    public string ItemTag = "Pickup";
    public string EquippedItemTag = "EquippedItem";

    [Header("Components")]
    [RenameProperty("Animator")]
    public Animator PlayerAnimator;
    public GameObject ItemAnchor;

    [Header("Inventory Content")]
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
                if (EquippedIndex < Items.Count - 1) { SetEquippedIndex(EquippedIndex += 1); }
                else if (EquippedIndex == Items.Count - 1) { SetEquippedIndex(-1);  }
                UpdateEquippedItem();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (EquippedIndex > -1) { SetEquippedIndex(EquippedIndex -= 1); }
                else if (EquippedIndex == -1) { SetEquippedIndex(Items.Count - 1); }
                UpdateEquippedItem();
            }

            if (Input.GetKeyDown(PickupKey)){

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {

                    if (hit.transform.gameObject.tag == ItemTag && hit.distance <= MaxPickupDistance) 
                    {

                        if (hit.transform.gameObject.GetComponent<InventoryItemRefrence>() != null)
                        {

                            AddItem(hit.transform.gameObject.GetComponent<InventoryItemRefrence>().Item);
                            Destroy(hit.transform.gameObject);
                            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red, 5);

                        }
                    }
                }     
            }
        }
    }

    private void UpdateEquippedItem()
    {
        foreach (Transform child in ItemAnchor.transform) { GameObject.Destroy(child.gameObject); }
        if (EquippedIndex >= 0)
        {
            EquippedItem = Items[EquippedIndex];
            GameObject Item = Instantiate(EquippedItem.Data.Prefab);
            Item.transform.SetParent(ItemAnchor.transform);
            Item.transform.localPosition = new Vector3(0f, 0f, 0f);
            Item.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            Item.transform.tag = EquippedItemTag;
            ToggleHoldingAnimation(true);
        }
        else if (EquippedIndex == -1)
        {
            EquippedItem = null;
            ToggleHoldingAnimation(false);
        }
    }

    private void ToggleHoldingAnimation(bool State)
    {
        Debug.Log("Holding Item: " + State + ", " + (State ? 1f : 0f).ToString());
        PlayerAnimator.SetBool("holdingItem", State);
        PlayerAnimator.SetLayerWeight(4, State ? 1f : 0f);
    }

    public List<InventoryItemInstance> GetDereferencedItemList() { return new List<InventoryItemInstance>(Items); }

    // Networking

    [Command]
    private void Cmd_UpdateInventoryContents(List<InventoryItemInstance> NewValue) { UpdateInventoryContents(Items, NewValue); }

    [Command]
    private void Cmd_SetEquippedIndex(int NewValue) { UpdateEquippedItemIndex(EquippedIndex, NewValue); }

    // Update Handlers

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

    public void UpdateEquippedItemIndex(int OldValue, int NewValue)
    {
        if (isServer)
        {
            EquippedIndex = NewValue;
        }
        if (isClient && (OldValue != NewValue))
        {
            _UpdateEquippedItemIndex(NewValue);
        }
    }

    // Network Sync

    public void TriggerNetworkSync() 
    { 
        ChangeInventoryContents(new List<InventoryItemInstance>(Items)); 
        if (EquippedIndex != -1){ this.EquippedItem = this.Items[EquippedIndex]; } 
        else { this.EquippedItem = null; }}

    // Private Secondary Update Handlers

    void _UpdateInventoryContents(List<InventoryItemInstance> NewValue) { Items = NewValue; }

    void _UpdateEquippedItemIndex(int NewValue) { EquippedIndex = NewValue; }

    public void ChangeInventoryContents(List<InventoryItemInstance> NewValue) { Cmd_UpdateInventoryContents(NewValue); }

    public void SetEquippedIndex(int Index) { Cmd_SetEquippedIndex(Index); }

}
