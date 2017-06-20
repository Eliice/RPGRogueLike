using UnityEngine;
using UnityEngine.UI;

public class MyButton : MonoBehaviour
{
    public SO_Item item;
    public Text amountText;
    public Text nameText;
    public Text equipText;
    int amount = 1;

    void Start()
    {
        if (item != null)
            nameText.text = item.name;
    }

    public void EquipItem()
    {
        equipText.text = "=>";
    }

    public void UnEquipItem()
    {
        equipText.text = "";
    }

    public void StackItem()
    {
        ++amount;
        amountText.text = amount.ToString();
    }

    public void UnstackItem()
    {
        --amount;
        if (amount == 1)
            amountText.text = "";
        else
            amountText.text = amount.ToString();
    }

    public bool RemoveButton (MyButton button)
    {
        if (amount > 1)
        {
            UnstackItem();
            return false;
        }
        else if (button != null)
            Destroy(button.gameObject);
        return true;
    }
}