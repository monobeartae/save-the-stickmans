using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public event EventHandler OnItemListChanged;
    List<Item> itemList;
    Action<Item> useItemAction;
    int amount;

    public PlayerInventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
        //AddItem(new Item { itemType = Item.Type.HealthPotion, amt = 1 });
        //AddItem(new Item { itemType = Item.Type.Currency, amt = 1 });
        //AddItem(new Item { itemType = Item.Type.HealthPotion, amt = 1 });
        //AddItem(new Item { itemType = Item.Type.HealthPotion, amt = 1 });
        //AddItem(new Item { itemType = Item.Type.Gun, amt = 1 });
        //AddItem(new Item { itemType = Item.Type.Eraser, amt = 1 });
    }

    public void AddUseItemAction(Action<Item> action)
    {
        useItemAction += action;
    }

    public void AddItem(Item item)
    {
        if (item.isStackable())
        {
            bool itemAlrInInven = false;
            foreach (Item invenItem in itemList)
            {
                if (invenItem.itemType == item.itemType)
                {
                    invenItem.amt += 1;
                    itemAlrInInven = true;
                }
                if (invenItem.itemType == Item.Type.Currency)
                {
                    amount = invenItem.amt;
                    Debug.Log("AA: " + amount);
                }
            }
            if (!itemAlrInInven)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.isStackable())
        {
            Item itemInInven = null;
            foreach (Item invenItem in itemList)
            {
                if (invenItem.itemType == item.itemType)
                {

                    Debug.Log("Before: " + invenItem.amt);
                    invenItem.amt -= 1;
                    Debug.Log("After: " + invenItem.amt);
                    itemInInven = invenItem;
                }
                if (invenItem.itemType == Item.Type.Currency)
                {
                    amount = invenItem.amt;
                    Debug.Log("BB: " + amount);
                }
            }
            Debug.Log("Now: " + itemInInven.amt);
            if (itemInInven.amt <= 0)
            {
                Debug.Log("AAAAAAAAAAAA");
                itemList.Remove(itemInInven);
            }
        }
        else
        {
            itemList.Remove(item);
            Debug.Log("Removed ITem");
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public int getCurrencyAmt()
    {
        return amount;
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public List<Item> getItemList()
    {
        return itemList;
    }
}
