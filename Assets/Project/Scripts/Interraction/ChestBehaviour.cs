using UnityEngine;

public class ChestBehaviour : MonoBehaviour, IInterract {

    private bool chestOpen = false;
    private RotateObject rotateObject;
	private Inventory inventory = null;

	void Awake()
	{
		inventory = GetComponentInParent<Inventory>();
		rotateObject = GetComponent<RotateObject>();
#if UNITY_EDITOR
		if (inventory == null) Debug.LogError("inventory == null");
		if (rotateObject == null) Debug.LogError("rotateObject == null");
#endif
	}

	void Start ()
    {
		inventory.InventoryList = ItemManager.Instance.GenerateRandomItems(6);
    }

    public void Use()
    {
        if (rotateObject.inAnimation)
            return;
        if (!chestOpen)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            rotateObject.Rotate(transform, new Vector3(1, 0, 0), new Vector3(25,0,0), new Vector3(0,0,0), true);
            chestOpen = true;
			UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Hide);
			UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Show, InventoryType.CHEST, inventory);
        }
        else
        {
            rotateObject.Rotate(transform, new Vector3(-1, 0, 0), new Vector3(355, 0, 0), new Vector3(0, 0, 0), true);
            chestOpen = false;
        }
    }

    public bool CanInterract()
    {
        return !rotateObject.inAnimation;
	}
	public string InterractionDescription()
	{
		return "[E] : Loot";
	}
}
