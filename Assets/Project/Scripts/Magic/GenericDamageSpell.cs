using UnityEngine;
using System.Collections;

public class GenericDamageSpell : MonoBehaviour {

    GameObject particlePrefabObj;
    public int damages;

	void Start () {
        Destroy(gameObject, 50);
        particlePrefabObj = Resources.Load("Prefabs/ParticleSystem/DamageSpellCollisionParticle") as GameObject;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject clone = Instantiate(particlePrefabObj);
        clone.transform.position = transform.position;
        ParticleSystem collisionParticle = clone.GetComponent<ParticleSystem>();
        collisionParticle.Play();
        Destroy(clone, collisionParticle.duration);
        Destroy(gameObject);
        IDamageable iDamage = collision.transform.GetComponent<IDamageable>();
        if (iDamage != null)
        {
            iDamage.takeDamage(damages);
        }
    }
}
