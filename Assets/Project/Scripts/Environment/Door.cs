using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    private Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = LevelManager.Instance.PlayerSpawn.position;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
            collision.transform.position = spawnPosition;
    }
}
