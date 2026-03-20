using UnityEngine;

public static class RuntimeBootstrapLoader {
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize() {
		if ( GlobalsMgr.Instance )
			return;

		var prefab = Resources.Load<GameObject>("GameGlobals");
		Object.Instantiate(prefab);
	}
}
