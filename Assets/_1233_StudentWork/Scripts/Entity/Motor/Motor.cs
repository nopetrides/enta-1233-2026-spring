using UnityEngine;

[RequireComponent(typeof(IMover))]
public abstract class Motor : MonoBehaviour {

	protected IMover _mover;

	protected virtual void Start() {
		_mover = GetComponent<IMover>();
	}

}