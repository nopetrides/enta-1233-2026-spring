using UnityEngine;

/// <summary>
///     Brain for the Bloom enemy.
///     Repositions to gain line-of-sight before shooting.
/// </summary>
public class BloomBrain : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] private EnemyStateMachine _stateMachine;

	[SerializeField] private DetectionSystem _detection;
	[SerializeField] private EnemyAnimatorDriver _animatorDriver;
	[SerializeField] private RotateToTarget _rotator;
	[SerializeField] private Health _health;
	[SerializeField] private ProjectileWeapon _weapon;

	[Header("Settings")]
	[SerializeField] private float _attackRange = 10f;

	[SerializeField] private float _stopRange = 8f; // Stay back a bit

	public IMover Mover { get; private set; }

	public DetectionSystem Detection => _detection;
	public EnemyAnimatorDriver AnimatorDriver => _animatorDriver;
	public RotateToTarget Rotator => _rotator;
	public ITargetProvider TargetProvider { get; private set; }

	public ProjectileWeapon Weapon => _weapon;
	public float AttackRange => _attackRange;
	public float StopRange => _stopRange;

	private void Awake()
	{
		TargetProvider = GetComponent<ITargetProvider>();
		Mover = GetComponent<IMover>();
		if (_stateMachine == null) _stateMachine = GetComponent<EnemyStateMachine>();
	}

	private void Start()
	{
		_stateMachine.Initialize(new BloomMoveState(this, _stateMachine));
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
///     State for when Bloom is moving to gain Line of Sight or get in range.
/// </summary>
public class BloomMoveState : EnemyState
{
	private readonly BloomBrain _brain;

	public BloomMoveState(BloomBrain brain, EnemyStateMachine machine) : base(machine)
	{
		_brain = brain;
	}

	public override void Tick()
	{
		// 1. Get the player's position
		var target = _brain.TargetProvider.GetTarget();
		if (target == null) return;

		var distance = Vector3.Distance(_brain.transform.position, target.position);
		var hasLOS = _brain.Detection.HasLineOfSight(target);

		// 2. If we have LOS and are in range, switch to Attack state
		if (hasLOS && distance <= _brain.AttackRange)
		{
			Machine.ChangeState(new BloomAttackState(_brain, Machine));
			return;
		}

		// 3. Move toward target to regain LOS or get in range
		_brain.Mover?.SetDestination(target.position);

		// 4. Update animations based on movement speed
		if (_brain.Mover != null)
			_brain.AnimatorDriver.SetSpeed(_brain.Mover.Velocity.magnitude);
		else
			_brain.AnimatorDriver.SetSpeed(0);
	}
}

/// <summary>
///     State for when Bloom is attacking the player.
/// </summary>
public class BloomAttackState : EnemyState
{
	private readonly BloomBrain _brain;

	public BloomAttackState(BloomBrain brain, EnemyStateMachine machine) : base(machine)
	{
		_brain = brain;
	}

	public override void Enter()
	{
		// Stop moving to shoot
		_brain.Mover?.Stop();
		_brain.AnimatorDriver.SetSpeed(0);
	}

	public override void Tick()
	{
		// 1. Check if we still have a target
		var target = _brain.TargetProvider.GetTarget();
		var targetPos = _brain.TargetProvider.GetTargetPosition();
		if (target == null)
		{
			Machine.ChangeState(new BloomMoveState(_brain, Machine));
			return;
		}

		var distance = Vector3.Distance(_brain.transform.position, target.position);
		var hasLOS = _brain.Detection.HasLineOfSight(target);

		// 2. If LOS is lost or we are out of range, go back to Move state
		if (!hasLOS || distance > _brain.AttackRange)
		{
			Machine.ChangeState(new BloomMoveState(_brain, Machine));
			return;
		}

		// 3. Face the player and shoot if weapon is ready
		_brain.Rotator.FacePosition(targetPos);
		if (_brain.Weapon.CanFire)
		{
			_brain.AnimatorDriver.TriggerAttack();
			_brain.Weapon.Fire(targetPos);
		}

		// 4. Optional: If player gets too close, back away (Kite)
		if (distance < _brain.StopRange - 1f)
		{
			// Simple kite logic: move away from target
			var kiteDir = (_brain.transform.position - target.position).normalized;
			_brain.Mover?.SetDestination(_brain.transform.position + kiteDir * 2f);
		}
	}
}
