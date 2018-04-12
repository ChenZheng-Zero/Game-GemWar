using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPlayerDataController {

	private static Dictionary<string, Dictionary<string, int>> player_data = new Dictionary<string, Dictionary<string, int>>();

	static StaticPlayerDataController() {
		for (int i = 1; i <= 4; ++i) {
			string tag = "player" + i.ToString ();
			player_data.Add (tag, new Dictionary<string, int> ());
			player_data [tag].Add ("kill", 0);
			player_data [tag].Add ("death", 0);
			player_data [tag].Add ("shoot", 0);
			player_data [tag].Add ("score", 0);
			player_data [tag].Add ("gem_pick_up", 0);
		}
	}

	public static void ClearData() {
		for (int i = 1; i <= 4; ++i) {
			string tag = "player" + i.ToString ();
			player_data [tag] ["kill"] = 0;
			player_data [tag] ["death"] = 0;
			player_data [tag] ["shoot"] = 0;
			player_data [tag] ["score"] = 0;
			player_data [tag] ["gem_pick_up"] = 0;
		}
	}

	public static void AddData(string tag, string field) {
		++player_data [tag] [field];
	}

	public static int GetData(string tag, string field) {
		return player_data [tag] [field];
	}
}
