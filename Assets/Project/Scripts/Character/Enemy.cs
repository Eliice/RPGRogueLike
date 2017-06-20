using UnityEngine;
using System.Collections;

public class Enemy : GenericCharacter, IInterract
{

    public Type race;
    public float dropChance;
    private StatePatternEnemy statePattern;
    private bool regen = false;
    private bool isAlive = true;
    private Inventory inventory = null;
    private ArrayList toKeep = new ArrayList();
    private Animation animations;

    [SerializeField]
    private int pexGiven; // level multiplicateur is setup on awake
    [SerializeField]
    private Charac upByLevel;

    static private bool setUp = false;

    #region init
    override protected void Awake()
    {
        base.Awake();
        statePattern = GetComponent<StatePatternEnemy>();
        animations = GetComponent<Animation>();
        InitComponentToKeep();
        RecalcAtkSpeed();
    }

    void InitComponentToKeep()
    {
        toKeep = new ArrayList();
        toKeep.Add(GetComponent<Transform>());
        toKeep.Add(GetComponent<CapsuleCollider>());
        toKeep.Add(GetComponent<MeshFilter>());
        toKeep.Add(GetComponent<MeshRenderer>());
        toKeep.Add(this);
    }

    void Start()
    {
        if (dropChance > 100)
            dropChance = 100;
        else if (dropChance < 0)
            dropChance = 0;

        charac.level = GameManager.Instance.Player.GetCharac().level;
        pexGiven *= charac.level;
        if (!setUp)
        {
            Init(charac.level, charac.strength + charac.level * upByLevel.strength, charac.constitution + charac.level * upByLevel.constitution, charac.intelligence + charac.level * upByLevel.intelligence, charac.dexterity + charac.level * upByLevel.dexterity);
            setUp = true;
        }
        
        currentHp = HpMax;
        if (gameObject.tag == "Boss")
            LevelManager.Instance.AddMainObject(gameObject);

    }
    #endregion

    public enum Type
    {
        Warrior,
        Wizard
    };


    public override void takeDamage(int dmgTaken)
    {
        hitByWeaponParticle.Play();
        currentHp -= (dmgTaken - BaseDef > 0 ? dmgTaken - BaseDef : 1);
        if (!regen)
        {
            regen = true;
            StartCoroutine(Regeneration());
        }
        if (currentHp < 1)
        {
            if (gameObject.tag == "Boss")
                LevelManager.Instance.RemoveObject(gameObject);
            GameManager.Instance.Player.EarnXp(pexGiven);
            setUp = false;
            Death();

        }
        if (statePattern.currentState == statePattern.patrolState)
            statePattern.currentState.ToAlerteState();
    }

    void Drop()
    {
        inventory = gameObject.AddComponent<Inventory>();

        float rand = Random.Range(0, 10f);
        if (dropChance != 0)
            if (rand <= dropChance / 10f)
                inventory.InventoryList = ItemManager.Instance.GenerateRandomItems(Random.Range(1, 4));
            
    }

    void Death()
    {
        if (animations.isPlaying)
            animations.Stop();

        Component[] components = GetComponents<Component>();
        foreach (Component comp in components)
        {
            if (!toKeep.Contains(comp))
                Destroy(comp);
        }
        
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider comp in colliders)
        comp.isTrigger = true;
        
        DeadStance();
        Drop();

        isAlive = false;
    }

    void DeadStance()
    {
        Vector3 pos = transform.position;
        pos.y -= 0.5f;
        transform.position = pos;
        Vector3 rot = transform.eulerAngles;
        rot.x = 270;
        transform.eulerAngles = rot;
    }

    public void Use()
    {
		UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Hide);
        UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Show, InventoryType.ENEMY, inventory);
    }
    public bool CanInterract()
    {
        return !isAlive;
    }
	public string InterractionDescription()
	{
		return "[E] : Loot";
	}

    private IEnumerator Regeneration()
    {
        while (regen)
        {
            currentHp += BaseRegen;
            if (currentHp >= HpMax)
            {
                regen = false;
                currentHp = HpMax;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}