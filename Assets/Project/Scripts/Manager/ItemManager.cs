using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
	#region singleton
	static private ItemManager instance;
    static public ItemManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<ItemManager>();

            return instance;
        }
    }
	#endregion

	private List<SO_Item> items = new List<SO_Item>();

    void Awake()
    {
        DataBase db = Resources.Load<DataBase>("ItemDB/ItemDB");
        foreach (ScriptableObject so in db.dataBase)
            items.Add(so as SO_Item);
    }

    public List<SO_Item> GenerateRandomItems(int nbOfItems)
    {
        List<SO_Item> itemList = new List<SO_Item>();
        for (int i = 0; i < nbOfItems; ++i)
            itemList.Add(items[Random.Range(0, items.Count)]);
        return itemList;
    }

    public List<SO_Item> GenerateAllItems(int amount)
    {
        List<SO_Item> itemList = new List<SO_Item>();
        foreach(SO_Item item in items)
        {
            for (int i = 0; i < amount; ++i)
                itemList.Add(item);
        }
        return itemList;
    }

	public SO_Item GetItem(string which)
	{
		foreach (SO_Item item in items)
			if (item.name == which)
				return item;

#if UNITY_EDITOR
		Debug.Log("Invalid item name requested");
#endif
		return null;
	}
}
