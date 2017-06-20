using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum InventoryType
{
    PLAYER,
    PLAYER_SHOP,
    PLAYER_STORE,
    CHEST,
    ENEMY,
    SHOP
}

public class Inventory : MonoBehaviour
{
    private List<SO_Item> inventoryList = new List<SO_Item>();
    private InventoryType type;
    [SerializeField]
    int gold;
    private int crtWeight = 0;
    [SerializeField]
    int maxWeight;

    public List<SO_Item> InventoryList
	{
		get { return inventoryList; }
		set { inventoryList = value; }
	}

    public InventoryType Type
    {
        get { return type; }
        set { type = value; }
    }

    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    public int CrtWeight
    {
        get { return crtWeight; }
        set { crtWeight = value; }
    }

    public int MaxWeight
    {
        get { return maxWeight; }
        set { maxWeight = value; }
    }

    public void Receive(MyButton button)
    {
        if (button != null)
        {
            if (button.item != null)
            {
                inventoryList.Add(button.item);
                crtWeight += button.item.weight;
				UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Update);
            }
        }
    }

    public void Receive(SO_Item itemToReceive)
    {
        if (itemToReceive != null)
        {
            inventoryList.Add(itemToReceive);
            crtWeight += itemToReceive.weight;
            UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Update);
        }
    }

    public void Remove(MyButton button)
    {
        if (button != null)
        {
            if (button.item != null)
            {
                inventoryList.Remove(button.item);
                crtWeight -= button.item.weight;
				UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Update);
			}
        }
    }
}