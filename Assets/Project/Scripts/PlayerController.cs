using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private CharacterController controller = null;

	#region motion
	[SerializeField] private float speed = 3f;
	private Vector3 motion = Vector3.zero;
	private bool jump = false;
	[SerializeField] private float jumpSpeed = 2f;
	#endregion
	#region camera
	[SerializeField] private float rotationSpeed = 180f;
	[SerializeField] [Range(0f, 90f)] private float maxLookUpAngle = 70f;
	[SerializeField] [Range(0f, -90f)] private float minLookDownAngle = -70f;
	private float lookAngle = 0f;
	new private GameObject camera = null;
	#endregion

	IInterract interractibleObject;

	private Inventory inventory;
    private Magic magicAbility;
    [HideInInspector] public bool shieldRaised = false;
    private Equipement equipement;
    public Equipement Equipement
    {
        get { return equipement; }
    }

    IAttack iAttack;

    void Awake()
	{
		iAttack = GetComponent<IAttack>();

		controller = GetComponent<CharacterController>();
		camera = transform.GetChild(0).gameObject;
		inventory = GetComponent<Inventory>();
		magicAbility = GetComponent<Magic>();
        equipement = GetComponent<Equipement>();
#if UNITY_EDITOR
		if (controller == null) Debug.Log("controller == null");
		if (camera == null) Debug.Log("camera == null");
		if (inventory == null) Debug.Log("inventory == null");
		if (magicAbility == null) Debug.Log("magicAbility == null");
#endif
	}	

	void FixedUpdate()
	{
		if (controller.isGrounded)
		{
			motion = transform.forward * InputManager.Instance.CentralAxis + transform.right * InputManager.Instance.LateralAxis;
			if (motion.magnitude > 1f)
				motion = motion.normalized;
			if (jump)
				motion.y = jumpSpeed;
			else
				motion.y = 0f;
		}
		motion.y -= 9.81f * Time.fixedDeltaTime;
		controller.Move(motion * speed * Time.fixedDeltaTime);
		jump = false;
		lookAngle -= InputManager.Instance.VerticalAxis * rotationSpeed * Time.fixedDeltaTime;
		lookAngle = Mathf.Clamp(lookAngle, minLookDownAngle, maxLookUpAngle);
		camera.transform.localEulerAngles = new Vector3(lookAngle, 0f, 0f);
		transform.Rotate(Vector3.up, InputManager.Instance.HorizontalAxis * rotationSpeed * Time.fixedDeltaTime);

		if (InputManager.Instance.Mode == InputMode.InGame)
			LookForInterraction();
	}

	public void SubscribeToInputManagerEvents()
	{
		InputManager.Instance.jumpKeyDown += Jump;
		InputManager.Instance.interractKeyDown += Interract;
		InputManager.Instance.spellCastKeyDown += magicAbility.Use;
		InputManager.Instance.attackKeyDown += Attack;
		InputManager.Instance.inventoryKeyDown += ToggleInventory;
		InputManager.Instance.selectSpellKeyDown += magicAbility.SelectSpeel;
        InputManager.Instance.defenseKeyDown += DefenseUp;
        InputManager.Instance.defenseKeyUp += DefenseDown;
	}
	public void UnsubscribeToInputManagerEvents()
	{
		InputManager.Instance.jumpKeyDown -= Jump;
		InputManager.Instance.interractKeyDown -= Interract;
		InputManager.Instance.spellCastKeyDown -= magicAbility.Use;
		InputManager.Instance.attackKeyDown -= Attack;
		InputManager.Instance.inventoryKeyDown -= ToggleInventory;
		InputManager.Instance.selectSpellKeyDown -= magicAbility.SelectSpeel;
	}

	private void LookForInterraction()
	{
		RaycastHit raycast;
		if (Physics.Raycast(camera.transform.position, camera.transform.forward, out raycast, 2.5f))
		{
            IInterract iInterract = raycast.collider.GetComponent<IInterract>();

            if (iInterract != null && iInterract.CanInterract())
            {
                UIManager.Instance.Notify(UIRequest.InterractionFeedback, UIRequestMode.Show, iInterract.InterractionDescription());
                interractibleObject = raycast.transform.GetComponent<IInterract>();
				return;
            }
		}

		if (interractibleObject != null)
			UIManager.Instance.Notify(UIRequest.InterractionFeedback, UIRequestMode.Hide);

		interractibleObject = null;
	}
	public void Interract()
	{
		if (interractibleObject != null)
		{
			interractibleObject.Use();
			interractibleObject = null;

			UIManager.Instance.Notify(UIRequest.InterractionFeedback, UIRequestMode.Hide);
		}
	}

	private void Jump()
	{
		jump = true;
	}

	private void ToggleInventory()
	{
		UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Toggle);
		UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Toggle, InventoryType.PLAYER, inventory, equipement);

		interractibleObject = null;
		UIManager.Instance.Notify(UIRequest.InterractionFeedback, UIRequestMode.Hide);
	}

    private void Attack()
	{
        if (!shieldRaised)
            iAttack.Attack();
	}
    private void DefenseUp()
    {
        Defense def = GetComponent<Defense>();
        if (def != null)
            def.ShieldUp();
    }
    private void DefenseDown()
    {
        Defense def = GetComponent<Defense>();
        if (def != null)
            def.ShieldDown();
    }
}