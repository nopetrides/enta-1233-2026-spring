using UnityEngine;

public interface IWeapon {
	bool CanFire { get; }
	void Fire(Quaternion direction);
	void Fire(Vector3 targetPosition);
}
