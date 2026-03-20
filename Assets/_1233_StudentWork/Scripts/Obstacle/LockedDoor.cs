using UnityEngine;

public class LockedDoor : MonoBehaviour {
    [SerializeField] private TriggerBase _trigger;

    private void Open(bool _) {
        gameObject.SetActive(false);
    }

    void Start() {
        _trigger.OnTriggered += Open;
    }

    void Update() {
        
    }
}
