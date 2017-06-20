using UnityEngine;

public abstract class Menu : MonoBehaviour
{
	[SerializeField] UIRequest type = UIRequest.NbMenus;

	protected virtual void Awake()
	{
		UIManager.Instance.AddMenu(this, type);
	}
	protected virtual void Start()
	{
		UIManager.Instance.Notify(type, UIRequestMode.Hide);
	}

	virtual public void UpdateDisplay()
	{

	}

	virtual public void OnShow(params object[] parameters)
	{

	}

	virtual public void OnHide(params object[] parameters)
	{

	}
}
