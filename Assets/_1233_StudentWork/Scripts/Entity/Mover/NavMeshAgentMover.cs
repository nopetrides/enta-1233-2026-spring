using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public sealed class NavMeshAgentMover : MonoBehaviour, IMover {
	[SerializeField] private NavMeshAgent _agent;

	private const float EPSILON = 0.01f;

	public Vector3 Velocity => _agent.velocity;
	public bool HasPath => _agent.hasPath;
	public float RemainingDistance => _agent.remainingDistance;
	public bool IsAtDestination => RemainingDistance <= EPSILON;

	public void Resume() {
		throw new System.NotImplementedException();
	}

	public void SetDestination(Vector3 destination) {
		_agent?.SetDestination(destination);
	}

	public void SetEnabled(bool enabled) {}

	public void Stop() {
		_agent?.ResetPath();
	}
}
