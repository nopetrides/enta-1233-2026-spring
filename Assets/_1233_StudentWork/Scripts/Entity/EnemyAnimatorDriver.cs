using UnityEngine;

public class EnemyAnimatorDriver : MonoBehaviour {
	private static readonly int SpeedHash = Animator.StringToHash("Speed");
	private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
	private static readonly int Attack1TriggerHash = Animator.StringToHash("Attack1");
	private static readonly int Attack2TriggerHash = Animator.StringToHash("Attack2");
	private static readonly int HitTriggerHash = Animator.StringToHash("Hit");
	private static readonly int DieTriggerHash = Animator.StringToHash("Die");
	[SerializeField] private Animator _animator;

	void Awake() {
		if ( _animator == null )
			_animator = GetComponentInChildren<Animator>();
	}

	public void SetSpeed(float speed) {
		_animator.SetFloat(SpeedHash, speed);
		_animator.SetBool(IsMovingHash, speed > 0.1f);
	}

	public void TriggerAttack1() {
		_animator.SetTrigger(Attack1TriggerHash);
	}

	public void TriggerAttack2() {
		_animator.SetTrigger(Attack2TriggerHash);
	}

	public void TriggerHit() {
		_animator.SetTrigger(HitTriggerHash);
	}

	public void TriggerDie() {
		_animator.SetTrigger(DieTriggerHash);
	}

}
