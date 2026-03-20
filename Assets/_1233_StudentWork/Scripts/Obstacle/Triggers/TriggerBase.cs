using System;
using UnityEngine;

public class TriggerBase : MonoBehaviour {

	[SerializeField] private bool _reversible;
	private bool isTriggered = false;

	public event Action<bool> OnTriggered;

	protected void Trigger(bool active) {
		if ( active == isTriggered )
			return;

		if ( !active && !_reversible )
			return;

		isTriggered = active;
		OnTriggered.Invoke(active);
	}

}