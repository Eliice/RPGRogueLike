﻿using System.Collections;
using UnityEngine;

public class Player : GenericCharacter {

    private int xp;
	[SerializeField]
    private int xpPerLevel = 200;
    private Inventory inventory = null;
    public Inventory Inventory { get { return inventory; } }
    private bool regen = false;
    private bool regenBoost = false;

    public int Xp
    {
        get { return xp; }
		set { xp = value; }
    }
	
    public bool RegenBoost
    {
        set { regenBoost = value; }
    }

    [SerializeField] private Transform playerWeaponAnchor;
    public Transform PlayerWeaponAnchor
    {
        get { return playerWeaponAnchor; }
    }

    [SerializeField] private Transform playerShieldAnchor;
    public Transform PlayerShieldAnchor
    {
        get { return playerShieldAnchor; }
    }

    private PlayerController playerCtrl;
    private Magic magic;
    public Magic Magic
    {
        get { return magic; }
    }

    override protected void Awake()
    {
        base.Awake();
        currentHp = HpMax;
		inventory = GetComponent<Inventory>();
        magic = GetComponent<Magic>();
        playerCtrl = GetComponent<PlayerController>();
        RecalcAtkSpeed();
    }

    public override void takeDamage(int dmgTaken)
    {
        dmgTaken -= playerCtrl.Equipement.Armor;
		if (dmgTaken < 0)
			dmgTaken = 0;
        if (playerCtrl.shieldRaised)
            dmgTaken /= 2;

        hitByWeaponParticle.Play();
		currentHp -= dmgTaken;
        if (!regen)
        {
            regen = true;
            StartCoroutine(Regeneration());
        }
		if (currentHp < 1)
		{
			UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Hide);
			UIManager.Instance.Notify(UIRequest.DeathMenu, UIRequestMode.Show);
		}
    }

    public void EarnXp(int xpAmount)
    {
        xp += xpAmount;
        if (xp >= xpPerLevel)
		LevelUp();
		UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Update);
    }

	private void LevelUp()
	{
		++charac.level;
		xp -= xpPerLevel;

		GameManager.Instance.PauseGame();
		UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Hide);
		UIManager.Instance.Notify(UIRequest.LevelUp, UIRequestMode.Show);
	}

	private IEnumerator Regeneration()
    {
        while (regen)
        {
            if (regenBoost)
                currentHp += BaseRegen * 100;
            else
                currentHp += BaseRegen;
            if (currentHp >= HpMax)
            {
                regen = false;
                currentHp = HpMax;
            }
			UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Update);
            yield return new WaitForSeconds(1f);
        }
    }
}