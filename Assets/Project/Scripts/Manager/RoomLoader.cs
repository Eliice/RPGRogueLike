using UnityEngine;

public class RoomLoader : MonoBehaviour {

    private int nbPrefab = 7;// care with this value, if too hight, can cause crash

    void Awake()
    {
        if(transform.parent.gameObject.name == "Start")
        {
            GameObject roomPrefab = Instantiate(Resources.Load<GameObject>("LoadedPrefab/Room/Start/Start1"));
            roomPrefab.transform.position = transform.position;
            LevelManager.Instance.PlayerSpawn = transform;
        }
        else if (transform.parent.gameObject.name == "End")
        {
            GameObject roomPrefab = Instantiate(Resources.Load<GameObject>("LoadedPrefab/Room/End/End1"));
            roomPrefab.transform.position = transform.position;
        }
        else
        {
            int random = Random.Range(0, nbPrefab) + 1;
            string path = "LoadedPrefab/Room/Random/Random" + random.ToString();
            GameObject roomPrefab = Instantiate(Resources.Load<GameObject>(path));
            roomPrefab.transform.position = transform.position;
        }
    }
}
