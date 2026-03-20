using System.Collections.Generic;
using UnityEngine;

public class PlayerService : Singleton<PlayerService> {

	[SerializeField] private Player _playerPrefab;
	private List<Player> players = new();

	public Player AddPlayer() {
		Player player = Instantiate(_playerPrefab);
		DontDestroyOnLoad(player);
		players.Add(player);
		return player;
	}

	public void RemovePlayer(Player player) {
		players.Remove(player);
	}

	public Player[] GetPlayers() {
		return players.ToArray();
	}

	public Player? GetPlayerFromCharacter(Character character) {
		foreach ( Player player in players ) {
			if ( player.Character.gameObject == character.gameObject )
				return player;
		}
		return null;
	}

	public override void Awake() {
		base.Awake();
		AddPlayer();
	}

}
