using UnityEngine;

namespace Assets._1233_StudentWork.Scripts.PlayerCharacter.States {
	class BaseAir : MX02StateBase {

		public override void OnEnter(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			base.OnEnter(deltaTime, character, input);
			character.velocity = new Vector3(character.velocity.x, 0, character.velocity.z);
		}

		public override void Step(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			base.Step(deltaTime, character, input);
			character.velocity -= new Vector3(0, character.gravity * deltaTime, 0);
		}

	}
}
