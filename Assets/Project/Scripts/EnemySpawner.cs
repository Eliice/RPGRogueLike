using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject[] enemysSpawnable;
    private GameObject enemy;
    [SerializeField]
    private bool respawn = false;

    void Start()
    {
        Spawn();
    }

	void Spawn () {
        enemy = Instantiate(enemysSpawnable[Random.Range(0, enemysSpawnable.Length)]);
        enemy.transform.SetParent(GetComponentInParent<Transform>());
        enemy.transform.position = transform.position;
        if (respawn)
            StartCoroutine(Respawn());
	}

    private IEnumerator Respawn()
    {
        while (enemy != null)
        {
            yield return new WaitForSeconds(20f);
        }
        Spawn();
    }
}
