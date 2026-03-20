using System.Collections.Generic;
using UnityEngine;

public class PatrolMotor : Motor {

	[SerializeField] private List<Vector3> _waypoints;
	[SerializeField] private int _repeatCount = -1;
	[SerializeField] private bool _reverses = false;

	private int _currentWaypoint = 0;
	private int _repetitions = 0;
	private bool _reversing = false;

	private bool HasReachedEnd => _currentWaypoint == ( _reversing ? 0 : _waypoints.Count - 1 );
	private bool IsFinished => _repetitions >= _repeatCount && _repeatCount >= 0;

	private void AdvanceToNextWaypoint() {
		bool isFinished = IsFinished;
		bool hasReachedEnd = HasReachedEnd;

		bool shouldReverse = _reverses && hasReachedEnd && !isFinished;
		bool shouldRepeat = (_reverses ? ( _reversing && hasReachedEnd ) : hasReachedEnd ) && !isFinished;

		if ( _reversing && !_reverses )
			_reversing = false;

		if ( shouldReverse )
			_reversing = !_reversing;

		if ( shouldRepeat )
			_repetitions++;

		int inc = _reversing ? -1 : 1;
		_currentWaypoint = Mathf.Clamp((_currentWaypoint + inc) % _waypoints.Count, 0, _waypoints.Count - 1);
		_mover.SetDestination(_waypoints[_currentWaypoint]);
	}

	protected override void Start() {
		base.Start();
		_mover.SetDestination(_waypoints[_currentWaypoint]);
	}

	private void Update() {
		if ( _mover.IsAtDestination && (!IsFinished || !HasReachedEnd) )
			AdvanceToNextWaypoint();
	}
}
