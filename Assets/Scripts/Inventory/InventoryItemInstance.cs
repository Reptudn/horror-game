using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

[Serializable]
public class InventoryItemInstance
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

        public ItemAttribute(ItemAttribute Attribute)
        {
            this.Key = Attribute.Key;
            this.Type = Attribute.Type;
            this.StringValue = Attribute.StringValue;
            this.IntegerValue = Attribute.IntegerValue;
            this.BooleanValue = Attribute.BooleanValue;
        }

    }

    [System.NonSerialized]
    public PlayerObjectController Owner;

    public InventoryItem Data;

    public List<ItemAttribute> Attributes = new List<ItemAttribute>();

    public InventoryItemInstance(InventoryItem Data) { this.Data = Data; }

    public void Awake() 
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

    public void SetAttribute(string Key, bool Value)
    {
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (Attributes[i].Key.Equals(Key)) 
            { 

                ItemAttribute Attribute = new ItemAttribute(Attributes[i]);
                Attribute.BooleanValue = Value;
                Attributes.Remove(Attributes[i]);
                Attributes.Add(Attribute);
                break; 
            }
        }
    }

    public void SetAttribute(string Key, string Value)
    {
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (Attributes[i].Key.Equals(Key)) 
            { 

                ItemAttribute Attribute = new ItemAttribute(Attributes[i]);
                Attribute.StringValue = Value;
                Attributes.Remove(Attributes[i]);
                Attributes.Add(Attribute);
                break; 
            }
        }
    }

    public void SetAttribute(string Key, int Value)
    {
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (Attributes[i].Key.Equals(Key)) 
            { 

                ItemAttribute Attribute = new ItemAttribute(Attributes[i]);
                Attribute.IntegerValue = Value;
                Attributes.Remove(Attributes[i]);
                Attributes.Add(Attribute);
                break; 
            }
        }
    }

}

public static class InventoryItemInstanceReadWriteFunctions
{
    public static void WriteInventoryItemInstance(this NetworkWriter writer, InventoryItemInstance value)
    {

        writer.WriteString("Items/" + value.Data.name);
        writer.WriteArray(value.Attributes.ToArray());
    }

    public static InventoryItemInstance ReadInventoryItemInstance(this NetworkReader reader)
    {

        string SOPath = reader.ReadString();
        InventoryItemInstance ItemInstance = new InventoryItemInstance(Resources.Load<InventoryItem>(SOPath));

        InventoryItemInstance.ItemAttribute[] ItemAttributes = reader.ReadArray<InventoryItemInstance.ItemAttribute>();
        ItemInstance.Attributes = new List<InventoryItemInstance.ItemAttribute>();

        foreach (InventoryItemInstance.ItemAttribute Attribute in ItemAttributes)
        {

            ItemInstance.Attributes.Add(Attribute);

        }

        return ItemInstance;
    }
}

public static class InventoryItemInstanceAttributeReadWriteFunctions
{
    public static void WriteInventoryItemInstanceAttribute(this NetworkWriter writer, InventoryItemInstance.ItemAttribute value)
    {

        writer.WriteString(value.Key);
        writer.WriteInt((int) value.Type);

        writer.WriteBool(value.BooleanValue);
        writer.WriteString(value.StringValue);
        writer.WriteInt(value.IntegerValue);
    }

    public static InventoryItemInstance.ItemAttribute ReadInventoryItemInstanceAttribute(this NetworkReader reader)
    {
        InventoryItemInstance.ItemAttribute ItemAttribute = new InventoryItemInstance.ItemAttribute();

        ItemAttribute.Key = reader.ReadString();
        ItemAttribute.Type = (InventoryItem.AttributeType) reader.ReadInt();

        ItemAttribute.BooleanValue = reader.ReadBool();
        ItemAttribute.StringValue = reader.ReadString();
        ItemAttribute.IntegerValue = reader.ReadInt();

        return ItemAttribute;
    }
}


