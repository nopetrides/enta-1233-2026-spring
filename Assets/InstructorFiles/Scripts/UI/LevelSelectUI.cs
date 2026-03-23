using UnityEngine;

/// <summary>
///     Data-driven level select menu that builds its entry list from <see cref="LevelMgr" />.
/// </summary>
public class LevelSelectUI : MenuBase
{
	[SerializeField] private Transform _contentParent;
	[SerializeField] private LevelSelectEntryUI _entryPrefab;

	public override GameMenus MenuType()
	{
		return GameMenus.LevelSelectMenu;
	}
	
	private void Start()
	{
		BuildEntries();
	}
	
	public void ButtonBack()
	{
		UIMgr.Instance.HideMenu(GameMenus.LevelSelectMenu);
	}

	private void BuildEntries()
	{
		var levels = LevelMgr.Instance.LevelSceneNames;
		if (levels.Length == 0) return;
		
		for (var i = 0; i < levels.Length; i++)
		{
			var entry = Instantiate(_entryPrefab, _contentParent);
			entry.Setup(levels[i], i);
		}
	}
}
