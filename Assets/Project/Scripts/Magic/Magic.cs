using System.Collections;
using UnityEngine;

public class Magic : MonoBehaviour, ICaster {

    public enum Type
    {
        DAMAGE,
        HEAL,
        COUNT
    };

    private Animation animations;
    private Transform hand;
    private Equipement equipement;

    private GameObject healedParticlePref; 
    private GameObject castDamageSpellParticlePref;
    private GameObject castHealSpellParticlePref;
    private ParticleSystem currCastDmg;
    private ParticleSystem currCastHeal;
    private int baseIntel;
    [SerializeField]
    private MagicBase[] spellList;
    private int spellSelector = 0;
    private float currentMana;
    private GenericCharacter self;
    private bool regen = false;
    private bool regenBoost = false;

    #region ressources
    private GameObject genericDamageSpell;
    private Transform parent;
    #endregion

    public float CurrentMana
    {
        get { return currentMana; }
    }
    
    public bool RegenBoost
    {
        set { regenBoost = value; }
    }

    public int ManaMax
    {
        get { return 300 + 10 * baseIntel; }
    }

    public float ManaRegen
    {
        get { return  1 + baseIntel / 10; }
    }

    void Awake()
    {
        hand = transform.GetChild(1).GetChild(1).FindChild("Hand");
        healedParticlePref = Resources.Load("Prefabs/ParticleSystem/HealedParticle") as GameObject;
        castHealSpellParticlePref = Resources.Load("Prefabs/ParticleSystem/CastHealSpellParticle") as GameObject;
        castDamageSpellParticlePref = Resources.Load("Prefabs/ParticleSystem/CastDamageSpellParticle") as GameObject;
        animations = GetComponent<Animation>();
        currentMana = ManaMax;
        genericDamageSpell = Resources.Load<GameObject>("Magic/MagicEffect/GenericSpell");
        regen = false;
        // -- tmp mana --//
        if (!transform.CompareTag("Player"))
            currentMana = 1000000f;
        // ------------- //
    }

	void Start()
	{
		self = GetComponent<GenericCharacter>();
		baseIntel = self.GetCharac().intelligence;
        equipement = GetComponent<Equipement>();
	}

	public void Use()
    {
        if (spellSelector <= spellList.Length && spellList[spellSelector] != null && !animations.isPlaying)
        {
            if (currentMana >= spellList[spellSelector].manaCost)
            {
                Cast();
                currentMana -= spellList[spellSelector].manaCost;
            }
        }
    }

    public void ShowWeapon(bool show)
    {
        if (equipement != null)
        {
            if (equipement.crtWeapon != null)
                equipement.crtWeapon.SetActive(show);
        }
    }

    public bool IsCasting()
    {
        if (animations.IsPlaying("AttackSpellAnimation") || animations.IsPlaying("HealSpellAnimation"))
        {
            return true;
        }
        return false;
    }

    private void Cast()
    {
        switch (spellList[spellSelector].type)
        {
            case Type.HEAL:
                {
                    animations.Play("HealSpellAnimation");
                    currCastHeal = PlayParticle(castHealSpellParticlePref, hand);
                    break;
                }
            case Type.DAMAGE:
                {
                    animations.Play("AttackSpellAnimation");
                    currCastDmg = PlayParticle(castDamageSpellParticlePref, hand);
                    break;
                }
        }
        if (!regen)
        {
            regen = true;
            StartCoroutine(Regeneration());
        }
    }

    private void Heal()
    {
        self.CurrentHp = self.CurrentHp + baseIntel * spellList[spellSelector].intelRatio > self.HpMax ? self.HpMax : self.CurrentHp + baseIntel * spellList[spellSelector].intelRatio;
        Destroy(currCastHeal.gameObject);
        ParticleSystem healPs = PlayParticle(healedParticlePref, transform, true);
        healPs.transform.localPosition = new Vector3(0, transform.lossyScale.y, 0);
        ShowWeapon(true);
    }

    private void Damage() // Called in Animation
    {
        GameObject dmgSpell = Instantiate(genericDamageSpell);
        dmgSpell.GetComponent<GenericDamageSpell>().damages = baseIntel * (int)spellList[spellSelector].intelRatio;
        Vector3 launch = hand.position + hand.forward;
        launch.y -= 1f;
        dmgSpell.transform.position = launch;
        Vector3 dir = new Vector3();
        if (transform.CompareTag("Player"))
            dir = Camera.main.transform.forward;
        else
            dir = transform.forward;
        dmgSpell.GetComponent<Rigidbody>().velocity = dir * spellList[spellSelector].speed;
        Destroy(currCastDmg.gameObject);
        ShowWeapon(true);
    }

    public void SelectSpeel(int seletor= 0)
    {

        if (seletor <= spellList.Length)
            spellSelector = seletor;
    }

    private IEnumerator Regeneration()
    {
        while (regen)
        {
            if (regenBoost)
                currentMana += ManaRegen * 100;
            else
                currentMana += ManaRegen;
            if (currentMana >= ManaMax)
            {
                regen = false;
                currentMana = ManaMax;
            }
			if (gameObject.tag == "Player")
				UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Update);
            yield return new WaitForSeconds(1f);
        }
    }

    ParticleSystem PlayParticle(GameObject toPlay, Transform parent = null, bool destroyAfterLoop = false)
    {
        ShowWeapon(false);
        GameObject clone = Instantiate(toPlay);
        if (parent)
        {
            clone.transform.parent = parent;
            clone.transform.localPosition = Vector3.zero;
        }
        ParticleSystem partSystem = clone.GetComponent<ParticleSystem>();
        partSystem.Play();
        if (destroyAfterLoop)
            Destroy(clone, partSystem.duration);
        return partSystem;
    }
}
