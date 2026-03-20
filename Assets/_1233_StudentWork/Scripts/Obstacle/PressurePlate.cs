using UnityEngine;

public class PressurePlate : MonoBehaviour {

	[SerializeField] private TriggerBase _trigger;

	private void PressDown(bool pressed) {
		gameObject.transform.position += new Vector3(0, 0.05f * (pressed ? -1 : 1), 0);
	}

	private void Awake() {
		if (_trigger == null)
			_trigger = GetComponent<TriggerBase>();
	}

	private void Start() {
		_trigger.OnTriggered += PressDown;
	}

}
