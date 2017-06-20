using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour, IInterract
{
    private SO_Item item;

    public void CreateDroppedItem(SO_Item itemToDrop)
    {
        item = itemToDrop;
    }

    public bool CanInterract()
    {
        return true;
    }

    public void Use ()
    {
        GameManager.Instance.Player.Inventory.Receive(item);
        Destroy(this.gameObject);
	}
	public string InterractionDescription()
	{
		return "[E] : Take";
	}
}
