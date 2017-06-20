using UnityEngine;

public class Boot : MonoBehaviour
{
	[SerializeField]
	private GameObject managerHolderPrefab = null;

	void Awake()
	{
		managerHolderPrefab = Instantiate(managerHolderPrefab);
		DontDestroyOnLoad(managerHolderPrefab);
		Destroy(gameObject);
	}
}