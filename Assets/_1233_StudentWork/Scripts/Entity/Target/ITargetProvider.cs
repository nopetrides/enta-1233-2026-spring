using UnityEngine;

public interface ITargetProvider {
	public bool HasTarget { get; }
	Transform GetTarget();
	Vector3 GetTargetPosition();
}
