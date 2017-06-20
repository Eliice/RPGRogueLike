using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState {

    private readonly StatePatternEnemy enemy;
    private int nextPatrolPoint = 0;

    public void UpdateState()
    {
        Patrol();
    }

    public void ToPatrolState()
    {        // cannot // 
    }

    public void ToAlerteState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState()
    {   // cannot //
    }

    public void OnTriggerEnter(Collider other)
    { }

    public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        Look(other.transform);
    }

    public PatrolState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }
        
    void Look(Transform target)
    {
        // proper Y
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
     
    private void Patrol()
    {
        if (enemy.patrolPoints.Length == 0)
        {
            ToAlerteState();
            return;
        }
        if (nextPatrolPoint >= enemy.patrolPoints.Length)
        {
            nextPatrolPoint = 0;
        }
        enemy.navMeshAgent.destination = enemy.patrolPoints[nextPatrolPoint].position;
        enemy.navMeshAgent.Resume();
        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
            nextPatrolPoint += 1;
    }
}
