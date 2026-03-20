
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Manager to apply level based data to the game state before the game loop begins
/// Might contain a list of difficulties, levels, etc.
/// </summary>
public class LevelMgr : Singleton<LevelMgr> {
	[SerializeField] private string[] _levelSceneNames;

	private int _currentLevelIndex = 0;
	public bool IsLoaded { get; private set; }

	private IEnumerator LoadLevelRoutine() {
		string levelName = _levelSceneNames[_currentLevelIndex];
		Debug.Log($"LevelMgr: Loading {levelName} additively");

		var asyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
		yield return new WaitUntil(() => asyncOperation.isDone);

		Debug.Log("LevelMgr: Level loaded");
		IsLoaded = true;
	}

	public void LoadCurrentLevel() {
		IsLoaded = false;
		StartCoroutine(LoadLevelRoutine());
	}

	public void NextLevel() {
		_currentLevelIndex = (_currentLevelIndex + 1) % _levelSceneNames.Length;
	}

}