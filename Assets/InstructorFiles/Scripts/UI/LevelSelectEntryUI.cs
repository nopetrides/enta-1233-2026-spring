using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     One reusable item inside the level select scroll view.
/// </summary>
public class LevelSelectEntryUI : MonoBehaviour
{
	[SerializeField] private TMP_Text _titleText;
	[SerializeField] private Button _button;

	private int _levelIndex;

	private void Awake()
	{
		if (_button == null) _button = GetComponent<Button>();
	}

	public void Setup(string level, int levelIndex)
	{
		_levelIndex = levelIndex;
		
		if (_titleText != null) _titleText.text = level;
	}

	public void ButtonPressed()
	{
		LevelMgr.Instance.SetCurrentLevel(_levelIndex);
		SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
	}
}
