using UnityEngine;

public class EnemyBrainBase : MonoBehaviour {
	[Header("Components")]
	[SerializeField] protected EnemyStateMachine _stateMachine;

	[SerializeField] protected EnemyAnimatorDriver _animatorDriver;
	[SerializeField] protected Health _health;

    public IMover Mover { get; private set; }
    public ITargetProvider TargetProvider { get; private set; }
    public EnemyAnimatorDriver AnimatorDriver => _animatorDriver;

	private void Awake() {
        TargetProvider = GetComponent<ITargetProvider>();
        Mover = GetComponent<IMover>();

        if ( _stateMachine == null )
			_stateMachine = GetComponent<EnemyStateMachine>();

        if ( _animatorDriver == null )
            _animatorDriver = GetComponent<EnemyAnimatorDriver>();

        if ( _health == null )
            _health = GetComponent<Health>();
    }

    protected virtual void OnEnable() {
        if ( _health != null ) {
            _health.OnDamaged += HandleDamaged;
            _health.OnDied += HandleDied;
        }
    }

    protected virtual void OnDisable() {
        if ( _health != null ) {
            _health.OnDamaged -= HandleDamaged;
            _health.OnDied -= HandleDied;
        }
    }

    private void HandleDamaged(HealthModifyInfo _) {
        _animatorDriver.TriggerHit();
    }

    private void HandleDied() {
        if (_stateMachine != null)
            _stateMachine.ChangeState(new EnemyDeadState(this, _stateMachine));

        _animatorDriver.SetSpeed(0);
        _animatorDriver.TriggerDie();
        enabled = false;

        if ( Mover != null ) {
            Mover.Stop();
            Mover.SetEnabled(false);
        }
    }
}
