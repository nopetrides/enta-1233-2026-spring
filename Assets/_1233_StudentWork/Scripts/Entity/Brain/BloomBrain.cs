using UnityEngine;

// Brain for the Bloom enemy. Repositions to gain line-of-sight before shooting.
public class BloomBrain : EnemyBrainBase {
    [Header("Components")]
    [SerializeField] private DetectionSystem _detection;
    [SerializeField] private RotateToTarget _rotator;
    [SerializeField] private ProjectileWeapon _weapon;

    [Header("Settings")]
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private float _stopRange = 8f; // Stay back a bit


    public DetectionSystem Detection => _detection;
    public RotateToTarget Rotator => _rotator;

    public ProjectileWeapon Weapon => _weapon;

    public float AttackRange => _attackRange;
    public float StopRange => _stopRange;

    private void Start() {
        _stateMachine.Initialize(new BloomMoveState(this, _stateMachine));
    }
}