using System;
using UnityEngine;

public class BudBrain : EnemyBrainBase {
	public enum FireMode {
		FixedAxis,
		DirectAim,
		ArcFire
	}

	[Header("Components")]
	[SerializeField] private ProjectileWeapon _weapon;
	[SerializeField] private DetectionSystem _detection;
	[SerializeField] private RotateToTarget _rotator;

	[Header("Settings")]
	[SerializeField] private FireMode _mode = FireMode.DirectAim;
	[SerializeField] private Vector3 _fixedAxis = Vector3.forward;

	void Update() {
		if ( _health != null && _health.IsDead )
			return;

		switch ( _mode ) {
			case FireMode.FixedAxis:
				if ( _weapon.CanFire ) {
					_animatorDriver?.TriggerAttack2();
					_weapon.Fire(transform.TransformDirection(_fixedAxis));
				}
				break;
			case FireMode.DirectAim:
				FaceAndAttackTarget((Vector3 targetPos) => {
					_animatorDriver?.TriggerAttack2();
					_weapon.Fire(targetPos);
				});
				break;
			case FireMode.ArcFire:
				FaceAndAttackTarget((Vector3 targetPos) => {
					_animatorDriver?.TriggerAttack1();
					_weapon.FireArc(targetPos);
				});
				break;
		}
	}

	private void FaceAndAttackTarget(Action<Vector3> andThen) {
		if ( TargetProvider == null || !TargetProvider.HasTarget )
			return;

		Transform target = TargetProvider.GetTarget();
		Vector3 targetPos = TargetProvider.GetTargetPosition();

		if ( _detection.IsTargetInDetectionRange(target) && _detection.HasLineOfSight(target) ) {
			_rotator?.FacePosition(targetPos);
			if ( _weapon.CanFire ) {
				andThen(targetPos);
			}
		}
	}
}
