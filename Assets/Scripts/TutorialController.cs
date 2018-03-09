using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class TutorialController : MonoBehaviour {
	/* Tutorial event driven system
	 * 0: Collect rock tutorial->
	 * 1: Place rock tutorial
	 * 2: Push rock tutorial->
	 * 3: Break rock tutorial (self rock, opponent rock)
	 * 4: Kill a fake opponent->
	 * 5: Carry gem tutorial
	 */

	/* Shared methods:
	 * 1. CreateDialogBox(Vector3 pos, string text): create a dialog box at pos and initialize with text
	 *    Returns the created game object.
	 * 2. EditDialogBox(GameObject box, string text): edit the text of the dialog box.
	 * 3. PlayerFunctionConstraint(bool allow_place_rock, bool allow_get_gems): prevent or allow user functions.
	 */
	public GameObject rock_collectable;
	public GameObject dialog_box;
	public GameObject down_pointer;
	public GameObject left_pointer;
	public GameObject right_pointer;
	private InputDevice input_device;

	GameObject[] players;

	int everyone_progress = 0;
	float[] player_progress;
	bool go_next_level = true;


	/* Class functions:
	 * Level progression in coroutines
	 * Update(): check and progress to next level
	 */
	void Start () {
		input_device = GetComponent<PlayerControl> ().GetInputDevice ();
		/* initialize player progress to 0 */
		player_progress = new float[4];
		for (int i = 0; i < 4; ++i) {
			player_progress [i] = 0f;
		}
		/* Find all players */
		players = new GameObject[4];
		for (int i = 0; i < 4; ++i) {
			players [i] = GameObject.Find ("Players/player" + (i + 1).ToString ());
		}
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
		case 3:
			StartCoroutine (Level3 ());
			break;
		case 4:
			StartCoroutine (Level4 ());
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

		PlayerFunctionConstraint (false, false);

		string text_1 = "\nCollect the rock on the ground.\n";
		string text_2 = "Good job!\nYou can see the number of rocks (1/3) you are carrying.\nPress Y to continue.";
		string text_3 = "Now wait for all the other players to finish this step.\n";

		Instantiate (rock_collectable, new Vector3 (-6f, 2f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (6f, 2f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (-6f, -3f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (6f, -3f, 0), Quaternion.identity);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (down_pointer, new Vector3 (-6f, 2.7f, 0), Quaternion.identity);
		pointers[1] = Instantiate (down_pointer, new Vector3 (6f, 2.7f, 0), Quaternion.identity);
		pointers[2] = Instantiate (down_pointer, new Vector3 (-6f, -2.3f, 0), Quaternion.identity);
		pointers[3] = Instantiate (down_pointer, new Vector3 (6f, -2.3f, 0), Quaternion.identity);

		GameObject[] diag_box = new GameObject[4];

		diag_box [0] = CreateDialogBox (new Vector3 (-2.3f, 3f, 0f), text_1);
		diag_box [1] = CreateDialogBox (new Vector3 (2.3f, 3f, 0f), text_1);
		diag_box [2] = CreateDialogBox (new Vector3 (-2.3f, -1f, 0f), text_1);
		diag_box [3] = CreateDialogBox (new Vector3 (2.3f, -1f, 0f), text_1);

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
			Destroy (diag_box [i]);
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

		GameObject[] diag_box = new GameObject[4];
		diag_box [0] = CreateDialogBox (new Vector3 (-2.3f, 3f, 0f), text_1);
		diag_box [1] = CreateDialogBox (new Vector3 (2.3f, 3f, 0f), text_1);
		diag_box [2] = CreateDialogBox (new Vector3 (-2.3f, -1f, 0f), text_1);
		diag_box [3] = CreateDialogBox (new Vector3 (2.3f, -1f, 0f), text_1);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (right_pointer, new Vector3 (-6f, 2f, 0), Quaternion.identity);
		pointers[1] = Instantiate (right_pointer, new Vector3 (6f, 2f, 0), Quaternion.identity);
		pointers[2] = Instantiate (right_pointer, new Vector3 (-6f, -2f, 0), Quaternion.identity);
		pointers[3] = Instantiate (right_pointer, new Vector3 (6f, -2f, 0), Quaternion.identity);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 1.0f &&
					Mathf.Abs (players [i].transform.position.x - pointers [i].transform.position.x) <= 0.5f &&
				    Mathf.Abs (players [i].transform.position.y - pointers [i].transform.position.y) <= 0.3f &&
				    players [i].transform.forward == Vector3.right) {
					Destroy (pointers [i]);
					PlayerFunctionConstraint (false, false, false);
					player_progress [i] = 1.5f;
					EditDialogBox (diag_box [i], text_2);
				} 
				if (player_progress [i] == 1.5f) {
					PlayerFunctionConstraint (true, false, false);
					if (players [i].GetComponent<RockBarDisplayer> ().IfRockCountZero ()) {
						EditDialogBox (diag_box [i], text_3);
						player_progress [i] = 2.0f;
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
			Destroy (diag_box [i]);
		}
		go_next_level = true;
	}

	IEnumerator Level3(){
		Debug.Log ("Level 3");
		PlayerFunctionConstraint (true, false, true);

		string text_1 = "Stand at the arrow, and face the rock\n";
		string text_2 = "Press B to break your own rock\n";
		string text_3 = "Now wait for all the other players to finish this step.\n";

		GameObject[] diag_box = new GameObject[4];
		diag_box [0] = CreateDialogBox (new Vector3 (-2.3f, 3f, 0f), text_1);
		diag_box [1] = CreateDialogBox (new Vector3 (2.3f, 3f, 0f), text_1);
		diag_box [2] = CreateDialogBox (new Vector3 (-2.3f, -1f, 0f), text_1);
		diag_box [3] = CreateDialogBox (new Vector3 (2.3f, -1f, 0f), text_1);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (down_pointer, new Vector3 (-2f, 2f, 0), Quaternion.identity);
		pointers[1] = Instantiate (down_pointer, new Vector3 (2f, 2f, 0), Quaternion.identity);
		pointers[2] = Instantiate (down_pointer, new Vector3 (-2f, -2f, 0), Quaternion.identity);
		pointers[3] = Instantiate (down_pointer, new Vector3 (2f, -2f, 0), Quaternion.identity);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 2.0f &&
					Mathf.Abs (players [i].transform.position.x - pointers [i].transform.position.x) < 0.5f &&
					Mathf.Abs (players [i].transform.position.y - pointers [i].transform.position.y) < 0.3f &&
					players [i].transform.forward == Vector3.right) {
					PlayerFunctionConstraint (true, false, false);
					Destroy (pointers [i]);
					player_progress [i] = 2.5f;
					EditDialogBox (diag_box [i], text_2);
				} 
				if (player_progress [i] == 2.5f) {
					PlayerFunctionConstraint (true, false, false);
					if (players [i].GetComponent<RockBarDisplayer> ().GetRockCount() == 1) {
						EditDialogBox (diag_box [i], text_3);
						player_progress [i] = 3.0f;
						PlayerFunctionConstraint (false, false, true);
					}
				}
			}
			if (CheckEveryPlayer (3)) {
				everyone_progress += 2;
				break;
			}
			yield return null;
		}
	}

	IEnumerator Level4(){
		Debug.Log ("Level 4");
		PlayerFunctionConstraint (false, false, true);

		string text_1 = "Stand at the arrow, and face the rock\n";
		string text_2 = "Press B to break opponent's rock\n";
		string text_3 = "Now wait for all the other players to finish this step.\n";

		GameObject[] diag_box = new GameObject[4];
		diag_box [0] = CreateDialogBox (new Vector3 (-2.3f, 3f, 0f), text_1);
		diag_box [1] = CreateDialogBox (new Vector3 (2.3f, 3f, 0f), text_1);
		diag_box [2] = CreateDialogBox (new Vector3 (-2.3f, -1f, 0f), text_1);
		diag_box [3] = CreateDialogBox (new Vector3 (2.3f, -1f, 0f), text_1);

		GameObject[] pointers = new GameObject[4];
		pointers[0] = Instantiate (down_pointer, new Vector3 (-6f, 2.7f, 0), Quaternion.identity);
		pointers[1] = Instantiate (down_pointer, new Vector3 (6f, 2.7f, 0), Quaternion.identity);
		pointers[2] = Instantiate (down_pointer, new Vector3 (-6f, -2.3f, 0), Quaternion.identity);
		pointers[3] = Instantiate (down_pointer, new Vector3 (6f, -2.3f, 0), Quaternion.identity);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 3.0f &&
					Mathf.Abs (players [i].transform.position.x - pointers [i].transform.position.x) < 0.5f &&
					Mathf.Abs (players [i].transform.position.y - pointers [i].transform.position.y) < 0.3f &&
					players [i].transform.forward == Vector3.right) {
					PlayerFunctionConstraint (true, false, false);
					Destroy (pointers [i]);
					player_progress [i] = 3.5f;
					EditDialogBox (diag_box [i], text_2);
				} 
				if (player_progress [i] == 3.5f) {
					PlayerFunctionConstraint (true, false, false);
					if (players [i].GetComponent<RockBarDisplayer> ().GetRockCount() == 2) {
						EditDialogBox (diag_box [i], text_3);
						player_progress [i] = 4.0f;
						PlayerFunctionConstraint (false, false, true);
					}
				}
			}
			if (CheckEveryPlayer (4)) {
				everyone_progress += 1;
				break;
			}
			yield return null;
		}
	}

	IEnumerator Level6(){
		Debug.Log ("Level 6");
		PlayerFunctionConstraint (false, true, true);

		string text_1 = "Move to opponents' gem base\n Press A to steal a gem\n";
		string text_2 = "Move to your own gem base\n Press A to put a gem\n";
		string text_3 = "Now wait for all the other players to finish this step.\n";

		GameObject[] diag_box = new GameObject[4];
		diag_box [0] = CreateDialogBox (new Vector3 (-2.3f, 3f, 0f), text_1);
		diag_box [1] = CreateDialogBox (new Vector3 (2.3f, 3f, 0f), text_1);
		diag_box [2] = CreateDialogBox (new Vector3 (-2.3f, -1f, 0f), text_1);
		diag_box [3] = CreateDialogBox (new Vector3 (2.3f, -1f, 0f), text_1);

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
						PlayerFunctionConstraint (false, false, true);
					}
				}
			}
			if (CheckEveryPlayer (7)) {
				everyone_progress += 1;
				break;
			}
			yield return null;
		}
	}
		
	// Public function, Shared by all levels.
	GameObject CreateDialogBox(Vector3 pos, string text){
		GameObject box = Instantiate (dialog_box, pos, Quaternion.identity);
		box.GetComponent<DialogBoxController> ().EditText (text);
		return box;
	}

	void EditDialogBox(GameObject box, string text){
		box.GetComponent<DialogBoxController> ().EditText (text);
	}

	void PlayerFunctionConstraint(bool allow_place_rock, bool allow_get_gems, bool allow_movement = true){
		for (int i = 0; i < 4; ++i) {
			players [i].GetComponent<RockInteraction> ().enabled = allow_place_rock;
			players [i].GetComponent<BaseInteraction> ().enabled = allow_get_gems;
			players [i].GetComponent<GridBaseMovement> ().enabled = allow_get_gems;
		}
	}


}
