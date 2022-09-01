using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using Mirror;

[System.Serializable]
[CreateAssetMenu(menuName="Inventory/Item")]
public class InventoryItem : ScriptableObject
{

    [Serializable]
    public enum AttributeType {
        String,
        Integer,
        Boolean
    }

    [Serializable]
    public struct ItemAttribute {

        public string Key;
        public AttributeType Type;
        
        [EnumToggle(nameof(Type), true, (int) AttributeType.Boolean)]
        [RenameProperty("Default Value")]
        public bool DefaultBoolean;
        [EnumToggle(nameof(Type), true, (int) AttributeType.String)]
        [RenameProperty("Default Value")]
        public string DefaultString;
        [EnumToggle(nameof(Type), true, (int) AttributeType.Integer)]
        [RenameProperty("Default Value")]
        public int DefaultInteger;

    }

    public string Id;
    public string DisplayName;
    public GameObject Prefab;

    public List<ItemAttribute> Attributes = new List<ItemAttribute>();

}