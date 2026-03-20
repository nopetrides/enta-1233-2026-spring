using UnityEngine;

public class ObjectActivator : MonoBehaviour {
    [SerializeField] private GameObject _object;
    [SerializeField] private TriggerBase _trigger;

    private void Activate(bool _) {
        _object.SetActive(true);
    }

    void Start() {
        _trigger.OnTriggered += Activate;
    }

    void Update() {

    }
}
