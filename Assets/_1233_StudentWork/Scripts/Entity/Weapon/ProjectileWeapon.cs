using UnityEngine;

public class ProjectileWeapon : MonoBehaviour, IWeapon {
	[SerializeField] private Projectile _projectilePrefab;
	[SerializeField] private Transform _muzzle;
	[SerializeField] private float _fireRate = 1f;
	[SerializeField] private float _arcHeight = 2f;

	private float _nextFireTime;

	public bool CanFire => Time.time >= _nextFireTime;

	public void Fire(Quaternion direction) {
		if ( !CanFire )
			return;
		_nextFireTime = Time.time + 1f / _fireRate;
		SpawnProjectile(direction);
	}

	public void Fire(Vector3 targetPosition) {
		// Direct fire by default, or could be configured for arc
		Vector3 direction = ( targetPosition - _muzzle.position ).normalized;
		Fire(Quaternion.LookRotation(direction));
	}

	public void FireArc(Vector3 targetPosition) {
		if ( !CanFire )
			return;
		_nextFireTime = Time.time + 1f / _fireRate;
		Vector3 velocity = CalculateArcVelocity(_muzzle.position, targetPosition, _arcHeight);
		Projectile projectile = Instantiate(_projectilePrefab, _muzzle.position, _muzzle.rotation);
		projectile.LaunchWithVelocity(velocity, gameObject);
	}

	private void SpawnProjectile(Quaternion direction) {
		Projectile projectile = Instantiate(_projectilePrefab, _muzzle.position, direction);
		projectile.Launch(direction * Vector3.forward, gameObject);
	}

	private Vector3 CalculateArcVelocity(Vector3 start, Vector3 end, float height) {
		float displacementY = end.y - start.y;
		Vector3 displacementXZ = new Vector3(end.x - start.x, 0, end.z - start.z);
		float gravity = Physics.gravity.y;

		float time =
			Mathf.Sqrt(-2 * height / gravity) +
			Mathf.Sqrt(Mathf.Abs(2 * ( displacementY - height ) / gravity));

		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
		Vector3 velocityXZ = displacementXZ / time;

		return velocityXZ + velocityY * -Mathf.Sign(gravity);
	}

	//private Vector3 CalculateArcVelocity(Vector3 start, Vector3 end, float height) {
	//	float apexY = Mathf.Max(start.y, end.y) + height;
	//	float dyUp = apexY - start.y;
	//	float dyDown = apexY - end.y;
	//	float vy0 = Mathf.Sqrt(-2f * Physics.gravity.y * dyUp);
	//	float tUp = -vy0 / Physics.gravity.y;
	//	float tDown = Mathf.Sqrt(2f * ( end.y - apexY ) / Physics.gravity.y);
	//	float tTotal = tUp + tDown;

	//	Vector3 to = end - start;
	//	float vx = to.x / tTotal;
	//	float vz = to.z / tTotal;

	//	return new Vector3(vx, vy0, vz);
	//}
}
