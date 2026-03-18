using UnityEngine;

/// <summary>
/// Moves CharacterControllers within an explosion.
/// </summary>
public class ExplosionForceCc : MonoBehaviour
{
	[SerializeField] private float _force = 10f;

	/// <summary>
	/// Calculates the direction and moves a given collider's CharacterController.
	/// </summary>
	/// <param name="col">The collider to apply movement to.</param>
	public void Apply(Collider col)
	{
		var cc = col.GetComponent<CharacterController>();

		if (cc == null)
			return;

		Vector3 direction = (col.transform.position - transform.position).normalized;
		cc.Move(direction * _force * Time.deltaTime);
	}
}
