using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

[System.Serializable]
public class InventoryItem
{
    public string Name = "Deez Nuts";
    public GameObject Object;

    public InventoryItem(String Name, GameObject Object)
    {
        this.Name = Name;
        this.Object = Object;
    }
}

public static class InventoryItemWriteFunctions
{
    public static void WriteInventoryItem(this NetworkWriter writer, InventoryItem value)
    {
        writer.WriteString(value.Name);
        writer.WriteGameObject(value.Object);
    }
    public static InventoryItem ReadInventoryItem(this NetworkReader reader)
    {
       string Name = reader.ReadString();
       GameObject Object = reader.ReadGameObject();

        return new InventoryItem(Name, Object);
       
    }
}