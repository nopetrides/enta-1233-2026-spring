using UnityEngine;

public class DetectionSystem : MonoBehaviour {
	[SerializeField] private Transform _eyePosition;
	[SerializeField] private float _detectionRange = 15f;
	[SerializeField] private float _fieldOfView = 120f;
	[SerializeField] private LayerMask _obstructionMask;

	public float DetectionRange => _detectionRange;

	void Awake() {
		if ( _eyePosition == null )
			_eyePosition = transform;
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, _detectionRange);

		Vector3 leftDir = Quaternion.Euler(0, -_fieldOfView / 2, 0) * transform.forward;
		Vector3 rightDir = Quaternion.Euler(0, _fieldOfView / 2, 0) * transform.forward;
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, leftDir * _detectionRange);
		Gizmos.DrawRay(transform.position, rightDir * _detectionRange);
	}

	public bool IsTargetInDetectionRange(Transform target) {
		if ( target == null )
			return false;
		return (transform.position - target.position).magnitude <= _detectionRange;
	}

	public bool HasLineOfSight(Transform target) {
		if (target == null) return false;

		Vector3 toTarget = ( target.position - _eyePosition.position );
		if (Vector3.Angle(transform.forward, toTarget.normalized) > _fieldOfView / 2f) return false;

		if ( Physics.Raycast(_eyePosition.position, toTarget.normalized, out var hit, toTarget.magnitude, _obstructionMask) )
			if ( hit.transform != target && !hit.transform.IsChildOf(target) )
				return false;

		return true;
	}
}
