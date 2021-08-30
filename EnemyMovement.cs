using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    LayerMask _whatIsGround, _whatIsPlayer;
    [SerializeField]
    Transform _player;
    [SerializeField]
    float _walkPointRange = 5;
    [SerializeField]
    float _timeBetweenAttacks = 1f;
    [SerializeField]
    float _sightRange = 10, _attackRange = 1.2f;

    NavMeshAgent _navMeshAgent;
    Animator _animator;

    Vector3 _walkPoint;
    Vector3 _originalPosition;

    bool _walkPointSet = false;
    bool _alreadyAttacked;

    bool _isGoingToOriginalPosition = false;
    bool _isChasingAfterAttack = false;

    public bool IsGoingToOriginalPosition { get => _isGoingToOriginalPosition; set => _isGoingToOriginalPosition = value; }
    public bool IsChasingAfterAttack { get => _isChasingAfterAttack; set => _isChasingAfterAttack = value; }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _originalPosition = transform.position;
    }

    private void FixedUpdate()
    {
        float velocity = _navMeshAgent.velocity.magnitude / _navMeshAgent.speed;
        _animator.SetFloat("Speed", velocity);

        if (_isGoingToOriginalPosition)
        {
            GoToOriginalPosition();
        }
        else if (_isChasingAfterAttack)
        {
            ChasePlayer();
        }     
        else
        {
            bool playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
            bool playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

            Debug.Log(playerInSightRange + " " + playerInAttackRange);

            if (!playerInSightRange && !playerInAttackRange)
                Patroling();
            if (playerInSightRange && !playerInAttackRange)
                ChasePlayer();
            if (playerInSightRange && playerInAttackRange)
                AttackPlayer();
        }
    }

    public void Stop()
    {
        _navMeshAgent.SetDestination(transform.position);
    }

    private void AttackPlayer()
    {
        Debug.Log("attack");
        _navMeshAgent.SetDestination(transform.position);
        transform.LookAt(_player);

        if (!_alreadyAttacked)
        {
            _animator.SetTrigger("Attack");

            _player.gameObject.GetComponent<IDamageable<int>>().TakeDamage(1);

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    public void ChasePlayer()
    {
        Debug.Log("chase");
        _navMeshAgent.SetDestination(_player.position);
    }

    private void Patroling()
    {
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet)
            _navMeshAgent.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            _walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = UnityEngine.Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = UnityEngine.Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _whatIsGround))
            _walkPointSet = true;
    }

    public void GoToOriginalPosition()
    {
        IsChasingAfterAttack = false;

        _navMeshAgent.SetDestination(_originalPosition);
        Vector3 distanceToWalkPoint = transform.position - _originalPosition;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            IsGoingToOriginalPosition = false;
    }
}
