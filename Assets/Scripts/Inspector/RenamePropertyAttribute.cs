using UnityEngine;

public class RenamePropertyAttribute : PropertyAttribute
{
    public string NewName { get ; private set; }    
    public RenamePropertyAttribute( string name )
    {
        NewName = name ;
    }
}