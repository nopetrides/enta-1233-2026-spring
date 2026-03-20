using UnityEngine;

public class LevelComplete : MenuBase {
    public override GameMenus MenuType() {
        return GameMenus.LevelCompleteMenu;
    }

    public void ButtonRetry() {
        LevelMgr.Instance.NextLevel();
        SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
    }

    public void ButtonMainMenu() {
        SceneMgr.Instance.LoadScene(GameScenes.MainMenu, GameMenus.MainMenu);
    }
}
