using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Compass : MonoBehaviour {

    private GameObject wayPoint;
    private GameObject North;
    private GameObject South;
    private GameObject East;
    private GameObject West;
    private Dictionary<GameObject, GameObject> mark;

    void Awake()
    {
        wayPoint = Resources.Load<GameObject>("LoadedPrefab/GUI/CompassMark/WayPointMark");
        North = Resources.Load<GameObject>("LoadedPrefab/GUI/CompassMark/North");
        South = Resources.Load<GameObject>("LoadedPrefab/GUI/CompassMark/South");
        East = Resources.Load<GameObject>("LoadedPrefab/GUI/CompassMark/East");
        West = Resources.Load<GameObject>("LoadedPrefab/GUI/CompassMark/West");
        mark = new Dictionary<GameObject, GameObject>();
    }

	void Start () {
        StartCoroutine(locator());
	}
   
    void Locate()
    {
        foreach (GameObject marked in mark.Keys)
        {
            Vector3 objecPos = marked.transform.position;
            objecPos.y = 0;
            Vector3 self = GameManager.Instance.Player.transform.forward;
            self.y = 0;
            Vector3 distance = objecPos - GameManager.Instance.Player.transform.position;
            float angle = Vector3.Angle(self, distance);
            angle *= -Mathf.Sign(Vector3.Cross(distance, self).y);
            Display(angle, mark[marked]);
        }
    }

    void OnEnable()
    {
        StartCoroutine(locator());
    }

    void Display(float angle, GameObject marker )
    {
        if (angle >= -40 && angle <= 40)
        {
            marker.SetActive(true);
            Vector3 newPos = marker.transform.position;
            newPos.x =  Screen.width / 2 + angle * 5;
            marker.transform.position = newPos;
        }
        else
            marker.SetActive(false);
    }

    private IEnumerator locator()
    {
        while (true)
        {
            Locate();
            yield return new WaitForSeconds(.05f);
        }
    }

    public void EditMarkList(GameObject obj) // if obj already exist : remove else add to the dictionary
    {
        if (mark.ContainsKey(obj) && mark[obj] != null)
        {
            Destroy(mark[obj].gameObject);
            mark.Remove(obj);
        }
        else
        {
            GameObject marker;
            LoadModel(obj, out marker);
            marker.transform.SetParent(transform);
            marker.transform.position = transform.position;
            marker.SetActive(false);
            mark.Add(obj, marker);
        }
    }

    void LoadModel(GameObject obj, out GameObject marker)
    {
        switch (obj.name)
        {
            case "North":
                {
                    marker = Instantiate(North);
                    break;
                }
            case "South":
                {
                    marker = Instantiate(South);
                    break;
                }
            case "East":
                {
                    marker = Instantiate(East);
                    break;
                }
            case "West":
                {
                    marker = Instantiate(West);
                    break;
                }
            default:
                {
                    marker = Instantiate(wayPoint);
                    break;
                }
        }
    }
}
