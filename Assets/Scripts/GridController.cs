using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

	private Dictionary<int, Dictionary<int, int>> grid_occupancy = new Dictionary<int, Dictionary<int, int>>();

	public int horizontal_length;
	public int vertical_length;
	public static GridController instance;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this);
		} else {
			instance = this;
		}

		for (int i = -horizontal_length / 2; i <= horizontal_length; ++i) {
			grid_occupancy.Add (i, new Dictionary<int, int> ());
			for (int j = -vertical_length / 2; j <= vertical_length / 2; ++j) {
				grid_occupancy [i].Add (j, 0);
			}
		}
	}
	
	public void TeamOccupyGrid(Vector3 pos, int team_number) {
		int x = Mathf.RoundToInt (pos.x);
		int y = Mathf.RoundToInt (pos.y);

//		Debug.Log ("Occupy: " + x.ToString () + "," + y.ToString ());

		if (team_number == 1) {
			if (grid_occupancy [x] [y] > 0) {
				Debug.Log (pos);
			}

			--grid_occupancy [x] [y];
		} else {
			if (grid_occupancy [x] [y] < 0) {
				Debug.Log (pos);
			}

			++grid_occupancy [x] [y];
		}
	}

	public void TeamLeaveGrid(Vector3 pos, int team_number) {
		int x = Mathf.RoundToInt (pos.x);
		int y = Mathf.RoundToInt (pos.y);

//		Debug.Log ("Leave: " + x.ToString () + "," + y.ToString ());

		if (team_number == 1) {
			if (grid_occupancy [x] [y] >= 0) {
				Debug.Log (pos);
			}

			++grid_occupancy [x] [y];
		} else {
			if (grid_occupancy [x] [y] <= 0) {
				Debug.Log (pos);
			}

			--grid_occupancy [x] [y];
		}
	}

	public int GetGridTeam(Vector3 pos) {
		int x = Mathf.RoundToInt (pos.x);
		int y = Mathf.RoundToInt (pos.y);

		if (Mathf.Abs (grid_occupancy [x] [y]) > 2) {
			Debug.Log (pos);
		}

		if (grid_occupancy [x] [y] == 0) {
			return 0;
		} else if (grid_occupancy [x] [y] < 0) {
			return 1;
		} else {
			return 2;
		}
	}
}
