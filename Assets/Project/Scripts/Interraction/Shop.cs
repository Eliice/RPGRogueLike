using UnityEngine;

public class Shop : MonoBehaviour, IInterract
{
	private Inventory inventory = null;

	void Awake()
	{
		inventory = GetComponent<Inventory>();
#if UNITY_EDITOR
		if (inventory == null) Debug.LogError("inventory == null");
#endif
	}

	void Start ()
	{
		inventory.InventoryList = ItemManager.Instance.GenerateAllItems(3);
	}

	public void Use()
	{
		UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Hide);
		UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Show, InventoryType.SHOP, inventory);
	}

    public bool CanInterract()
    {
        return true;
	}
	public string InterractionDescription()
	{
		return "[E] : Shop";
	}
}