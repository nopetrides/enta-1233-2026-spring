using UnityEngine;

namespace Assets._1233_StudentWork.Scripts.PlayerCharacter.States {
	class Jumping : BaseAir {

		public override void OnEnter(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			base.OnEnter(deltaTime, character, input);
			character.velocity = new Vector3(character.velocity.x, character.jumpPower, character.velocity.z);
		}

	}
}
