﻿using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public MatchSettings matchSettings;
	private const string PLAYER_ID_PREFIX = "Player ";
	private static Dictionary<string, Player> players = new Dictionary<string, Player>();

public static void RegisterPlayer (string netID, Player player) {
		string playerID = PLAYER_ID_PREFIX + netID;
		players.Add (playerID, player);
		player.transform.name = playerID;
	}

	public static Player GetPlayer (string playerID)
	{
		return players[playerID];
	}

	public static void DeregisterPlayer(string playerID) {
		players.Remove (playerID);
	}

	void Awake() {
		if (instance != null) {
			Debug.LogError("More than one GameManager in scene");
		} else {
			instance = this;
	}
		
	}

}
