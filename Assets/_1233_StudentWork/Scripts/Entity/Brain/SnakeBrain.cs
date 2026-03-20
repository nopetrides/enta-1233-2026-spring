using UnityEngine;

// Brain for the Snake enemy. Uses a state machine to switch between Chase and Attack.
public class SnakeBrain : EnemyBrainBase {
    [Header("Components")]
    [SerializeField] private DetectionSystem _detection;
    [SerializeField] private RotateToTarget _rotator;

    [Header("Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _attackDamage = 15;

    public DetectionSystem Detection => _detection;
    public RotateToTarget Rotator => _rotator;

    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public int AttackDamage => _attackDamage;

    private void Start() {
        _stateMachine.Initialize(new SnakeChaseState(this, _stateMachine));
    }
}