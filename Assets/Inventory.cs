using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int currency { get; private set; } = 0;
    public List<Item> items = new List<Item> ();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item item)
    {
        foreach(Item thing in items)
        {
            if (item.itemName == thing.itemName)
            {
                thing.AddQuantity(1);
            }
            else
            {
                items.Add(item);
            }
        }
    }
}
