using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Moves NavMeshAgents within an explosion by temporarily disabling their pathfinding.
/// </summary>
public class ExplosionForceNma : MonoBehaviour
{
	[SerializeField] private float _force = 10f;
	[SerializeField] private float _knockbackDuration = 0.5f;

	private readonly Dictionary<NavMeshAgent, Coroutine> _activeKnockbacks = new Dictionary<NavMeshAgent, Coroutine>();

	/// <summary>
	/// Calculates the direction and applies a knockback force to a given collider's NavMeshAgent.
	/// </summary>
	/// <param name="col">The collider to apply movement to.</param>
	public void Apply(Collider col)
	{
		var agent = col.GetComponentInParent<NavMeshAgent>();

		if (agent == null)
			return;

		Vector3 direction = (col.transform.position - transform.position).normalized;

		if (_activeKnockbacks.TryGetValue(agent, out Coroutine existing))
			StopCoroutine(existing);

		_activeKnockbacks[agent] = StartCoroutine(KnockbackNavMeshAgent(agent, direction * _force));
	}

	/// <summary>
	/// Coroutine to handle NavMeshAgent knockback by temporarily disabling agent control.
	/// </summary>
	private IEnumerator KnockbackNavMeshAgent(NavMeshAgent agent, Vector3 force)
	{
		// Disable agent's own movement control
		agent.isStopped = true;
		agent.updatePosition = false;
		agent.updateRotation = false;

		float elapsed = 0f;
		while (elapsed < _knockbackDuration)
		{
			if (agent == null) yield break;

			// Move the agent manually based on the force
			float t = elapsed / _knockbackDuration;
			float currentForce = Mathf.Lerp(1f, 0f, t);
			
			agent.Move(force * currentForce * Time.deltaTime);
			
			elapsed += Time.deltaTime;
			yield return null;
		}

		if (agent != null)
		{
			agent.updatePosition = true;
			agent.updateRotation = true;
			agent.isStopped = false;
			_activeKnockbacks.Remove(agent);
		}
	}
}
