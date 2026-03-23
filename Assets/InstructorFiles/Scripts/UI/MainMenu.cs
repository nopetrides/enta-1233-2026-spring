using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     The main menu when starting the game
///     The simple entry point after the game loads and return point if exiting gameplay
/// </summary>
public class MainMenu : MenuBase
{
	public override GameMenus MenuType()
	{
		return GameMenus.MainMenu;
	}

	public void ButtonStart()
	{
		if (LevelMgr.Instance != null) LevelMgr.Instance.SetCurrentLevel(0);
		SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
	}

	public void ButtonLevelSelect()
	{
		UIMgr.Instance.ShowMenu(GameMenus.LevelSelectMenu);
	}

	public void ButtonSettings()
	{
		UIMgr.Instance.ShowMenu(GameMenus.SettingsMenu);
	}

	public void ButtonQuit()
	{
		Application.Quit();
	}
}
