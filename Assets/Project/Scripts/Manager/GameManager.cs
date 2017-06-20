using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
	MainMenu = 0,
	NewGame,
	LoadingGame,
	InGame
}

public class GameManager : MonoBehaviour {

	#region singleton
	private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
#if UNITY_EDITOR
				if (instance == null)
                    Debug.LogError("instance == null");
#endif
			}
            return instance;
        }
    }
	#endregion

	private GameState state = GameState.MainMenu;
	public GameState State { get { return state; } }
	private Player player;
    [SerializeField]
    private int characPointsPerLevel = 6;
    private int levelComplet = 0;

    public Player Player
    {
        get { return player; }
    }
	
    public int CharacPointsPerLevel
    {
        get { return characPointsPerLevel; }
    }

    public int LevelComplet
    {
        get { return levelComplet; }
    }

	public delegate void PauseEventHandler();
	public event PauseEventHandler pauseEvent;
	public event PauseEventHandler unpauseEvent;

    void Awake()
    {
		GoToMainMenu();
    }

	public void GoToMainMenu()
	{
		if (state == GameState.InGame)
			DestroyPlayer();

		state = GameState.MainMenu;
		SceneManager.LoadScene("MainMenu");
	}

	public void NewGame()
	{
		if (state == GameState.InGame)
			DestroyPlayer();

		state = GameState.NewGame;
		InitNewPlayer();
		SceneManager.LoadScene("gameScene");
	}
	public void LoadGame()
	{
		state = GameState.LoadingGame;
		InitNewPlayer();
		SceneManager.LoadScene("gameScene");
	}
	public void ExitGame()
	{
		Application.Quit();
	}
	public void PauseGame()
	{
		pauseEvent();
	}
	public void UnpauseGame()
	{
		unpauseEvent();
	}

	void OnLevelWasLoaded()
	{
		if (state == GameState.MainMenu)
			StartCoroutine(ShowMenu(UIRequest.MainMenu));

		else if (state == GameState.NewGame || state == GameState.LoadingGame)
		{
			if (state == GameState.LoadingGame)
			{
				SaveGameManager.Instance.LoadGame();
				UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Show);
				StartCoroutine(ShowMenu(UIRequest.HUD));
			}
			else
				StartCoroutine(ShowMenu(UIRequest.LevelUp));
			state = GameState.InGame;
		}
	}

	private IEnumerator ShowMenu(UIRequest which)
	{
		yield return new WaitForEndOfFrame();
		UIManager.Instance.Notify(which, UIRequestMode.Show);
	}

	private void InitNewPlayer()
    {
		player = Instantiate(Resources.Load<GameObject>("loadedPrefab/Player").GetComponent<Player>());
		player.GetComponent<PlayerController>().SubscribeToInputManagerEvents();

        DontDestroyOnLoad(player.gameObject);
    }
	private void DestroyPlayer()
	{
		if (player != null)
		{
			player.GetComponent<PlayerController>().UnsubscribeToInputManagerEvents();
			Destroy(player.gameObject);
			player = null;
		}
	}

    public void initPlayer(int strength, int constitution, int intelligence, int dexterity)
    {
        player.GetCharac().strength = strength;
        player.GetCharac().constitution = constitution;
        player.GetCharac().intelligence = intelligence;
        player.GetCharac().dexterity = dexterity;
        player.RecalcHp();
    }

    public void LevelDone()
    {
        ++levelComplet;
    }
}
