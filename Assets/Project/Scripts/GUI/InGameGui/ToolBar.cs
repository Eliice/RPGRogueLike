using UnityEngine;
using UnityEngine.UI;

public class ToolBar : MonoBehaviour
{
	[SerializeField] Text goldTxt;
	[SerializeField] Text mainStatTxt;
	[SerializeField] Text weightTxt;
    [SerializeField] Text fstInput;
    [SerializeField] Text sndInput;

    void Start ()
    {
        UpdateGoldAndWeight();
    }

    public void UpdateGoldAndWeight ()
    {
        Inventory playerInv = GameManager.Instance.Player.Inventory;
        goldTxt.text = "Gold: " + playerInv.Gold.ToString();
        weightTxt.text = "Weight: " + playerInv.CrtWeight.ToString() + "/" + playerInv.MaxWeight.ToString();
        if (playerInv.CrtWeight >= playerInv.MaxWeight)
            weightTxt.color = Color.red;
        else
            weightTxt.color = Color.black;
    }

    public void UpdateInput(InventoryType invType)
    {
        switch (invType)
        {
            case InventoryType.CHEST:
                fstInput.text = "[E] : Take";
                sndInput.text = "[R] : Take All";
                break;
            case InventoryType.ENEMY:
                fstInput.text = "[E] : Take";
                sndInput.text = "[R] : Take All";
                break;
            case InventoryType.PLAYER:
                fstInput.text = "[E] : Equip";
                sndInput.text = "[R] : Throw";
                break;
            case InventoryType.SHOP:
                fstInput.text = "[E] : Buy";
                sndInput.text = "";
                break;
            case InventoryType.PLAYER_SHOP:
                fstInput.text = "[E] : Sell";
                sndInput.text = "";
                break;
            case InventoryType.PLAYER_STORE:
                fstInput.text = "[E] : Store";
                sndInput.text = "";
                break;
            default:
                break;
        }
    }
}