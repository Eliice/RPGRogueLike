using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState {

    StatePatternEnemy enemy;

    public void UpdateState()
    {
        Chase();
    }

    public void ToPatrolState()
    {   // cannot //
    }

    public void ToAlerteState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {   // cannot //
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void OnTriggerEnter(Collider other)
    { }

    public void OnTriggerStay(Collider other)
    { }

    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    void Chase()
    {
        RaycastHit hit;
        Vector3 dir = enemy.chaseTarget.position - enemy.eyes.position;
        if (Physics.Raycast(enemy.eyes.position, dir, out hit, enemy.sightDistance) && hit.collider.CompareTag("Player"))
        {

            enemy.chaseTarget = hit.transform;
            enemy.navMeshAgent.destination = enemy.chaseTarget.position;
            enemy.navMeshAgent.Resume();
            float dist = Vector3.Distance(enemy.transform.position, hit.transform.position);
            if (dist <= enemy.attackRange)
                ToAttackState();
        }
        else
            ToAlerteState();
    }
}