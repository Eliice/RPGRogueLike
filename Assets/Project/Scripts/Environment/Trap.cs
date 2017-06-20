using UnityEngine;

public class Trap : MonoBehaviour {

	[SerializeField] private int damage = 50;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<GenericCharacter>().takeDamage(damage);
        }
    }
}
