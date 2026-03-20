using UnityEngine;

namespace Assets._1233_StudentWork.Scripts.PlayerCharacter.States {
	class Locomoting : BaseGround {

		public override void Step(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			base.Step(deltaTime, character, input);

			Vector3 velocity = input.MoveDirection * character.walkSpeed;
			character.velocity = new(velocity.x, character.velocity.y, velocity.z);
			character.Collider.transform.rotation = Quaternion.Slerp(character.Collider.transform.rotation, Quaternion.LookRotation(input.MoveDirection), deltaTime * 24);
		}

	}
}
