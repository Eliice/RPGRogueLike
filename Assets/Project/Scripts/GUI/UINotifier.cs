using UnityEngine;
using UnityEngine.UI;

public class UINotifier : MonoBehaviour
{
	[SerializeField] private UIRequest request = UIRequest.NbMenus;
	[SerializeField] private UIRequestMode mode = UIRequestMode.Default;
	private bool isSubscribed = false;
    public delegate void UIRequestEvent(UIRequest request, UIRequestMode mode, params object[] parameters);
    public event UIRequestEvent notify;

    public bool IsSubscribed
	{
		get { return isSubscribed; }
		set { isSubscribed = value; }
	}
	

	void Awake()
	{
		UIManager.Instance.SubscribeToNotifier(this);
		GetComponent<Button>().onClick.AddListener(SendNotification);
	}

	void OnDestroy()
	{
		if (isSubscribed)
			UIManager.Instance.UnsubscribeToNotifier(this);
	}

	public void SendNotification()
	{
		notify(request, mode);
	}
}