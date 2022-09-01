using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

[Serializable]
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

    [System.NonSerialized]
    public PlayerObjectController Owner;

    public InventoryItem Data;

    [SerializeField]
    public List<ItemAttribute> Attributes = new List<ItemAttribute>();

    private void Awake() 
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

public static class InventoryItemInstanceReadWriteFunctions
{
    public static void WriteInventoryItemInstance(this NetworkWriter writer, InventoryItemInstance value)
    {
        writer.WriteString(value.name);
        writer.WriteArray(value.Attributes.ToArray());
    }

    public static InventoryItemInstance ReadInventoryItemInstance(this NetworkReader reader)
    {
        InventoryItemInstance ItemInstance = new InventoryItemInstance();
        ItemInstance.Data = Resources.Load<InventoryItem>(reader.ReadString());

        InventoryItemInstance.ItemAttribute[] ItemAttributes = reader.ReadArray<InventoryItemInstance.ItemAttribute>();
        ItemInstance.Attributes = new List<InventoryItemInstance.ItemAttribute>();

        foreach (InventoryItemInstance.ItemAttribute Attribute in ItemAttributes)
        {

            ItemInstance.Attributes.Add(Attribute);

        }

        return ItemInstance;
    }
}


