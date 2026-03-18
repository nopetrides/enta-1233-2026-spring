using UnityEngine;

/// <summary>
/// Handles damage application to entities within an explosion radius.
/// </summary>
public class ExplosionDamageReceiver : MonoBehaviour
{
	[SerializeField] private int _damage = 1;

	/// <summary>
	/// Calculates damage info and applies it to a detected collider.
	/// </summary>
	/// <param name="col">The collider to apply damage to.</param>
	public void Apply(Collider col)
	{
		if (!col.TryGetComponent(out IDamageReceiver receiver))
			return;

		Vector3 explosionCenter = transform.position;

		Vector3 hitPoint = col.ClosestPoint(explosionCenter);
		Vector3 hitNormal = (hitPoint - explosionCenter).normalized;

		var info = new DamageInfo
		{
			Amount = _damage,
			Source = gameObject,
			HitPoint = hitPoint,
			HitNormal = hitNormal
		};

		receiver.ApplyDamage(info);
	}
}
