using UnityEngine;

public class StatePatternEnemy : MonoBehaviour, IPausable
{

    public Transform    eyes;           //  Init in Editor
    public Transform[]  patrolPoints;   //  Init in Editor
    public float        sightDistance;
    public float        fieldOfView;
    public float        speed;
    public float        searchDuration;
    public float        rotationSpeed;


    [HideInInspector]   public IAttack  iAttack = null;
    [HideInInspector]   public ICaster  iCaster = null;
    [HideInInspector]   public float    attackRange;

    public NavMeshAgent navMeshAgent;
    public Transform    chaseTarget;

    [HideInInspector] public IEnemyState  currentState;
    [HideInInspector] public PatrolState  patrolState;
    [HideInInspector] public ChaseState   chaseState;
    [HideInInspector] public AlertState   alertState;
    [HideInInspector] public AttackState  attackState;

    void Awake()
    {
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
        GetComponent<SphereCollider>().radius = sightDistance;
        navMeshAgent.speed = speed;
        Enemy enemy = GetComponent<Enemy>();
        switch (enemy.race)
        {
            case Enemy.Type.Warrior:
                {
                    iAttack = GetComponent<IAttack>();
                    attackRange = GetComponentInChildren<Weapon>().range;
                }
                break;
            case Enemy.Type.Wizard:
                {
                    iCaster = GetComponent<ICaster>();
                    attackRange = 7; // tmp
                    break;
                }
            default: break;
        }
        currentState = patrolState;

		IsPaused = false;
		GameManager.Instance.pauseEvent += Pause;
		GameManager.Instance.unpauseEvent += Unpause;
	}

    void Update()
    {
		if (!IsPaused)
			currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

	#region IPausable
	public bool IsPaused { get; set; }
	public void Pause()
	{
		IsPaused = true;
	}
	public void Unpause()
	{
		IsPaused = false;
	}
	public void TogglePause()
	{
		if (IsPaused)
			Unpause();
		else
			Pause();
	}
	#endregion
}
