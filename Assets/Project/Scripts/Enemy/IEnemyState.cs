using UnityEngine;
using System.Collections;

public interface IEnemyState {

    void UpdateState();

    void ToPatrolState();

    void ToAlerteState();

    void ToChaseState();

    void ToAttackState();

    void OnTriggerEnter(Collider other);

    void OnTriggerStay(Collider other);
}
