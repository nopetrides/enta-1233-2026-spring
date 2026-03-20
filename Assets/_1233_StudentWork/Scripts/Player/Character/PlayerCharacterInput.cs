using UnityEngine;

public struct PlayerCharacterInput {
	public readonly Vector3 MoveDirection;
	public readonly bool Jump;
	public readonly bool Sprint;
	public readonly bool Attack;

	public PlayerCharacterInput(Vector3 moveDir, bool jump, bool sprint, bool attack) {
		MoveDirection = moveDir;
		Jump = jump;
		Sprint = sprint;
		Attack = attack;
	}
}
