using UnityEngine;

public class Character : MonoBehaviour {

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Collider _collider;

    public CharacterController Controller => _characterController;
    public Collider Collider => _collider;
    public float gravity = 9.81f;

}
