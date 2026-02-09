using UnityEngine;
using UnityEngine.AI;

/// <summary>
///     Implementation of IMover using Unity's NavMeshAgent.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMover : MonoBehaviour, IMover
{
	[SerializeField] private NavMeshAgent _agent;

	private void Awake()
	{
		if (_agent == null)
			_agent = GetComponent<NavMeshAgent>();
	}

	public Vector3 Velocity => _agent.velocity;
	public float RemainingDistance => _agent.remainingDistance;
	public bool IsAtDestination => !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;

	public void SetDestination(Vector3 destination)
	{
		if (!_agent.enabled || !_agent.isOnNavMesh) return;
		_agent.SetDestination(destination);
	}

	public void Stop()
	{
		if (!_agent.enabled) return;
		_agent.isStopped = true;
	}

	public void Resume()
	{
		if (!_agent.enabled) return;
		_agent.isStopped = false;
	}

	public void SetEnabled(bool value)
	{
		_agent.enabled = value;
	}
}
