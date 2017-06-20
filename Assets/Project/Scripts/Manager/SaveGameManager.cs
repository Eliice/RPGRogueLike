using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
	#region singleton
	static private SaveGameManager instance = null;
	static public SaveGameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<SaveGameManager>();
#if UNITY_EDITOR
				if (instance == null)
					Debug.Log("instance == null");
#endif
			}

			return instance;
		}
	}
	#endregion

	public void SaveGame()
	{
		SavedPlayer player = new SavedPlayer();
		player.SaveData(GameManager.Instance.Player);
		FileStream saveFile = File.Open("savegame", FileMode.Create);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(saveFile, player);
		saveFile.Close();
	}

	public void LoadGame()
	{
		FileStream saveFile;
		try
		{
			saveFile = File.Open("savegame", FileMode.Open);
		}
		catch (FileNotFoundException exception)
		{
#if UNITY_EDITOR
			Debug.Log("FileNotFoundException : " + exception.Message);
#endif
			return;
		}

		BinaryFormatter bn = new BinaryFormatter();
		SavedPlayer player = bn.Deserialize(saveFile) as SavedPlayer;
		saveFile.Close();
		if (player == null)
		{
#if UNITY_EDITOR
		Debug.Log("player == null");
#endif
			return;
		}
		player.LoadData(GameManager.Instance.Player);
	}
}