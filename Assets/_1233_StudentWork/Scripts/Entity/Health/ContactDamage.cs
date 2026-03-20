using UnityEngine;

public class ContactDamage : MonoBehaviour {
	[SerializeField] private int _damage = 10;
	[SerializeField] private float _cooldown = 1f;

	private float _nextDamageTime;

	private void OnCollisionEnter(Collision collision) {
		TryApplyDamage(collision.gameObject);
	}

	private void OnCollisionStay(Collision collision) {
		TryApplyDamage(collision.gameObject);
	}

	private void OnTriggerEnter(Collider other) {
		TryApplyDamage(other.gameObject);
	}

	private void OnTriggerStay(Collider other) {
		TryApplyDamage(other.gameObject);
	}

	private void TryApplyDamage(GameObject target) {
		if ( Time.time < _nextDamageTime )
			return;

		IDamageReceiver damageReceiver = target.GetComponent<IDamageReceiver>();
		if ( damageReceiver != null ) {
			HealthModifyInfo info = new HealthModifyInfo {
				Source = gameObject,
				Amount = _damage,
				HitPos = target.transform.position,
				HitNormal = Vector3.up,
			};
			damageReceiver.ReceiveDamage(info);
			_nextDamageTime = Time.time + _cooldown;
		}
	}
}
