using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	#region singleton
	private static LevelManager instance;
	public static LevelManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<LevelManager>();
#if UNITY_EDITOR
				if (instance == null)
					Debug.LogError("instance == null");
#endif
			}
			return instance;
		}
	}
	#endregion

	private Transform playerSpawn;
    public Transform PlayerSpawn
    {
        get { return playerSpawn; }
        set { playerSpawn = value; }
    }

    private List<GameObject> mark;
    public List<GameObject> Mark
    {
        get { return mark; }
    }

    private Player player;
	private Compass compass = null;

    void Awake()
    {
        mark = new List<GameObject>();
        Cursor.lockState = CursorLockMode.Locked;
		compass = GameObject.FindGameObjectWithTag("Compass").GetComponent<Compass>();
    }

    void Start()
    {
		player = GameManager.Instance.Player;
		if (GameManager.Instance.State == GameState.NewGame)
		{
			player.transform.position = playerSpawn.position;
		}
    }

    public void AddMainObject(GameObject newMark)
    {
        mark.Add(newMark);
        compass.EditMarkList(newMark);
    }

    public void RemoveObject(GameObject objectToRemove)
    {
        if (mark.Contains(objectToRemove))
        {
            mark.Remove(objectToRemove);
			compass.EditMarkList(objectToRemove);
            LevelComplete();
        }
    }

    private void LevelComplete()
    {

        if(mark.Count == 4)
        {
            GameManager.Instance.LevelDone();
            SceneManager.LoadScene("GameScene");
            player.gameObject.transform.position = playerSpawn.position;
        }
    }
}