using Assets._1233_StudentWork.Scripts.Enum;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour {

    [SerializeField] private Character _defaultCharacterPrefab;
    [SerializeField] public SpawnLocation? SpawnLocation;

    private PlayerCharacterInput _inputs = new(Vector3.zero, false, false, false);

    public Character? Character { get; private set; } = null;
    public CharacterRelativeMovementMode RelativeMovementMode = CharacterRelativeMovementMode.Camera;
    public PlayerCharacterInput CharacterInputs => new(GetRelativeMoveDirection(_inputs.MoveDirection), _inputs.Jump, _inputs.Sprint, _inputs.Attack);

    private Vector3 GetSpawnPosition() {
        return SpawnLocation != null ? SpawnLocation.transform.position : Vector3.zero;
    }

    public Character SpawnCharacter(Vector3 position, Quaternion rotation, Character characterPrefab) {
        DespawnCharacter();
        Character character = Instantiate(characterPrefab, position, rotation);
        Character = character;
        return character;
    }

    public Character SpawnCharacter(Vector3 position, Quaternion? rotation) {
        return SpawnCharacter(position, rotation ?? Quaternion.identity, _defaultCharacterPrefab);
    }

    public Character SpawnCharacter(Character characterPrefab) {
        return SpawnCharacter(GetSpawnPosition(), Quaternion.identity, characterPrefab);
    }

    public Character SpawnCharacter() {
        return SpawnCharacter(GetSpawnPosition(), Quaternion.identity, _defaultCharacterPrefab);
    }

    public void DespawnCharacter() {
        if ( Character != null ) {
            Destroy(Character);
            Character = null;
        }
    }

    void OnMove(InputValue value) {
        Vector2 rawMoveVector = value.Get<Vector2>();
        _inputs = new(new Vector3(rawMoveVector.x, 0, rawMoveVector.y), _inputs.Jump, _inputs.Sprint, _inputs.Attack);
    }

    void OnJump(InputValue value) {
        bool jump = value.isPressed;
        _inputs = new(_inputs.MoveDirection, jump, _inputs.Sprint, _inputs.Attack);
    }

    void OnAttack(InputValue value) {
        bool attack = value.isPressed;
        _inputs = new(_inputs.MoveDirection, _inputs.Jump, _inputs.Sprint, attack);
    }

    void OnMovementMode() {
        RelativeMovementMode = RelativeMovementMode == CharacterRelativeMovementMode.Camera ? CharacterRelativeMovementMode.Character : CharacterRelativeMovementMode.Camera;
    }

    private Vector3 GetRelativeMoveDirection(Vector3 rawMoveVector) {
        Vector3 rightVector = Vector3.right;

        if ( RelativeMovementMode == CharacterRelativeMovementMode.Character ) {
            rightVector = Character.transform.right;
        } else if ( RelativeMovementMode == CharacterRelativeMovementMode.Camera ) {
            Camera camera = CameraMgr.Instance._mainCamera;
            rightVector = camera.transform.right;
        }

        Quaternion quat = Quaternion.LookRotation(Vector3.Cross(rightVector, Vector3.up), Vector3.up);
        return quat * rawMoveVector;
    }

    void Awake() {}

    void Start() { }

    void Update() {
        Cursor.lockState = Character != null ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
