using UnityEngine;

namespace Assets._1233_StudentWork.Scripts {
	public class DebugWin : MonoBehaviour {
		private void OnTriggerEnter(Collider other) {
			if ( other.CompareTag("Player") ) {
				GameMgr.Instance.WinGame();
			}
		}
	}
}
