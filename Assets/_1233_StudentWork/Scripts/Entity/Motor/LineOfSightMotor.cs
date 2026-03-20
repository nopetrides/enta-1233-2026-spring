using UnityEngine;

[RequireComponent(typeof(ITargetProvider))]
public class LineOfSightMotor : FollowTargetMotor {

	protected override bool ShouldStop(Vector3 destination) {
		if ( base.ShouldStop(destination) )
			return true;

		Vector3 dir = destination - transform.position;
		bool didHit = Physics.Raycast(transform.position, dir, dir.magnitude);

		return !didHit;
	}

}