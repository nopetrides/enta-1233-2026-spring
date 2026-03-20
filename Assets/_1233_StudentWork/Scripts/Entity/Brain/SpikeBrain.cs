using UnityEngine;

// Brain for the Spike enemy. Focuses on patrol movement and contact damage.
public class SpikeBrain : EnemyBrainBase {
    [SerializeField] private PatrolMotor _patrolMotor;
    [SerializeField] private ContactDamage _contactDamage;

    private void Update() {
        if ( _health != null && _health.IsDead )
            return;

        // Update animator based on mover velocity
        if ( _animatorDriver != null && Mover != null )
            _animatorDriver.SetSpeed(Mover.Velocity.magnitude);
    }

    protected override void OnEnable() {
        base.OnEnable();
        if ( _health != null ) {
            _health.OnDied += HandleDied;
        }
    }

    protected override void OnDisable() {
        base.OnDisable();
        if ( _health != null ) {
            _health.OnDied -= HandleDied;
        }
    }

    private void HandleDied() {
        _patrolMotor.enabled = false;
        _contactDamage.enabled = false;
    }
}