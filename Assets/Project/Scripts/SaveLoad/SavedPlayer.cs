using UnityEngine;

[System.Serializable]
public class SavedPlayer : SavedObject
{
	int level;
	int xp;
	int strength;
	int constitution;
	int intelligence;
	int dexterity;
	float positionX;
	float positionY;
	float positionZ;
	float rotationY;
	SavedInventory inventory;

	public override void SaveData(object obj)
	{
		Player player = obj as Player;
		level = player.GetCharac().level;
		xp = player.Xp;
		strength = player.GetCharac().strength;
		constitution = player.GetCharac().constitution;
		intelligence = player.GetCharac().intelligence;
		dexterity = player.GetCharac().dexterity;
		positionX = player.transform.position.x;
		positionY = player.transform.position.y;
		positionZ = player.transform.position.z;
		rotationY = player.transform.rotation.eulerAngles.y;
		inventory = new SavedInventory();
		inventory.SaveData(player.Inventory);
	}

	public override void LoadData(object obj)
	{
		Player player = obj as Player;
		player.GetCharac().level = level;
		player.Xp = xp;
		player.GetCharac().strength = strength;
		player.GetCharac().constitution = constitution;
		player.GetCharac().intelligence = intelligence;
		player.GetCharac().dexterity = dexterity;
		player.transform.position = new Vector3(positionX, positionY, positionZ);
		player.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
		inventory.LoadData(player.Inventory);
	}
}