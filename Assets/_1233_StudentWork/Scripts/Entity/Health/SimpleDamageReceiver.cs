using UnityEngine;

public class SimpleDamageReceiver : MonoBehaviour, IDamageReceiver {

	[SerializeField] private Health _health;
	[SerializeField] private float _damageMultiplier = 1f;

	void Awake() {
		if ( _health == null )
			_health = GetComponentInParent<Health>();
	}

	public void ReceiveDamage(HealthModifyInfo info) {
		if ( _health == null )
			return;

		info.Amount = Mathf.RoundToInt(info.Amount * _damageMultiplier);
		_health.TakeDamage(info);
	}

}