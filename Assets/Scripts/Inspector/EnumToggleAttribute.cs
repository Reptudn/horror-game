using UnityEngine;
using System;
using System.Collections;
 
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class EnumToggleAttribute : PropertyAttribute
{
    //The name of the bool field that will be in control
    public string ConditionalSourceField = "";
    //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public bool HideInInspector = false;
    public int WantedEnum;
 
    public EnumToggleAttribute(string conditionalSourceField, bool hideInInspector, int wantedEnum)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.WantedEnum = wantedEnum;
    }


}