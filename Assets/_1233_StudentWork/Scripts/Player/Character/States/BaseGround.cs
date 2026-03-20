using UnityEngine;

namespace Assets._1233_StudentWork.Scripts.PlayerCharacter.States {
	public class BaseGround : MX02StateBase {

		public override void OnEnter(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			base.OnEnter(deltaTime, character, input);
			character.velocity = new(character.velocity.x, -10, character.velocity.z);
		}

		public override void Step(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			base.Step(deltaTime, character, input);
		}

		public override void OnLeave(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			base.OnLeave(deltaTime, character, input);
			character.velocity = new(character.velocity.x, 0, character.velocity.z);
		}

	}
}
