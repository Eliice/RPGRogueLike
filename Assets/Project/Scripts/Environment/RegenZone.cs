using UnityEngine;

public class RegenZone : MonoBehaviour {

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().RegenBoost = true;
            collision.gameObject.GetComponent<Magic>().RegenBoost = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().RegenBoost = false;
            collision.gameObject.GetComponent<Magic>().RegenBoost = false;
        }
    }
}
