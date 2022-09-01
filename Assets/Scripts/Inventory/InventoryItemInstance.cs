using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryItemInstance : MonoBehaviour
{

    [Serializable]
    public struct ItemAttribute {

        [ReadOnlyProperty]
        public string Key;
        [HideInInspector]
        public InventoryItem.AttributeType Type;
        
        [EnumToggle(nameof(Type), true, (int) InventoryItem.AttributeType.Boolean)]
        [RenameProperty("Value")]
        public bool BooleanValue;
        [EnumToggle(nameof(Type), true, (int) InventoryItem.AttributeType.String)]
        [RenameProperty("Value")]
        public string StringValue;
        [EnumToggle(nameof(Type), true, (int) InventoryItem.AttributeType.Integer)]
        [RenameProperty("Value")]
        public int IntegerValue;

        public ItemAttribute(string Key, bool Default)
        {
            this.Key = Key;
            this.BooleanValue = Default;
            this.IntegerValue = -1;
            this.StringValue  = null;
            this.Type = InventoryItem.AttributeType.Boolean;
        }

        public ItemAttribute(string Key, int Default)
        {
            this.Key = Key;
            this.BooleanValue = false;
            this.IntegerValue = Default;
            this.StringValue  = null;
            this.Type = InventoryItem.AttributeType.Integer;
        }

        public ItemAttribute(string Key, string Default)
        {
            this.Key = Key;
            this.BooleanValue = false;
            this.IntegerValue = -1;
            this.StringValue  = Default;
            this.Type = InventoryItem.AttributeType.String;
        }

    }

    public InventoryItem Data;

    public PlayerObjectController Owner;
    
    public List<ItemAttribute> Attributes = new List<ItemAttribute>();

    private void Start() 
    {
        
        foreach(InventoryItem.ItemAttribute Attribute in Data.Attributes)
        {

            switch (Attribute.Type)
            {
                case InventoryItem.AttributeType.String:

                    Attributes.Add(new ItemAttribute(Attribute.Key, Attribute.DefaultString));
                    break;

                case InventoryItem.AttributeType.Integer:

                    Attributes.Add(new ItemAttribute(Attribute.Key, Attribute.DefaultInteger));
                    break;

                case InventoryItem.AttributeType.Boolean:

                    Attributes.Add(new ItemAttribute(Attribute.Key, Attribute.DefaultBoolean));
                    break;

            }

        }

    }

}
