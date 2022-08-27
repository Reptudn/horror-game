using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject[] items;

    public GameObject hand;


    int index = 0;
    int itemsInInventory = 0;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        Show(items[index]);
        //itemsInInventory = items.Length();
        
    }

    public GameObject[] GetInventoryItems(){
        return items;
    }

    public void AddItemToInventory(GameObject item){
        item.transform.position = hand.transform.position;
        item.transform.SetParent(hand.transform);

        items[itemsInInventory] = item;
        itemsInInventory++;

        Debug.Log("Item added to inventory: " + item.name);
    }

    public void RemoveItemFromInventory(GameObject item){
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0){ //scroll up
            if(index < items.Length) index++;
            Show(items[index]);
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0){ //scroll down
            if(index < items.Length && index > 0) index--;
            Show(items[index]);
        }
    }

    void Show(GameObject toShow){
        foreach(GameObject o in items){
            if(o == toShow) o.SetActive(true);
            else o.SetActive(false);
        }
    }

}
