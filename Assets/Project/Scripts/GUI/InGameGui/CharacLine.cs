using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacLine : MonoBehaviour
{
	private LevelUpMenu menu = null;

	private Characteristics charac = Characteristics.NbCharacteristics;
	public Characteristics Characteristic { get { return charac; } set { charac = value; } }
	private int value = 0;
	private int diff = 0;

	private Text characLabel = null;
	private Text valueLabel = null;
	private Text diffLabel = null;
	private Button minusButton = null;
	private Button plusButton = null;

	void Awake()
	{
		characLabel = transform.GetChild(0).GetComponent<Text>();
		valueLabel = transform.GetChild(1).GetComponent<Text>();
		diffLabel = transform.GetChild(2).GetComponent<Text>();
		minusButton = transform.GetChild(3).GetComponent<Button>();
		plusButton = transform.GetChild(4).GetComponent<Button>();
			
#if UNITY_EDITOR
		if (characLabel == null)
			Debug.Log("characLabel == null");
		if (valueLabel == null)
			Debug.Log("valueLabel == null");
		if (diffLabel == null)
			Debug.Log("diffLabel == null");
		if (minusButton == null)
			Debug.Log("minusButton == null");
		if (plusButton == null)
			Debug.Log("plusButton == null");
#endif
		minusButton.onClick.AddListener(() => { DecCharac(); });
		plusButton.onClick.AddListener(() => { IncCharac(); });
	}

	void Start()
	{
		menu = transform.parent.GetComponent<LevelUpMenu>();
#if UNITY_EDITOR
		if (charac == Characteristics.NbCharacteristics)
			Debug.Log("charac = Characteristics.NbCharacteristics");
#endif
		characLabel.text = Enum.GetName(charac.GetType(), charac);
		ResetValue();
	}

	private void UpdateDisplay()
	{
		diffLabel.text = diff.ToString();
	}

	public void ResetValue()
	{
		value = GameManager.Instance.Player.GetCharacteristic(charac);
		valueLabel.text = value.ToString();
		diff = 0;
		UpdateDisplay();
	}

	public void ApplyValue()
	{
		GameManager.Instance.Player.GetComponent<Player>().SetCharacteristic(charac, value + diff);
	}

	private void IncCharac()
	{
		if (menu.PointsLeft > 0)
		{
			--menu.PointsLeft;
			++diff;
			UpdateDisplay();
		}
	}

	private void DecCharac()
	{
		if (diff > 0)
		{
			++menu.PointsLeft;
			--diff;
			UpdateDisplay();
		}
	}
}