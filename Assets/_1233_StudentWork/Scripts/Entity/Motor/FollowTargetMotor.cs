using UnityEngine;

[RequireComponent(typeof(ITargetProvider))]
public class FollowTargetMotor : Motor {

	[SerializeField] private float _minimumFollowDistance = 1f;

	protected ITargetProvider _targetProvider;

	protected virtual bool ShouldStop(Vector3 destination) {
		return _mover.RemainingDistance < _minimumFollowDistance;
	}

	protected override void Start() {
		base.Start();
		_targetProvider = GetComponent<ITargetProvider>();
	}

	protected void Update() {
		if ( !_targetProvider.HasTarget )
			return;

		Vector3 destination = _targetProvider.GetTarget().gameObject.transform.position;

		if ( ShouldStop(destination) ) {
			_mover.Stop();
			return;
		}
		
		float magnitude = ( gameObject.transform.position - destination ).magnitude;
		if ( magnitude > 1f ) {
			_mover.SetDestination(destination);
		}
	}
}