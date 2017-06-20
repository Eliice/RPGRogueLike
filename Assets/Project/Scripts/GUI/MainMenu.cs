using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
	private Button newGameButton = null;
	private Button continueButton = null;
	private Button exitButton = null;
	protected override void Awake()
	{
		base.Awake();
		newGameButton = transform.GetChild(0).GetComponent<Button>();
		continueButton = transform.GetChild(1).GetComponent<Button>();
		exitButton = transform.GetChild(2).GetComponent<Button>();
#if UNITY_EDITOR
		if (newGameButton == null) Debug.Log("newGameButton == null");
		if (continueButton == null) Debug.Log("continueButton == null");
		if (exitButton == null) Debug.Log("exitButton == null");
#endif
	}

	protected override void Start()
	{
		newGameButton.onClick.AddListener(() => { UIManager.Instance.Notify(UIRequest.MainMenu, UIRequestMode.Hide); GameManager.Instance.NewGame(); });
		continueButton.onClick.AddListener(() => { UIManager.Instance.Notify(UIRequest.MainMenu, UIRequestMode.Hide); GameManager.Instance.LoadGame(); });
		exitButton.onClick.AddListener(() => { GameManager.Instance.ExitGame(); });
		if (!File.Exists("savegame"))
		{
			continueButton.interactable = false;
			continueButton.GetComponent<CanvasRenderer>().SetAlpha(0.25f);
		}

		base.Start();
	}
}