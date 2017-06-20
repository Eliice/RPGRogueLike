using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Text nameTxt;
	[SerializeField] private Text priceTxt;
	[SerializeField] private Text weightTxt;
	[SerializeField] private Text statTxt;
    [SerializeField] private Text slotTxt;

    public void UpdateTooltip (MyButton button)
    {
        if (button != null)
        {
            if (button.item == null)
            {
                nameTxt.text = "";
                priceTxt.text = "";
                weightTxt.text = "";
                statTxt.text = "";
                slotTxt.text = "";
            }
            else
            {
                nameTxt.text = button.item.name;
                priceTxt.text = "Value: " + button.item.value.ToString();
                weightTxt.text = "Weight: " + button.item.weight.ToString();
                statTxt.text = button.item.getMainStat();
                slotTxt.text = button.item.getSlot();
            }
        }
    }
	public void ResetTooltip()
	{
		nameTxt.text = "";
		priceTxt.text = "";
		weightTxt.text = "";
		statTxt.text = "";
		slotTxt.text = "";
	}
}
