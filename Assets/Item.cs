using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName { get; private set; }
    public int quantity { get; private set; } = 0;

    public Item(string itemName)
    {
        this.itemName = itemName;
    }

    public void AddQuantity(int number)
    {
        quantity += number;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
