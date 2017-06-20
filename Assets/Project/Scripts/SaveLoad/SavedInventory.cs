using System.Collections.Generic;

[System.Serializable]
public class SavedInventory : SavedObject
{
	private string[] itemList = null;
	private int gold;
	private int weigth;

	public override void SaveData(object obj)
	{
		Inventory inventory = obj as Inventory;
		List<SO_Item> inventoryList = inventory.InventoryList;
		itemList = new string[inventoryList.Count];
		for (int i = 0; i < inventoryList.Count; ++i)
			itemList[i] = inventoryList[i].name;
		gold = inventory.Gold;
		weigth = inventory.CrtWeight;
	}

	public override void LoadData(object obj)
	{
		Inventory inventory = obj as Inventory;
		List<SO_Item> inventoryList = inventory.InventoryList;
		foreach (string name in itemList)
			inventoryList.Add(ItemManager.Instance.GetItem(name));
		inventory.Gold = gold;
		inventory.CrtWeight = weigth;
	}
}