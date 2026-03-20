using System;
using UnityEngine;

public struct HealthModifyInfo {
	public GameObject Source;
	public int Amount;
	public bool CanResurrect;

	public Vector3 HitPos;
	public Vector3 HitNormal;
	// We can add DamageType here later if needed
}

public class Health : MonoBehaviour {
	[SerializeField] private int _maxHealth = 100;
	[SerializeField] private bool _isInvulnerable;

	public int Current { get; private set; }
	public int Max => _maxHealth;
	public bool IsDead { get; private set; }

	void Awake() {
		ResetHealth();
	}

	public event Action<HealthModifyInfo> OnDamaged;
	public event Action OnDied;
	public event Action OnHealed;
	public event Action OnReset;

	public void ResetHealth() {
		Current = _maxHealth;
		IsDead = false;
		OnReset?.Invoke();
	}

	public void TakeDamage(HealthModifyInfo info) {
		if ( IsDead || _isInvulnerable )
			return;

		Current = Math.Max(Current - info.Amount, 0);
		OnDamaged?.Invoke(info);

		if ( Current <= 0 )
			Die();
	}

	public void Heal(int amount) {
		if ( IsDead )
			return;

		Current = Math.Min(Current + amount, _maxHealth);
		OnHealed?.Invoke();
	}

	private void Die() {
		IsDead = true;
		OnDied?.Invoke();
	}

	public void SetInvulnerable(bool invulnerable) {
		_isInvulnerable = invulnerable;
	}

}
