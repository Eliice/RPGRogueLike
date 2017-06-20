using UnityEngine;
using System.Collections;

public class AlertState : IEnemyState {

    private StatePatternEnemy enemy;
    private float searchTimer = 0f;

    public void UpdateState()
    {
        Search();
        Look(enemy.chaseTarget);
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
        searchTimer = 0f;
        enemy.chaseTarget = null;
    }

    public void ToAlerteState()
    {   //  cannot //
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
        searchTimer = 0f;
    }

    public void ToAttackState()
    {
    }

    public void OnTriggerEnter(Collider other)
    { }

    public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        Look(other.transform);
    }

    public AlertState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    private void Search()
    {
        enemy.navMeshAgent.Stop();
        enemy.transform.Rotate(Vector3.up* (enemy.rotationSpeed * Time.deltaTime));
        searchTimer += Time.deltaTime;
        if (searchTimer >= enemy.searchDuration)
            ToPatrolState();
    }

    private void Look(Transform target)
    {
        if (!target)
            return;
        Vector3 dir = target.position - enemy.eyes.position;
        float angle = Vector3.Angle(dir, enemy.eyes.forward);

        if (angle < enemy.fieldOfView / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(enemy.eyes.position, dir.normalized, out hit, enemy.sightDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    enemy.chaseTarget = hit.transform;
                    ToChaseState();
                }
            }
        }
    }
}