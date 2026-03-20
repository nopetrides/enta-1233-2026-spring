using UnityEngine;

public class PlayerTargetProvider : MonoBehaviour, ITargetProvider {
	[SerializeField] private Vector3 _offset = new(0, 1f, 0);

	public bool HasTarget =>
		PlayerService.Instance != null &&
		PlayerService.Instance.GetPlayers().Length > 0 &&
		PlayerService.Instance.GetPlayers()[0].Character != null;

	public Transform GetTarget() {
		if ( HasTarget )
			return PlayerService.Instance.GetPlayers()[0].Character.Collider.transform;
		return null;
	}

	public Vector3 GetTargetPosition() {
		if ( HasTarget )
			return PlayerService.Instance.GetPlayers()[0].Character.Collider.transform.position + _offset;
		return transform.position;
	}
}
