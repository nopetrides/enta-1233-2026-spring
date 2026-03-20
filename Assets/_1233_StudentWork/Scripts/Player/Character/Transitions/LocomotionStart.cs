using Assets._1233_StudentWork.Scripts.FSM;
using Assets._1233_StudentWork.Scripts.PlayerCharacter.States;
using System;

namespace Assets._1233_StudentWork.Scripts.PlayerCharacter.Transitions {
	class LocomotionStart : FSM_TransitionMeta<Character_MX02, PlayerCharacterInput> {
		new public static Type[] From => new Type[] { typeof(Idling), typeof(Landed) };
		new public static Type To => typeof(Locomoting);

		public override bool Test(float deltaTime, in Character_MX02 character, in PlayerCharacterInput input) {
			return character.Controller.isGrounded && input.MoveDirection.magnitude > 0;
		}
	}
}
