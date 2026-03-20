using UnityEngine;

public class EnemyDeadState : EnemyState {
    private readonly EnemyBrainBase _brain;

    public EnemyDeadState(EnemyBrainBase brain, EnemyStateMachine machine)
        : base(machine) {
        _brain = brain;
    }
}