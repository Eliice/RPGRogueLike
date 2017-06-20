using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : Menu
{
	[SerializeField] private Button mainMenuButton = null;
	[SerializeField] private Button exitButton = null;
	[SerializeField] private Button tryAgainButton = null;

	protected override void Awake()
	{
		base.Awake();
#if UNITY_EDITOR
		if (mainMenuButton == null) Debug.Log("mainMenuButton == null");
		if (exitButton == null) Debug.Log("exitButton == null");
		if (tryAgainButton == null) Debug.Log("tryAgainButton == null");
#endif
	}

	protected override void Start()
	{
		mainMenuButton.onClick.AddListener(() => { GameManager.Instance.GoToMainMenu(); });
		exitButton.onClick.AddListener(() => { GameManager.Instance.ExitGame(); });
		tryAgainButton.onClick.AddListener(() => { GameManager.Instance.NewGame(); });

		base.Start();
	}

	public override void OnShow(params object[] parameters)
	{
		InputManager.Instance.Mode = InputMode.InGameMenu;

		if (File.Exists("savegame"))
			File.Delete("savegame");
	}
}