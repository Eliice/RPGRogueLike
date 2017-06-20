using UnityEngine;

public enum InputMode
{
	MainMenu,
	InGame,
	Inventory,
	InGameMenu
}

public class InputManager : MonoBehaviour
{
	#region singleton
	static private InputManager instance;
	static public InputManager Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType<InputManager>();

			return instance;
		}
	}
	#endregion

	private InputMode mode;
    private delegate void InputUpdate();
    private InputUpdate update;

    public InputMode Mode
	{
		get { return mode; }
		set
		{
			mode = value;
			ResetAxes();
			update = null;
			switch (mode)
			{
				case InputMode.MainMenu:
					CursorMode = CursorLockMode.Confined;
					break;
				case InputMode.InGame:
					update += InGameUpdate;
					CursorMode = CursorLockMode.Locked;
					break;
				case InputMode.Inventory:
					update += InventoryUpdate;
					CursorMode = CursorLockMode.Confined;
					break;
				case InputMode.InGameMenu:
					CursorMode = CursorLockMode.Confined;
					break;
				default:
					break;
			}
		}
	}

	#region cursor management
	private CursorLockMode cursorMode = CursorLockMode.None;
	public CursorLockMode CursorMode
	{
		get { return cursorMode; }
		set
		{
			if (value != cursorMode)
			{
				cursorMode = value;
				cursorModeChanged = true;
			}
		}
	}
	private bool cursorModeChanged = false;
	#endregion

	#region InputKey events
	public delegate void InputKey();
    public delegate void InputSpellKey(int value);
	public event InputKey jumpKeyDown;
	public event InputKey spellCastKeyDown;
    public event InputKey interractKeyDown;
	public event InputKey attackKeyDown;
	public event InputKey inventoryKeyDown;
    public event InputKey inventoryAction1;
    public event InputKey inventoryAction2;
    public event InputKey defenseKeyDown;
    public event InputKey defenseKeyUp;

    public event InputSpellKey selectSpellKeyDown;
	#endregion

	#region axes
	private float centralAxis = 0f;
	public float CentralAxis { get { return centralAxis; } }
	private float lateralAxis = 0f;
	public float LateralAxis { get { return lateralAxis; } }
	private float verticalAxis = 0f;
	public float VerticalAxis { get { return verticalAxis; } }
	private float horizontalAxis = 0f;
	public float HorizontalAxis { get { return horizontalAxis; } }
	#endregion

	void Awake()
	{
		Mode = InputMode.MainMenu;
		CursorMode = CursorLockMode.Confined;
	}

	private void ResetAxes()
	{
		centralAxis = 0f;
		lateralAxis = 0f;
		verticalAxis = 0f;
		horizontalAxis = 0f;
	}

	void Update()
	{
		if (update != null)
            update();
		if (cursorModeChanged)
		{
			Cursor.lockState = cursorMode;
			Cursor.visible = (cursorMode != CursorLockMode.Locked);
			cursorModeChanged = false;
		}
	}

	private void InGameUpdate()
	{
		centralAxis = Input.GetAxis("Vertical");
		lateralAxis = Input.GetAxis("Horizontal");
		verticalAxis = Input.GetAxis("Mouse Y");
		horizontalAxis = Input.GetAxis("Mouse X");
		if (Input.GetKeyDown(KeyCode.LeftShift))
			if (jumpKeyDown != null) jumpKeyDown();
		if (Input.GetKeyDown(KeyCode.Space))
			if (spellCastKeyDown != null) spellCastKeyDown();
		if (Input.GetKeyDown(KeyCode.E))
			if (interractKeyDown != null) interractKeyDown();
		if (Input.GetKeyDown(KeyCode.Tab))
			if (inventoryKeyDown != null) inventoryKeyDown();
        if (Input.GetMouseButtonDown(0))
            if (attackKeyDown != null) attackKeyDown();
        if (Input.GetMouseButton(1))
            if (defenseKeyDown != null) defenseKeyDown();
        if (Input.GetMouseButtonUp(1))
            if (defenseKeyUp != null) defenseKeyUp();
		if (Input.GetKeyDown(KeyCode.Alpha1))
			if (selectSpellKeyDown != null) selectSpellKeyDown(0);
		if (Input.GetKeyDown(KeyCode.Alpha2))
			if (selectSpellKeyDown != null) selectSpellKeyDown(1);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.Instance.PauseGame();
			UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Hide);
			UIManager.Instance.Notify(UIRequest.PauseMenu, UIRequestMode.Show);
		}
	}

	private void InventoryUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
			if (inventoryKeyDown != null) inventoryKeyDown();

		if (Input.GetKeyDown(KeyCode.E))
            if (inventoryAction1 != null) inventoryAction1();
        if (Input.GetKeyDown(KeyCode.R))
            if (inventoryAction2 != null) inventoryAction2();
    }
}
