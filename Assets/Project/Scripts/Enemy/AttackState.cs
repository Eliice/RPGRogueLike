using UnityEngine;

public class AttackState : IEnemyState {

    private StatePatternEnemy enemy;
    private bool attacked = false;

    public void UpdateState()
    {
        Attack();
    }

    public void ToPatrolState()
    { // when target killed ? //
    }

    public void ToAlerteState()
    { // depends on chase state // 
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
        attacked = false;
    }

    public void ToAttackState()
    { // cannot //
    }

    public void OnTriggerEnter(Collider other)
    { }

    public void OnTriggerStay(Collider other)
    { }

    public AttackState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void Attack()
    {
        if (enemy.iAttack != null)
            WeaponAttack();
        else
            CastSpell();
        enemy.transform.LookAt(enemy.chaseTarget.position);
        enemy.navMeshAgent.Stop();
    }

    void CastSpell()
    {
        if (enemy.iCaster.IsCasting())
            return;
        else if (attacked)
        {
            ToChaseState();
            return;
        }
        enemy.iCaster.Use();
        attacked = true;
    }

    void WeaponAttack()
    {
        if (enemy.iAttack.IsAttacking())
            return;
        else if (attacked)
        {
            ToChaseState();
            return;
        }
        enemy.iAttack.Attack();
        attacked = true;
    }
}
