using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMgr : Singleton<LevelMgr>
{
	[SerializeField] private string[] _levelSceneNames;
	public string[] LevelSceneNames => _levelSceneNames;
	
	private int _currentLevelIndex;
	
	public bool IsLevelLoaded { get; private set; }
	
	public void SetCurrentLevel(int currentLevelIndex)
	{
		_currentLevelIndex = currentLevelIndex;
	}
	
	public void LoadCurrentLevel()
	{
		IsLevelLoaded = false;
		StartCoroutine(LoadLevelRoutine());
	}

	public void LoadNextLevel()
	{
		_currentLevelIndex = Mathf.Min(_currentLevelIndex + 1, _levelSceneNames.Length - 1);
		LoadCurrentLevel();
	}

	private IEnumerator LoadLevelRoutine()
	{
		var levelName = _levelSceneNames[_currentLevelIndex];

		Debug.Log($"LevelMgr: Loading {levelName} additively");

		var asyncOperation =
			SceneManager.LoadSceneAsync(
				levelName, LoadSceneMode.Additive);

		while (asyncOperation is {isDone: false}) yield return null;

		Debug.Log("LevelMgr: Level loaded");

		IsLevelLoaded = true;
	}
}
