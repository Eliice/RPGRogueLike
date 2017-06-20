using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu
{
	[SerializeField] private Button resumeButton = null;
	[SerializeField] private Button exitButton = null;
	[SerializeField] private Button mainMenuButton = null;

	protected override void Awake()
	{
		base.Awake();

#if UNITY_EDITOR
		if (resumeButton == null) Debug.Log("resumeButton == null");
		if (exitButton == null) Debug.Log("exitButton == null");
		if (mainMenuButton == null) Debug.Log("mainMenuButton == null");
#endif
	}

	protected override void Start()
	{
		resumeButton.onClick.AddListener(() => { GameManager.Instance.UnpauseGame(); UIManager.Instance.Notify(UIRequest.PauseMenu, UIRequestMode.Hide); UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Show); });
		exitButton.onClick.AddListener(() => { SaveGameManager.Instance.SaveGame(); GameManager.Instance.ExitGame(); });
		mainMenuButton.onClick.AddListener(() => { SaveGameManager.Instance.SaveGame(); GameManager.Instance.GoToMainMenu(); });

		base.Start();
	}

	public override void OnShow(params object[] parameters)
	{
		InputManager.Instance.Mode = InputMode.InGameMenu;
	}
	public override void OnHide(params object[] parameters)
	{
		InputManager.Instance.Mode = InputMode.InGame;
	}
}
