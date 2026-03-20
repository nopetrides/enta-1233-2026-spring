using System.Collections.Generic;
using UnityEngine;

public class EntityDeathTrigger : TriggerBase {

	[SerializeField] private List<Health> _healths;
	private int _numTriggers = 0;

	private void OnDied() {
		_numTriggers++;
		if ( _numTriggers == _healths.Count )
			Trigger(true);
	}

	private void Awake() {
		for ( int i = 0; i < _healths.Count; i++ ) {
			_healths[i].OnDied += OnDied;
		}
	}

}
