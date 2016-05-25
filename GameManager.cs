using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool showingMouse;
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
		showingMouse = false;
		Cursor.visible = false;
		if (instance != null) {
			Debug.LogError("More than one GameManager in scene");
		} else {
			instance = this;
	}
		
	}
	void Update() {
		if(Input.GetKeyDown(KeyCode.Escape) && showingMouse == false){
			showingMouse = true;
			Cursor.visible =true;
			//Cursor.lockState = false;
		}
		//if you press "escape" and the mouse is shown then make it not shown
		else if(Input.GetKeyDown(KeyCode.Escape) && showingMouse == true){
			showingMouse = false;
			Cursor.visible =false;
			//Cursor.lockState = true;
		}
	}
}
