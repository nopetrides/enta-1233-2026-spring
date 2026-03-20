using UnityEngine;

namespace Assets._1233_StudentWork.Scripts.PlayerCharacter.States {
	public class Idling : BaseGround {

		public override void OnEnter(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			base.OnEnter(deltaTime, character, input);
			character.velocity = new(0, character.velocity.y, 0);
		}

	}
}
