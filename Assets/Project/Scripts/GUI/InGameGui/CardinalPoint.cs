using UnityEngine;

public class CardinalPoint : MonoBehaviour {

	void Start () {
        LevelManager.Instance.AddMainObject(gameObject);
    }
}
