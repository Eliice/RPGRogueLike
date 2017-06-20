[System.Serializable]
public abstract class SavedObject
{
	public abstract void SaveData(object obj);
	public abstract void LoadData(object obj);
}