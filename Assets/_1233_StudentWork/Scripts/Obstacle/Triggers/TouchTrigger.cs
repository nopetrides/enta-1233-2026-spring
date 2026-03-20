using UnityEngine;

public class TouchTrigger : TriggerBase {

	private void OnCollisionEnter(Collision collision) {
		Trigger(true);
	}

	private void OnCollisionExit(Collision collision) {
		Trigger(false);
	}

	private void OnTriggerEnter(Collider other) {
		Trigger(true);
	}

	private void OnTriggerExit(Collider other) {
		Trigger(false);
	}

}
