using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour {
	/* Tutorial event-driven system
	 * 0: Collect rock tutorial->
	 * 1: Place rock tutorial
	 * 2: Push rock tutorial->
	 * 3: Break self rock
	 * 4: break opponent rock
	 * 5: Kill a fake opponent->
	 * 6: Carry gem tutorial
	 * 
	 * 
	 * 
	 * Shared methods:
	 * 1. CreateDialogBox(Vector3 pos, string text): create a dialog box at pos and initialize with text
	 *    Returns the created game object.
	 * 2. EditDialogBox(GameObject box, string text): edit the text of the dialog box.
	 * 3. PlayerFunctionConstraint(bool allow_place_rock, bool allow_get_gems): prevent or allow user functions.
	 */

	public GameObject rock_collectable;
	public GameObject dialog_box;
	public GameObject up_pointer;
	public GameObject down_pointer;
	public GameObject left_pointer;
	public GameObject right_pointer;
	public GameObject box_sensor;
	public GameObject fake_red_opponent;
	public GameObject fake_blue_opponent;
	public GameObject red_rock;
	public GameObject blue_rock;

	GameObject[] diag_box;
	GameObject[] players;

	int everyone_progress = 0;
	float[] player_progress;
	bool go_next_level = true;
	public int start_level = 0; // For debug only


	/* Class functions:
	 * Level progression in coroutines
	 * Update(): check and progress to next level
	 */
	void Start () {
		/* initialize player progress to 0 */
		player_progress = new float[4];
		for (int i = 0; i < 4; ++i) {
			player_progress [i] = start_level;
		}
		/* Find all players */
		players = new GameObject[4];
		for (int i = 0; i < 4; ++i) {
			players [i] = GameObject.Find ("Players/player" + (i + 1).ToString ());
		}
		everyone_progress = start_level;

		diag_box = new GameObject[4];

		diag_box [0] = CreateDialogBox (new Vector3 (-2.3f, 3f, 0f), "");
		diag_box [1] = CreateDialogBox (new Vector3 (2.3f, 3f, 0f), "");
		diag_box [2] = CreateDialogBox (new Vector3 (-2.3f, -1f, 0f), "");
		diag_box [3] = CreateDialogBox (new Vector3 (2.3f, -1f, 0f), "");
	}
	
	void Update () {
		if (go_next_level) {
			ActivateLevel (everyone_progress);
		}
	}

	void ActivateLevel(int level){
		go_next_level = false;
		switch (level) {
		case 0:
			StartCoroutine (Level0 ());
			break;
		case 1:
			StartCoroutine (Level1 ());
			break;
		case 2:
			StartCoroutine (Level2 ());
			break;
		case 3:
			StartCoroutine (Level3 ());
			break;
		case 4:
			StartCoroutine (Level4 ());
			break;
		case 5:
			StartCoroutine (Level5 ());
			break;
		case 6:
			StartCoroutine (Level6 ());
			break;
		// TODO: Add more cases
		}
	}

	bool CheckEveryPlayer(int level){
		for (int i = 0; i < 4; ++i)
			if (player_progress [i] < level+1)
				return false;
		return true;
	}
		
		
	IEnumerator Level0(){
		Debug.Log ("Level 0");

		PlayerFunctionConstraint (false, false, true, -1);

		string text_1 = "\nCollect the rock on the ground.\n";
		string text_2 = "Good job!\nYou can see the number of rocks (1/3) you are carrying.\nPress Y to continue.";
		string text_3 = "\nNow please wait for all the other players to finish this step.\n";

		Instantiate (rock_collectable, new Vector3 (-6f, 2f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (6f, 2f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (-6f, -3f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (6f, -3f, 0), Quaternion.identity);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (down_pointer, new Vector3 (-6f, 2.7f, 0), Quaternion.identity);
		pointers[1] = Instantiate (down_pointer, new Vector3 (6f, 2.7f, 0), Quaternion.identity);
		pointers[2] = Instantiate (down_pointer, new Vector3 (-6f, -2.3f, 0), Quaternion.identity);
		pointers[3] = Instantiate (down_pointer, new Vector3 (6f, -2.3f, 0), Quaternion.identity);

		InitAllDiagBox (text_1);

		GameObject[] bar_pointer = new GameObject[4];

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 0f && !players [i].GetComponent<RockBarDisplayer> ().IfRockCountZero ()) {
					Destroy(pointers[i]);
					EditDialogBox (diag_box [i], text_2);
					player_progress[i] += 0.1f;
					bar_pointer [i] = Instantiate (left_pointer, new Vector3 (-300f, -300f, 0f), Quaternion.identity);
				}
				if (player_progress [i] == 0.1f && players[i].GetComponent<PlayerControl>().GetInputDevice().Action4) {
					player_progress [i] = 1.0f;
					EditDialogBox (diag_box [i], text_3);
				}
				if (bar_pointer [i] != null) {
					Vector3 player_pos = players [i].transform.position;
					bar_pointer [i].transform.position = new Vector3 (player_pos.x+0.85f, player_pos.y+0.62f, player_pos.z);
				}

			}
			if (CheckEveryPlayer (0)) {
				everyone_progress += 1;
				break;
			}
			yield return null;
		}
		// restore
		for (int i = 0; i < 4; ++i) {
			Destroy (bar_pointer [i]);
		}
		go_next_level = true;
	}

	IEnumerator Level1(){
		Debug.Log ("Level 1");
		PlayerFunctionConstraint (false, false, true);

		//check every can place a rock
		for (int i = 0; i < 4; ++i) {
			Debug.Assert (!players [i].GetComponent<RockBarDisplayer> ().IfRockCountZero ());
		}

		string text_1 = "Stand at the arrow, and face right\n";
		string text_2 = "Press A to place the rock\n";
		string text_3 = "Now wait for all the other players to finish this step.\n";

		InitAllDiagBox (text_1);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (right_pointer, new Vector3 (-5f, 2f, 0), Quaternion.identity);
		pointers[1] = Instantiate (right_pointer, new Vector3 (5f, 2f, 0), Quaternion.identity);
		pointers[2] = Instantiate (right_pointer, new Vector3 (-5f, -2f, 0), Quaternion.identity);
		pointers[3] = Instantiate (right_pointer, new Vector3 (5f, -2f, 0), Quaternion.identity);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 1.0f &&
					Mathf.Abs (players [i].transform.position.x - pointers [i].transform.position.x) < 0.3f &&
					Mathf.Abs (players [i].transform.position.y - pointers [i].transform.position.y) < 0.3f){
					if (players [i].GetComponent<SpriteRenderer> ().sprite.name.StartsWith ("player_right")) {
						PlayerFunctionConstraint (true, false, true, i);
						player_progress [i] = 1.5f;
						EditDialogBox (diag_box [i], text_2);
					} 
//					else {
//						player_progress [i] = 1f;
//						PlayerFunctionConstraint (false, false, true, i);
//						EditDialogBox (diag_box [i], text_1);
//					}
				}  
				if (player_progress [i] == 1.5f) {
					if (players [i].GetComponent<RockBarDisplayer> ().IfRockCountZero ()) {
						PlayerFunctionConstraint (false, false, true, i);
						EditDialogBox (diag_box [i], text_3);
						player_progress [i] = 2.0f;
					} else if (!players [i].GetComponent<SpriteRenderer> ().sprite.name.StartsWith ("player_right") ||
						!(Mathf.Abs (players [i].transform.position.x - pointers [i].transform.position.x) < 0.3f &&
							Mathf.Abs (players [i].transform.position.y - pointers [i].transform.position.y) < 0.3f)) {
						player_progress [i] = 1f;
						PlayerFunctionConstraint (false, false, true, i);
						EditDialogBox (diag_box [i], text_1);
					}
				}
			}
			if (CheckEveryPlayer (1)) {
				everyone_progress += 1;
				break;
			}
			yield return null;
		}
		// restore
		for (int i = 0; i < 4; ++i) {
			Destroy (pointers [i]);
		}
		go_next_level = true;
	}

	IEnumerator Level2(){
		Debug.Log ("Level 2");

		PlayerFunctionConstraint (false, false, true, -1);

		string text_1 = "\nGo to the target place, face the rock, and press A.\n";
		string text_2 = "Good job!\nYou can push any rock placed by your team.\nPress Y to continue.";
		string text_3 = "\nNow please wait for all the other players to finish this step.\n";

		GameObject[] stand_sensor = new GameObject[4];
		stand_sensor [0] = Instantiate (box_sensor, new Vector3 (-5f, 2f, 0f), Quaternion.identity);
		stand_sensor [1] = Instantiate (box_sensor, new Vector3 (5f, 2f, 0f), Quaternion.identity);
		stand_sensor [2] = Instantiate (box_sensor, new Vector3 (-5f, -2f, 0f), Quaternion.identity);
		stand_sensor [3] = Instantiate (box_sensor, new Vector3 (5f, -2f, 0f), Quaternion.identity);
		for (int i = 0; i < 4; ++i) {
			stand_sensor [i].GetComponent<BoxSensorController> ().SetProperty (i, true);
		}

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (right_pointer, new Vector3 (-5f, 2f, 0), Quaternion.identity);
		pointers[1] = Instantiate (right_pointer, new Vector3 (5f, 2f, 0), Quaternion.identity);
		pointers[2] = Instantiate (right_pointer, new Vector3 (-5f, -2f, 0), Quaternion.identity);
		pointers[3] = Instantiate (right_pointer, new Vector3 (5f, -2f, 0), Quaternion.identity);

		GameObject[] rock_sensor = new GameObject[4];
		rock_sensor [0] = Instantiate (box_sensor, new Vector3 (-1f, 2f, 0f), Quaternion.identity);
		rock_sensor [1] = Instantiate (box_sensor, new Vector3 (8f, 2f, 0f), Quaternion.identity);
		rock_sensor [2] = Instantiate (box_sensor, new Vector3 (-1f, -2f, 0f), Quaternion.identity);
		rock_sensor [3] = Instantiate (box_sensor, new Vector3 (8f, -2f, 0f), Quaternion.identity);
		for (int i = 0; i < 4; ++i) {
			rock_sensor [i].GetComponent<BoxSensorController> ().SetProperty (i, false);
		}

		InitAllDiagBox (text_1);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 2.0f) {
					if (stand_sensor [i].GetComponent<BoxSensorController> ().IsStanding ()) {
						PlayerFunctionConstraint (true, false, true, i);
					} else {
						PlayerFunctionConstraint (false, false, true, i);
					}
					// TODO: add animation for telling the player it's the right place
				}
				if (player_progress [i] == 2.0f && rock_sensor [i].GetComponent<BoxSensorController> ().IsStanding ()) {
					PlayerFunctionConstraint (false, false, true, i);
					player_progress [i] = 2.5f;
					EditDialogBox (diag_box [i], text_2);
				}
				if (player_progress [i] == 2.5f && players[i].GetComponent<PlayerControl>().GetInputDevice().Action4) {
					player_progress [i] = 3.0f;
					EditDialogBox (diag_box [i], text_3);
				}
			}

			if (CheckEveryPlayer (2)) {
				everyone_progress = 3;
				break;
			}
			yield return null;
		}
		// restore
		for (int i = 0; i < 4; ++i){
			Destroy (stand_sensor [i]);
			Destroy (rock_sensor [i]);
			Destroy (pointers [i]);
		}
		go_next_level = true;
	}


	IEnumerator Level3(){
		Debug.Log ("Level 3");
		PlayerFunctionConstraint (true, false, true);

		string text_1 = "Stand at the arrow, and face the rock\n Press B to break self rock\n Recollect it";
		string text_2 = "Now wait for all the other players to finish this step.\n";

		InitAllDiagBox (text_1);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (down_pointer, new Vector3 (-2f, 2f, 0), Quaternion.identity);
		pointers[1] = Instantiate (down_pointer, new Vector3 (7f, 2f, 0), Quaternion.identity);
		pointers[2] = Instantiate (down_pointer, new Vector3 (-2f, -2f, 0), Quaternion.identity);
		pointers[3] = Instantiate (down_pointer, new Vector3 (7f, -2f, 0), Quaternion.identity);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 3 && 
					(players [i].GetComponent<RockBarDisplayer> ().GetRockCount() == 1 ||
						FindCollector(pointers[i].transform.position + Vector3.right))){
					player_progress [i] = 4.0f;
					PlayerFunctionConstraint (true, false, true, i);
					Destroy (pointers [i]);
					EditDialogBox (diag_box [i], text_2);
				} 
			}
			if (CheckEveryPlayer (3)) {
				everyone_progress += 1;
				break;
			}
			yield return null;
		}
		go_next_level = true;
	}


	IEnumerator Level4(){
		Debug.Log ("Level 4");
		PlayerFunctionConstraint (true, false, true, -1);

		string text_1 = "Stand at the arrow, and face the rock\n Hold B to break opponent's rock.\n";
		string text_2 = "Now wait for all the other players to finish this step.\n";

		Vector3[] rock_positions = new Vector3[4];
		rock_positions[0] = new Vector3(-8f, 3f, 0f);
		rock_positions[1] = new Vector3(8f, 3f, 0f);
		rock_positions[2] = new Vector3(-8f, -3f, 0f);
		rock_positions[3] = new Vector3(8f, -3f, 0f);

		GameObject[] opponent_rocks = new GameObject[4];
		opponent_rocks[0] = ChangeRock(rock_positions[0], red_rock);
		opponent_rocks[1] = ChangeRock(rock_positions[1], red_rock);
		opponent_rocks[2] = ChangeRock(rock_positions[2], blue_rock);
		opponent_rocks[3] = ChangeRock(rock_positions[3], blue_rock);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (left_pointer, new Vector3 (-7f, 3f, 0), Quaternion.identity);
		pointers[1] = Instantiate (right_pointer, new Vector3 (7f, 3f, 0), Quaternion.identity);
		pointers[2] = Instantiate (left_pointer, new Vector3 (-7f, -3f, 0), Quaternion.identity);
		pointers[3] = Instantiate (right_pointer, new Vector3 (7f, -3f, 0), Quaternion.identity);

		InitAllDiagBox (text_1);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 4.0f && 
					(players [i].GetComponent<RockBarDisplayer> ().GetRockCount() == 2 ||
						FindCollector(rock_positions[i]))){
					PlayerFunctionConstraint (true, false, true, i);
					player_progress[i] = 5.0f;
					EditDialogBox (diag_box [i], text_2);
				} 
			}
			if (CheckEveryPlayer (4)) {
				everyone_progress += 1;
				break;
			}
			yield return null;
		}
		// restore
		for (int i = 0; i < 4; ++i){
			Destroy (pointers [i]);
		}
		go_next_level = true;
	}

	IEnumerator Level5(){
		Debug.Log ("Level 5");

		PlayerFunctionConstraint (true, false, true, -1);

		string text_1 = "A moving rock can kill your opponent. Try it now!!\n";
		string text_2 = "Good job! Notice that your opponent will resurrect in a few seconds at the resurrect point.\nPress Y to continue.";
		string text_3 = "Be careful for the moving rocks!\nNow please wait for all the other players to finish this step.\n";

		InitAllDiagBox (text_1);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (down_pointer, new Vector3 (-1f, 2f, 0), Quaternion.identity);
		pointers[1] = Instantiate (down_pointer, new Vector3 (1f, 2f, 0), Quaternion.identity);
		pointers[2] = Instantiate (down_pointer, new Vector3 (-1f, -2f, 0), Quaternion.identity);
		pointers[3] = Instantiate (down_pointer, new Vector3 (1f, -2f, 0), Quaternion.identity);

		GameObject[] fake_players = new GameObject[4];
		fake_players[0] = Instantiate (fake_red_opponent, new Vector3 (-1f, 1f, 0), Quaternion.identity);
		fake_players[1] = Instantiate (fake_red_opponent, new Vector3 (1f, 1f, 0), Quaternion.identity);
		fake_players[2] = Instantiate (fake_blue_opponent, new Vector3 (-1f, -3f, 0), Quaternion.identity);
		fake_players[3] = Instantiate (fake_blue_opponent, new Vector3 (1f, -3f, 0), Quaternion.identity);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 5.0f && !fake_players [i].GetComponent<FakePlayerController> ().IsAlive ()) {
					if (i == 0 || i == 1) {
						fake_players [i].transform.position = new Vector3 (8f, 0f, 0f);
						pointers [i].transform.position = new Vector3 (9f, 1f, 1f);
					} else {
						fake_players [i].transform.position = new Vector3 (-8f, 0f, 0f);
						pointers [i].transform.position = new Vector3 (-9f, 1f, 1f);
					}
					player_progress [i] = 5.5f;
					EditDialogBox (diag_box [i], text_2);
				}
				if (player_progress [i] == 5.5f && players [i].GetComponent<PlayerControl> ().GetInputDevice ().Action4) {
					player_progress [i] = 6.0f;
					EditDialogBox (diag_box [i], text_3);
				}

			}
			if (CheckEveryPlayer (5)) {
				everyone_progress = 6;
				break;
			}
			yield return null;
		}

		// restore
		for (int i = 0; i < 4; ++i){
			Destroy (pointers [i]);
			Destroy (fake_players [i]);
		}
		go_next_level = true;
	}
		

	IEnumerator Level6(){
		Debug.Log ("Level 6");
		Destroy(GameObject.Find("Walls/TemporaryWalls"));
		PlayerFunctionConstraint (true, true, true);

		string text_1 = "Move to opponents' gem base\n Press A to steal a gem\n";
		string text_2 = "Move to your own gem base\n Press A to put a gem\n";
		string text_3 = "Now wait for all the other players to finish this step.\n";

		InitAllDiagBox (text_1);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (down_pointer, new Vector3 (-5f, 1f, 0), Quaternion.identity);
		pointers[1] = Instantiate (down_pointer, new Vector3 (5f, 1f, 0), Quaternion.identity);
		pointers[2] = Instantiate (up_pointer, new Vector3 (-5f, -1f, 0), Quaternion.identity);
		pointers[3] = Instantiate (up_pointer, new Vector3 (5f, -1f, 0), Quaternion.identity);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 6.0f &&
					players [i].GetComponent<GemInteraction> ().GetHolding()) {
					player_progress [i] = 6.5f;
					EditDialogBox (diag_box [i], text_2);
				} 
				if (player_progress [i] == 6.5f) {
					if (!players [i].GetComponent<GemInteraction> ().GetHolding()) {
						EditDialogBox (diag_box [i], text_3);
						player_progress [i] = 7.0f;
					}
				}
			}
			if (CheckEveryPlayer (7)) {
				SceneManager.LoadScene (0);
				break;
			}
			yield return null;
		}

		for (int i = 0; i < 4; ++i) {
			Destroy (pointers [i]);
		}
	}


	/* 
	 * Public functions
	 * Shared by all levels.
	 */

	private void PlayerFunctionConstraint(bool allow_place_rock, bool allow_get_gems, bool allow_movement = true, int player_id=-1){
		if (player_id == -1) {
			for (int i = 0; i < 4; ++i) {
				players [i].GetComponent<RockInteraction> ().enabled = allow_place_rock;
				players [i].GetComponent<BaseInteraction> ().enabled = allow_get_gems;
				players [i].GetComponent<GridBaseMovement> ().enabled = allow_movement;
			}
		} else {
			players [player_id].GetComponent<RockInteraction> ().enabled = allow_place_rock;
			players [player_id].GetComponent<BaseInteraction> ().enabled = allow_get_gems;
			players [player_id].GetComponent<GridBaseMovement> ().enabled = allow_movement;

		}
	}

	private GameObject CreateDialogBox(Vector3 pos, string text){
		GameObject box = Instantiate (dialog_box, pos, Quaternion.identity);
		box.GetComponent<DialogBoxController> ().EditText (text);
		return box;
	}

	private void EditDialogBox(GameObject box, string text){
		box.GetComponent<DialogBoxController> ().EditText (text);
	}

	private bool FindCollector(Vector3 pos){
		Collider collider = PublicFunctions.instance.FindObjectOnPosition (pos);
		if (collider.gameObject.CompareTag ("rock_collectable"))
			return true;
		return false;
	}

	private GameObject ChangeRock(Vector3 pos, GameObject opponent_rock){
		Collider collider = PublicFunctions.instance.FindObjectOnPosition (pos);
		Debug.Assert (collider.gameObject.CompareTag ("wall"));
		Destroy (collider.gameObject);
		return Instantiate (opponent_rock, pos, Quaternion.identity);
	}

	private void InitAllDiagBox(string text_1){
		for (int i = 0; i < 4; ++i) {
			EditDialogBox (diag_box [i], text_1);
		}
	}
}
