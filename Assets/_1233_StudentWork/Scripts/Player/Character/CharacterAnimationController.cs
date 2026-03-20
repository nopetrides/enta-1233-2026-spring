using UnityEngine;

namespace Assets._1233_StudentWork.Scripts.Player.Character {
	public class CharacterAnimationController : MonoBehaviour {

		[SerializeField] private Character_MX02 _character;
		[SerializeField] private Collider _hurtbox;

		public void OnProjectileFired() { }

		public void EnableAttacking() {
			_character.SetCanAttack(true);
		}

		public void DisableAttacking() {
			_character.SetCanAttack(false);
		}

		public void EnableHurtbox() {
			_hurtbox.gameObject.SetActive(true);
		}

		public void DisableHurtbox() {
			_hurtbox.gameObject.SetActive(false);
		}

		public void Died() {
			GameMgr.Instance.GameOver();
		}

	}
}
