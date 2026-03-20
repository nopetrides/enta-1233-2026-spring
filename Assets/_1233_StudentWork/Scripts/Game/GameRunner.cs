using System.Collections;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        StartCoroutine(StartWhenReady());
    }

    private IEnumerator StartWhenReady() {
        Debug.Log("GameStarter: Requesting level load");
        LevelMgr.Instance.LoadCurrentLevel();

        Debug.Log("GameStarter: Waiting for level to finish loading...");
        yield return new WaitUntil(() => LevelMgr.Instance.IsLoaded);

        Debug.Log("GameStarter: Spawning player");
        yield return new WaitUntil(() => PlayerService.Instance.GetPlayers().Length > 0);
        Player player = PlayerService.Instance.GetPlayers()[0];
        player.SpawnCharacter();

        Debug.Log("Game starting");
    }
}
