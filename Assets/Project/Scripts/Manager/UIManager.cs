using System.Collections.Generic;
using UnityEngine;

public enum UIRequest
{
	MainMenu = 0,
	NewGame,
	PauseMenu,
	HUD,
	LevelUp,
	Inventory,
	InterractionFeedback,
	DeathMenu,
	NbMenus
}

public enum UIRequestMode
{
	Default = 0,
	Show,
	Hide,
	Toggle,
	Update
}

public class UIManager : MonoBehaviour
{
	#region singleton
	static private UIManager instance;
	static public UIManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<UIManager>();
#if UNITY_EDITOR
				if (instance == null)
					Debug.LogError("instance == null");
#endif
			}

			return instance;
		}
	}
	#endregion

	private Menu[] menus = null;
	private List<UINotifier> notifiers = null;

	void Awake()
	{
		menus = new Menu[(int)UIRequest.NbMenus];
		notifiers = new List<UINotifier>();
	}

	void OnDestroy()
	{
		UINotifier[] notifiersCpy = new UINotifier[notifiers.Count];
		notifiers.CopyTo(notifiersCpy);
		foreach (UINotifier notifier in notifiersCpy)
			UnsubscribeToNotifier(notifier);
	}

	public void SubscribeToNotifier(UINotifier notifier)
	{
		if (!notifier.IsSubscribed)
		{
			notifier.notify += OnNotification;
			notifiers.Add(notifier);
			notifier.IsSubscribed = true;
		}
	}
	public void UnsubscribeToNotifier(UINotifier notifier)
	{
		if (notifier.IsSubscribed)
		{
			notifier.notify -= OnNotification;
			notifiers.Remove(notifier);
			notifier.IsSubscribed = false;
		}
	}

	public void AddMenu(Menu menu, UIRequest which)
	{
		menus[(int)which] = menu;
	}

	private void OnNotification(UIRequest request, UIRequestMode mode, params object[] parameters)
	{
		Notify(request, mode, parameters);
	}
	public void Notify(UIRequest request, UIRequestMode mode, params object[] parameters)
	{
		if (request == UIRequest.NbMenus || menus[(int)request] == null)
		{
#if UNITY_EDITOR
			if (request == UIRequest.NbMenus)
				Debug.Log("request == UIRequest.NbMenus");
			else
				Debug.Log("menus[" + request.ToString() + "] == null");
#endif
			return;
		}
		UseMenu(request, mode, parameters);
	}
	private void UseMenu(UIRequest request, UIRequestMode mode, params object[] parameters)
	{
		Menu menu = menus[(int)request];
		switch (mode)
		{
			case UIRequestMode.Show:
				menu.gameObject.SetActive(true);
				menu.OnShow(parameters);
				break;
			case UIRequestMode.Hide:
				menu.OnHide(parameters);
				menu.gameObject.SetActive(false);
				break;
			case UIRequestMode.Toggle:
				if (menu.gameObject.activeSelf)
					goto case UIRequestMode.Hide;
				else
					goto case UIRequestMode.Show;
			case UIRequestMode.Update:
				menu.UpdateDisplay();
				break;
			default:
#if UNITY_EDITOR
				Debug.Log("invalid enum value");
#endif
				break;
		}
	}
}