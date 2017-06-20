using UnityEngine;
 
public enum Characteristics : int
{
	Strength = 0,
	Constitution,
	Intelligence,
	Dexterity,
	NbCharacteristics
}

public abstract class GenericCharacter : MonoBehaviour, IDamageable
{
    protected Charac charac;
	public Charac GetCharac() { return charac; }
    protected ParticleSystem hitByWeaponParticle;
    protected Animation anim;

    public int GetCharacteristic(Characteristics which)
	{
		switch(which)
		{
			case Characteristics.Strength:
				return charac.strength;

			case Characteristics.Constitution:
				return charac.constitution;

			case Characteristics.Intelligence:
				return charac.intelligence;

			case Characteristics.Dexterity:
				return charac.dexterity;

			default:
#if UNITY_EDITOR
				Debug.Log("Invalid enum value");
#endif
				return 0;
		}
	}
	public void SetCharacteristic(Characteristics which, int value)
	{
		switch (which)
		{
			case Characteristics.Strength:
				charac.strength = value;
				break;

			case Characteristics.Constitution:
				charac.constitution = value;
				break;

			case Characteristics.Intelligence:
				charac.intelligence = value;
				break;

			case Characteristics.Dexterity:
				charac.dexterity = value;
				break;

#if UNITY_EDITOR
			default:
				Debug.Log("Invalid enum value");
				break;
#endif
		}
	}


	#region propertie
	protected float currentHp;
	public float CurrentHp
    {
        get { return currentHp; }
        set { currentHp = value; }
    }

    #region strength
    public int BaseAtk
    {
        get { return 3 + charac.strength; }
    }
    #endregion

    #region const
    public int HpMax
    {
        get { return 300 + 10 * charac.constitution; }
    }

    public int BaseDef
    {
        get { return charac.constitution; }
    }
    public float BaseRegen
    {
        get { return 1 + charac.constitution / 10; }
    }
    #endregion

    #region dext
    public float AtkSpeed
    {
        get { return 1 + charac.dexterity / 100; }
    }

    public float Accuracy
    {
        get { return 25 - charac.dexterity / 10; }
    }

    #endregion
    #endregion

    #region init
    public void Init(int lvl =1 , int strenght = 0, int constit = 0, int intel = 0, int dext = 0)
    {
        charac.level = lvl;
        charac.strength = strenght;
        charac.constitution = constit;
        charac.intelligence = intel;
        charac.dexterity = dext;
    }
    #endregion

    virtual protected void Awake()
	{
		charac = ScriptableObject.CreateInstance<Charac>();
		Charac _charac = Resources.Load<Charac>("Charac/BaseCharacteristics");

		charac.level = _charac.level;
		charac.strength = _charac.strength;
		charac.constitution = _charac.constitution;
		charac.intelligence = _charac.intelligence;
		charac.dexterity = _charac.dexterity;
        anim = GetComponent<Animation>();
        RecalcAtkSpeed();

#if UNITY_EDITOR
		if (charac == null)  Debug.LogError("carac == null");
#endif
        hitByWeaponParticle = transform.FindChild("BloodParticle").GetComponent<ParticleSystem>();

        
    }

    public void RecalcHp()
    {
        currentHp = HpMax;
    }

    public void RecalcAtkSpeed()
    {
        var atkAnim = anim["AttackAnimation"];
        if (atkAnim != null)
            atkAnim.speed = AtkSpeed  > 3 ? 3 : AtkSpeed;
    }

    abstract public void takeDamage(int dmgTaken);
}
