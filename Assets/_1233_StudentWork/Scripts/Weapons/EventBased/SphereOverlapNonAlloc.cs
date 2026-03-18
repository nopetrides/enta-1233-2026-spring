using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Performs a non-allocating sphere overlap check and invokes events for each detected collider.
/// </summary>
public class SphereOverlapNonAlloc : MonoBehaviour
{
	[SerializeField] private float _radius = 5f;
	[SerializeField] private LayerMask _layerMask;
	[SerializeField] private int _maxHits = 32;

	[SerializeField] private UnityEvent<Collider> _onColliderDetected;

	private Collider[] _results;

	private void Awake()
	{
		_results = new Collider[_maxHits];
	}

	/// <summary>
	/// Triggers the overlap check using Physics.OverlapSphereNonAlloc.
	/// </summary>
	public void CheckOverlap()
	{
		int count = Physics.OverlapSphereNonAlloc(
			transform.position,
			_radius,
			_results,
			_layerMask
		);
		// If we want an additional LoS check for the explosion,
		// we can do it here
		
		// Call events on each
		for (int i = 0; i < count; i++)
		{
			_onColliderDetected?.Invoke(_results[i]);
		}
	}
	
	/// <summary>
	/// Draws a wire sphere in the editor to visualize the overlap radius.
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _radius);
	}
}
