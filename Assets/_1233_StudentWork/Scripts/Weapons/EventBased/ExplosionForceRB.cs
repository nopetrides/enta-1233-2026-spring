using UnityEngine;

/// <summary>
/// Applies physics-based forces to Rigidbodies within an explosion.
/// </summary>
public class ExplosionForceRb : MonoBehaviour
{
	[SerializeField] private float _force = 10f;

	/// <summary>
	/// Calculates the direction and applies force to a given collider's Rigidbody.
	/// </summary>
	/// <param name="col">The collider to apply force to.</param>
	public void Apply(Collider col)
	{
		var rb = col.attachedRigidbody;

		if (rb == null)
			return;

		Vector3 direction = (col.transform.position - transform.position).normalized;
		rb.AddForce(direction * _force, ForceMode.Impulse);
	}
}
