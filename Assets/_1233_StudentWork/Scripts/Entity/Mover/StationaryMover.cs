
using UnityEngine;

public class StationaryMover : MonoBehaviour, IMover {
	public Vector3 Velocity => Vector3.zero;
	public float RemainingDistance => 0f;
	public bool IsAtDestination => true;

	public void SetDestination(Vector3 destination) { }
	public void Stop() { }
	public void Resume() { }
	public void SetEnabled(bool value) { }
}
