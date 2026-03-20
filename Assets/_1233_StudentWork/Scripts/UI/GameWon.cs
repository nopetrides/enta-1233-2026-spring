using UnityEngine;

public class GameWon : MenuBase {
    public override GameMenus MenuType() {
        return GameMenus.GameWinMenu;
    }

    public void ButtonMainMenu() {
        LevelMgr.Instance.NextLevel();
        SceneMgr.Instance.LoadScene(GameScenes.MainMenu, GameMenus.MainMenu);
    }
}
