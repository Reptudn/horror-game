using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

[System.Serializable]
[CreateAssetMenu(menuName="Inventory/Item")]
public class InventoryItem : ScriptableObject
{

    public enum AttributeType {
        String,
        Integer,
        Boolean
    }

    [Serializable]
    public struct ItemAttribute {

        public string Key;
        public AttributeType Type;

    }

    public string Id;
    public string DisplayName;
    public GameObject Prefab;

    public List<ItemAttribute> Attributes = new List<ItemAttribute>();

}


/*
public static class InventoryItemWriteFunctions
{
    public static void WriteInventoryItem(this NetworkWriter writer, InventoryItem value)
    {
        writer.WriteString(value.Name);
        writer.WriteGameObject(value.Prefab);
    }
    public static InventoryItem ReadInventoryItem(this NetworkReader reader)
    {
       string Name = reader.ReadString();
       GameObject Object = reader.ReadGameObject();

        return new InventoryItem(Name, Object);
       
    }
}
*/