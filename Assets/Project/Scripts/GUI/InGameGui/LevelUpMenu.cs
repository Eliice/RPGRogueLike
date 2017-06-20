using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : Menu
{
	[SerializeField] GameObject characLinePrefab = null;

	[SerializeField] private Button confirmButton = null;
	private Text pointsLeftLabel = null;
	private CharacLine[] characLines = null;

	private int pointsLeft;
	public int PointsLeft
	{
		get { return pointsLeft; }
		set { pointsLeft = value; UpdatePointsLeftLabel(); }
	}

	protected override void Awake()
	{
		base.Awake();

		pointsLeftLabel = transform.GetChild(1).GetComponent<Text>();

#if UNITY_EDITOR
		if (pointsLeftLabel == null) Debug.LogError("pointsLeftLabel == null");
		if (confirmButton == null) Debug.LogError("confirmButton == null");
#endif
	}
	protected override void Start()
	{
		characLines = new CharacLine[(int)Characteristics.NbCharacteristics];
		for (int i = 0; i < (int)Characteristics.NbCharacteristics; ++i)
		{
			characLines[i] = Instantiate(characLinePrefab).GetComponent<CharacLine>();
			characLines[i].transform.SetParent(transform);
			characLines[i].transform.SetSiblingIndex(transform.childCount - 2);
			characLines[i].Characteristic = (Characteristics)i;
			characLines[i].ResetValue();
		}

		confirmButton.onClick.AddListener(OnConfirmation);

		base.Start();
	}

	public override void OnShow(params object[] parameters)
	{
		InputManager.Instance.Mode = InputMode.InGameMenu;

		foreach (CharacLine line in characLines)
			line.ResetValue();
		PointsLeft += GameManager.Instance.CharacPointsPerLevel;
	}

	private void OnConfirmation()
	{
		foreach (CharacLine line in characLines)
			line.ApplyValue();

        GameManager.Instance.Player.RecalcAtkSpeed();

		GameManager.Instance.UnpauseGame();

		UIManager.Instance.Notify(UIRequest.LevelUp, UIRequestMode.Hide);
		UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Show);

		InputManager.Instance.Mode = InputMode.InGame;
	}

	void UpdatePointsLeftLabel()
	{
		pointsLeftLabel.text = "Points left : " + pointsLeft;
	}
}