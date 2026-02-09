using UnityEngine;

/// <summary>
///     Brain for the Snake enemy.
///     Uses a state machine to switch between Chase and Attack.
/// </summary>
public class SnakeBrain : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] private EnemyStateMachine _stateMachine;

	[SerializeField] private DetectionSystem _detection;
	[SerializeField] private EnemyAnimatorDriver _animatorDriver;
	[SerializeField] private RotateToTarget _rotator;
	[SerializeField] private Health _health;

	[Header("Settings")]
	[SerializeField] private float _attackRange = 2f;

	[SerializeField] private float _attackCooldown = 2f;
	[SerializeField] private int _attackDamage = 15;

	public IMover Mover { get; private set; }

	public DetectionSystem Detection => _detection;
	public EnemyAnimatorDriver AnimatorDriver => _animatorDriver;
	public RotateToTarget Rotator => _rotator;
	public ITargetProvider TargetProvider { get; private set; }

	public float AttackRange => _attackRange;
	public float AttackCooldown => _attackCooldown;
	public int AttackDamage => _attackDamage;

	private void Awake()
	{
		TargetProvider = GetComponent<ITargetProvider>();
		Mover = GetComponent<IMover>();
		if (_stateMachine == null) _stateMachine = GetComponent<EnemyStateMachine>();
	}

	private void Start()
	{
		_stateMachine.Initialize(new SnakeChaseState(this, _stateMachine));
	}

	private void OnEnable()
	{
		if (_health != null) _health.OnDied += HandleDied;
	}

	private void OnDisable()
	{
		if (_health != null) _health.OnDied -= HandleDied;
	}

	private void HandleDied()
	{
		_stateMachine.ChangeState(null);
		if (Mover != null)
		{
			Mover.Stop();
			Mover.SetEnabled(false);
		}

		_animatorDriver.TriggerDie();
		enabled = false;
	}
}

/// <summary>
///     State for when the Snake is chasing the player.
/// </summary>
public class SnakeChaseState : EnemyState
{
	private readonly SnakeBrain _brain;

	public SnakeChaseState(SnakeBrain brain, EnemyStateMachine machine) : base(machine)
	{
		_brain = brain;
	}

	public override void Tick()
	{
		// 1. Get the player's position
		var target = _brain.TargetProvider.GetTarget();
		if (target == null) return;

		// 2. Tell the mover to go there
		_brain.Mover?.SetDestination(target.position);

		// 3. Update animations based on movement speed
		if (_brain.Mover != null)
			_brain.AnimatorDriver.SetSpeed(_brain.Mover.Velocity.magnitude);
		else
			_brain.AnimatorDriver.SetSpeed(0);

		// 4. If we are close enough, switch to Attack state
		var distance = Vector3.Distance(_brain.transform.position, target.position);
		if (distance <= _brain.AttackRange) Machine.ChangeState(new SnakeAttackState(_brain, Machine));
	}
}

/// <summary>
///     State for when the Snake is attacking the player.
/// </summary>
public class SnakeAttackState : EnemyState
{
	private readonly SnakeBrain _brain;
	private float _exitTime;

	public SnakeAttackState(SnakeBrain brain, EnemyStateMachine machine) : base(machine)
	{
		_brain = brain;
	}

	public override void Enter()
	{
		// Stop moving and trigger the attack animation
		_brain.Mover?.Stop();
		_brain.AnimatorDriver.SetSpeed(0);
		_brain.AnimatorDriver.TriggerAttack();

		// Calculate when we can leave this state
		_exitTime = Time.time + _brain.AttackCooldown;

		// Apply damage immediately (simplified)
		ApplyMeleeDamage();
	}

	public override void Tick()
	{
		// Keep facing the player during the attack
		var target = _brain.TargetProvider.GetTarget();
		var targetPos = _brain.TargetProvider.GetTargetPosition();
		if (target != null) _brain.Rotator.FacePosition(targetPos);

		// Return to chase state once the cooldown is over
		if (Time.time >= _exitTime) Machine.ChangeState(new SnakeChaseState(_brain, Machine));
	}

	private void ApplyMeleeDamage()
	{
		var target = _brain.TargetProvider.GetTarget();
		if (target == null) return;

		// Final check to see if target is still in range
		if (Vector3.Distance(_brain.transform.position, target.position) <= _brain.AttackRange + 0.5f)
		{
			var receiver = target.GetComponent<IDamageReceiver>();
			if (receiver != null)
				receiver.ApplyDamage(
					new DamageInfo
					{
						Amount = _brain.AttackDamage,
						Source = _brain.gameObject,
						HitPoint = target.position,
						HitNormal = Vector3.up
					});
		}
	}
}
