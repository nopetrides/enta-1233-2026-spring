using UnityEngine;

public interface IMover {
	Vector3 Velocity { get; }
	float RemainingDistance { get; }
	bool IsAtDestination { get; }
	void SetDestination(Vector3 desination);
	void Stop();
	void Resume();
	void SetEnabled(bool enabled);
}