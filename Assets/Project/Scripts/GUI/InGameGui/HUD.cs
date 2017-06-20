using UnityEngine;

public class HUD : Menu
{
	[SerializeField] private Compass compass;
	[SerializeField] private HpBarre hpBarre;
	[SerializeField] private PexBarre pexBarre;
	[SerializeField] private ManaBarre manaBarre;

	public override void UpdateDisplay()
	{
		UpdateHpBarre();
		UpdatePexBarre();
		UpdateManaBarre();
	}
	public override void OnShow(params object[] parameters)
	{
        InputManager.Instance.Mode = InputMode.InGame;
	}

	public void UpdateCompass(GameObject obj)
	{
		if (compass == null)
			compass = FindObjectOfType<Compass>();
		compass.EditMarkList(obj);
	}
	public void UpdateHpBarre()
	{
		if (hpBarre == null)
			hpBarre = FindObjectOfType<HpBarre>();
		float hpRatio = GameManager.Instance.Player.CurrentHp / GameManager.Instance.Player.HpMax;
		hpBarre.Display(hpRatio);
	}
	public void UpdatePexBarre()
	{
		if (pexBarre == null)
			pexBarre = FindObjectOfType<PexBarre>();
		pexBarre.Display(GameManager.Instance.Player.Xp);
	}
	public void UpdateManaBarre()
	{
		if (manaBarre == null)
			manaBarre = FindObjectOfType<ManaBarre>();
		float manaRatio = GameManager.Instance.Player.Magic.CurrentMana / GameManager.Instance.Player.Magic.ManaMax;
		manaBarre.Display(manaRatio);
	}
}