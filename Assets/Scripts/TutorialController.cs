using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
	public GameObject letter;
	public GameObject up_pointer;
	public GameObject down_pointer;
	public GameObject left_pointer;
	public GameObject right_pointer;
	public GameObject box_sensor;
	public GameObject fake_red_opponent;
	public GameObject fake_blue_opponent;
	public GameObject red_rock;
	public GameObject blue_rock;
	public GameObject tick;
	public VideoClip[] video_clips;

	GameObject[] diag_box;
	GameObject[] players;
	GameObject[] ticks;

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
		ticks = new GameObject[4];
		for (int i = 0; i < 4; ++i) {
			players [i] = GameObject.Find ("Players/player" + (i + 1).ToString ());
			ticks [i] = Instantiate (tick, Vector3.zero, Quaternion.identity);
		}
		everyone_progress = start_level;

		diag_box = new GameObject[4];

		diag_box [0] = CreateDialogBox (new Vector3 (-2.3f, 2.9f, 0f), "");
		diag_box [1] = CreateDialogBox (new Vector3 (2.3f, 2.9f, 0f), "");
		diag_box [2] = CreateDialogBox (new Vector3 (-2.3f, -1.1f, 0f), "");
		diag_box [3] = CreateDialogBox (new Vector3 (2.3f, -1.1f, 0f), "");
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
		for (int i = 0; i < 4; ++i) {
			if (player_progress [i] == level + 1) {
				ticks [i].GetComponent<SpriteRenderer> ().enabled = true;
				ticks [i].transform.position = players [i].transform.position + new Vector3 (0.56f, 0.31f, 0f);
			} else {
				ticks [i].GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
		for (int i = 0; i < 4; ++i)
			if (player_progress [i] < level+1)
				return false;
		return true;
	}
		
		
	IEnumerator Level0(){
		Debug.Log ("Level 0");

		string text_1 = "\nCollect the rock on the ground.\n";
		string text_2 = "Welcome to the Gem World!\n\nIn this tutorial, we will guide you to get familiar with this game.";
		string text_3 = "Tutorial 1: Collect Rock\n\nYou can stand on a small rock to collect it.";

		InitAllDiagBox ("");

		GameObject[] pointers = new GameObject[4];
		GameObject letter_box = CreateLetter (new string[]{ text_2, text_3 }, new VideoClip[]{null, video_clips[0]});

		bool initialized = false;
		while (true) {
			if (letter_box) {
				yield return null;
				continue;
			}
			if (!initialized) {
				initialized = true;
				PlayerFunctionConstraint (false, false, true, -1);
				Instantiate (rock_collectable, new Vector3 (-6f, 2f, 0), Quaternion.identity);
				Instantiate (rock_collectable, new Vector3 (6f, 2f, 0), Quaternion.identity);
				Instantiate (rock_collectable, new Vector3 (-6f, -3f, 0), Quaternion.identity);
				Instantiate (rock_collectable, new Vector3 (6f, -3f, 0), Quaternion.identity);
				pointers[0] = Instantiate (down_pointer, new Vector3 (-6f, 2.7f, 0), Quaternion.identity);
				pointers[1] = Instantiate (down_pointer, new Vector3 (6f, 2.7f, 0), Quaternion.identity);
				pointers[2] = Instantiate (down_pointer, new Vector3 (-6f, -2.3f, 0), Quaternion.identity);
				pointers[3] = Instantiate (down_pointer, new Vector3 (6f, -2.3f, 0), Quaternion.identity);
				InitAllDiagBox (text_1);
			}
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 0.0f && !players [i].GetComponent<RockBarDisplayer> ().IfRockCountZero ()) {
					Destroy(pointers[i]);
					EditDialogBox (diag_box [i], "");
					player_progress[i] = 1.0f;
				}
			}
			if (CheckEveryPlayer (0)) {
				everyone_progress += 1;
				break;
			}
			yield return null;
		}
		go_next_level = true;
	}

	IEnumerator Level1(){
		Debug.Log ("Level 1");

		for (int i = 0; i < 4; ++i) {
			Debug.Assert (!players [i].GetComponent<RockBarDisplayer> ().IfRockCountZero ());
		}

		string text_1 = "Stand on the arrow; face right.\n";
		string text_2 = "Press A to place the rock.\n";

		string text_3 = "Good Job!\n\nNotice that you can read the number of rocks you have collected from the bar on the top of you.";
		string text_4 = "Tutorial 2: Place Rock\n\nYou can place a rock on the ground at which you are facing by pressing A.";

		GameObject[] pointers = new GameObject[4];

		GameObject letter_box = CreateLetter (new string[]{ text_3, text_4 }, new VideoClip[]{null, video_clips[1]});

		bool initialized = false;
		while (true) {
			if (letter_box) {
				InitAllDiagBox ("");
				yield return null;
				continue;
			}
			if (!initialized) {
				initialized = true;
				PlayerFunctionConstraint (false, false, true);
				InitAllDiagBox (text_1);
				pointers[0] = Instantiate (right_pointer, new Vector3 (-5f, 2f, 0), Quaternion.identity);
				pointers[1] = Instantiate (right_pointer, new Vector3 (5f, 2f, 0), Quaternion.identity);
				pointers[2] = Instantiate (right_pointer, new Vector3 (-5f, -2f, 0), Quaternion.identity);
				pointers[3] = Instantiate (right_pointer, new Vector3 (5f, -2f, 0), Quaternion.identity);
			}
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 1.0f &&
					Mathf.Abs (players [i].transform.position.x - pointers [i].transform.position.x) < 0.3f &&
					Mathf.Abs (players [i].transform.position.y - pointers [i].transform.position.y) < 0.3f){
					if (players [i].GetComponent<SpriteRenderer> ().sprite.name.StartsWith ("player_right")) {
						PlayerFunctionConstraint (true, false, true, i);
						player_progress [i] = 1.5f;
						EditDialogBox (diag_box [i], text_2);
					} 
				}  
				if (player_progress [i] == 1.5f) {
					if (players [i].GetComponent<RockBarDisplayer> ().IfRockCountZero ()) {
						PlayerFunctionConstraint (false, false, true, i);
						player_progress [i] = 2.0f;
						EditDialogBox (diag_box [i], "");
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

		string text_1 = "Stand on the arrow, face the rock, and press A.\n";
		string text_3 = "Tutorial 3: Push Rock\n\nYou can shoot a rock by pushing it on the side. Press A when you face to the rock.";

		GameObject[] stand_sensor = new GameObject[4];
		stand_sensor [0] = Instantiate (box_sensor, new Vector3 (-5f, 2f, 0f), Quaternion.identity);
		stand_sensor [1] = Instantiate (box_sensor, new Vector3 (5f, 2f, 0f), Quaternion.identity);
		stand_sensor [2] = Instantiate (box_sensor, new Vector3 (-5f, -2f, 0f), Quaternion.identity);
		stand_sensor [3] = Instantiate (box_sensor, new Vector3 (5f, -2f, 0f), Quaternion.identity);
		for (int i = 0; i < 4; ++i) {
			stand_sensor [i].GetComponent<BoxSensorController> ().SetProperty (i, true);
		}

		GameObject[] pointers = new GameObject[4];

		GameObject[] rock_sensor = new GameObject[4];
		rock_sensor [0] = Instantiate (box_sensor, new Vector3 (-1f, 2f, 0f), Quaternion.identity);
		rock_sensor [1] = Instantiate (box_sensor, new Vector3 (8f, 2f, 0f), Quaternion.identity);
		rock_sensor [2] = Instantiate (box_sensor, new Vector3 (-1f, -2f, 0f), Quaternion.identity);
		rock_sensor [3] = Instantiate (box_sensor, new Vector3 (8f, -2f, 0f), Quaternion.identity);
		for (int i = 0; i < 4; ++i) {
			rock_sensor [i].GetComponent<BoxSensorController> ().SetProperty (i, false);
		}

		GameObject letter_box = CreateLetter (new string[]{text_3},  new VideoClip[]{video_clips[2]});
		bool initialized = false;
		while (true) {
			if (letter_box) {
				InitAllDiagBox ("");
				yield return null;
				continue;
			}
			if (!initialized) {
				initialized = true;
				PlayerFunctionConstraint (false, false, true, -1);
				InitAllDiagBox (text_1);
				pointers[0] = Instantiate (right_pointer, new Vector3 (-5f, 2f, 0), Quaternion.identity);
				pointers[1] = Instantiate (right_pointer, new Vector3 (5f, 2f, 0), Quaternion.identity);
				pointers[2] = Instantiate (right_pointer, new Vector3 (-5f, -2f, 0), Quaternion.identity);
				pointers[3] = Instantiate (right_pointer, new Vector3 (5f, -2f, 0), Quaternion.identity);
			}
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 2.0f) {
					if (stand_sensor [i].GetComponent<BoxSensorController> ().IsStanding ()) {
						PlayerFunctionConstraint (true, false, true, i);
					} else {
						PlayerFunctionConstraint (false, false, true, i);
					}
				}
				if (player_progress [i] == 2.0f && rock_sensor [i].GetComponent<BoxSensorController> ().IsStanding ()) {
					PlayerFunctionConstraint (false, false, true, i);
					player_progress [i] = 3.0f;
					EditDialogBox (diag_box [i], "");
					Destroy (rock_sensor [i]);
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
//			Destroy (rock_sensor [i]);
			Destroy (pointers [i]);
		}
		go_next_level = true;
	}


	IEnumerator Level3(){
		Debug.Log ("Level 3");

		string text_1 = "Stand on the arrow;\n Press B to break it!";
		string text_2 = "Tutorial 4: Break Rock(1)\n\n To break your own rock, face to it and press B.";

		GameObject[] pointers = new GameObject[4];

		GameObject letter_box = CreateLetter (new string[]{text_2}, new VideoClip[]{video_clips[3]});
		bool initialized = false;
		while (true) {
			if (letter_box) {
				InitAllDiagBox ("");
				yield return null;
				continue;
			}
			if (!initialized) {
				initialized = true;
				pointers[0] = Instantiate (right_pointer, new Vector3 (-2f, 2f, 0), Quaternion.identity);
				pointers[1] = Instantiate (right_pointer, new Vector3 (7f, 2f, 0), Quaternion.identity);
				pointers[2] = Instantiate (right_pointer, new Vector3 (-2f, -2f, 0), Quaternion.identity);
				pointers[3] = Instantiate (right_pointer, new Vector3 (7f, -2f, 0), Quaternion.identity);
				InitAllDiagBox (text_1);
				PlayerFunctionConstraint (true, false, true);
			}
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 3 && 
					(players [i].GetComponent<RockBarDisplayer> ().GetRockCount() == 1 ||
						FindCollector(pointers[i].transform.position + Vector3.right))){
					player_progress [i] = 4.0f;
					EditDialogBox (diag_box [i], "");
					PlayerFunctionConstraint (true, false, true, i);
					Destroy (pointers [i]);
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

		string text_1 = "Stand on the arrow;\n Hold B to break it!";
		string text_2 = "Tutorial 4: Break Rock(2)\n\n To break your opponents' rock, face to it and hold B for a while.";

		Vector3[] rock_positions = new Vector3[4];
		rock_positions[0] = new Vector3(-8f, 3f, 0f);
		rock_positions[1] = new Vector3(8f, 3f, 0f);
		rock_positions[2] = new Vector3(-8f, -3f, 0f);
		rock_positions[3] = new Vector3(8f, -3f, 0f);

		GameObject[] opponent_rocks = new GameObject[4];

		GameObject[] pointers = new GameObject[4];

		GameObject letter_box = CreateLetter (new string[]{text_2}, new VideoClip[]{video_clips[4]});
		bool initialized = false;
		while (true) {
			if (letter_box) {
				InitAllDiagBox ("");
				yield return null;
				continue;
			}
			if (!initialized) {
				initialized = true;
				InitAllDiagBox (text_1);
				PlayerFunctionConstraint (true, false, true, -1);
				pointers[0] = Instantiate (left_pointer, new Vector3 (-7f, 3f, 0), Quaternion.identity);
				pointers[1] = Instantiate (right_pointer, new Vector3 (7f, 3f, 0), Quaternion.identity);
				pointers[2] = Instantiate (left_pointer, new Vector3 (-7f, -3f, 0), Quaternion.identity);
				pointers[3] = Instantiate (right_pointer, new Vector3 (7f, -3f, 0), Quaternion.identity);
				opponent_rocks[0] = ChangeRock(rock_positions[0], red_rock);
				opponent_rocks[1] = ChangeRock(rock_positions[1], red_rock);
				opponent_rocks[2] = ChangeRock(rock_positions[2], blue_rock);
				opponent_rocks[3] = ChangeRock(rock_positions[3], blue_rock);
			}
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 4.0f && 
					(players [i].GetComponent<RockBarDisplayer> ().GetRockCount() == 2 ||
						FindCollector(rock_positions[i]))){
					PlayerFunctionConstraint (true, false, true, i);
					player_progress[i] = 5.0f;
					EditDialogBox (diag_box [i], "");
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

		string text_1 = "Shoot a rock to attack your opponent!\n";
		string text_2 = "Tutorial 5: Attack Your Opponent\n\nYou can attack your opponent by shooting a rock onto him/her.";
		
		GameObject[] pointers = new GameObject[4];
		GameObject[] fake_players = new GameObject[4];

		GameObject letter_box = CreateLetter (new string[]{text_2}, new VideoClip[]{video_clips[5]});
		bool initialized = false;
		while (true) {
			if (letter_box) {
				InitAllDiagBox ("");
				yield return null;
				continue;
			}
			if (!initialized) {
				initialized = true;
				PlayerFunctionConstraint (true, false, true, -1);
				pointers[0] = Instantiate (down_pointer, new Vector3 (-1f, 2f, 0), Quaternion.identity);
				pointers[1] = Instantiate (down_pointer, new Vector3 (1f, 2f, 0), Quaternion.identity);
				pointers[2] = Instantiate (down_pointer, new Vector3 (-1f, -2f, 0), Quaternion.identity);
				pointers[3] = Instantiate (down_pointer, new Vector3 (1f, -2f, 0), Quaternion.identity);
				fake_players[0] = Instantiate (fake_red_opponent, new Vector3 (-1f, 1f, 0), Quaternion.identity);
				fake_players[1] = Instantiate (fake_red_opponent, new Vector3 (1f, 1f, 0), Quaternion.identity);
				fake_players[2] = Instantiate (fake_blue_opponent, new Vector3 (-1f, -3f, 0), Quaternion.identity);
				fake_players[3] = Instantiate (fake_blue_opponent, new Vector3 (1f, -3f, 0), Quaternion.identity);
				InitAllDiagBox (text_1);
			}
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 5.0f && fake_players [i] && !fake_players [i].GetComponent<FakePlayerController> ().IsAlive ()) {
					Destroy (fake_players [i]);
					if (i == 0 || i == 1) {
						fake_players [i].transform.position = new Vector3 (8f, 0f, 0f);
						pointers [i].transform.position = new Vector3 (9f, 1f, 1f);
					} else {
						fake_players [i].transform.position = new Vector3 (-8f, 0f, 0f);
						pointers [i].transform.position = new Vector3 (-9f, 1f, 1f);
					}
					player_progress [i] = 6.0f;
					EditDialogBox (diag_box [i], "");
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

		string text_1 = "Go to opponents' gem base\nPress A to steal a gem\n";
		string text_2 = "Go back to your own gem base\nPress A to drop a gem\n";
		string text_3 = "Good job!\n\nAnyone hit by a rock will resurrect at the team's resurrection place in 5 second.";
		string text_4 = "Tutorial 6: Carry Gem\n\nPress A to steal your opponents' gems and drop them in your base.";
		string text_5 = "Congratulations! You finish all tutorials.\n\nThe team carries more gems in 90s will win.";
		string text_6 = "Cheet sheet:\nLeft stick: motion control\nButton A: place and push rock, steal and drop gem\nButton B: break rocks.";

		diag_box [0].GetComponent<DialogBoxController> ().ChangePosition (new Vector3 (-5f, 3f, 0f));
		diag_box [1].GetComponent<DialogBoxController> ().ChangePosition (new Vector3 (5f, 3f, 0f));
		diag_box [2].GetComponent<DialogBoxController> ().ChangePosition (new Vector3 (-5f, -3f, 0f));
		diag_box [3].GetComponent<DialogBoxController> ().ChangePosition (new Vector3 (5f, -3f, 0f));

		GameObject[] pointers = new GameObject[4];

		GameObject letter_box = CreateLetter (new string[]{text_3, text_4}, new VideoClip[]{video_clips[7], video_clips[6]});
		GameObject finish_letter = null;
		bool check_finish_letter = false;
		bool initialized = false;
		while (true) {
			if (letter_box) {
				InitAllDiagBox ("");
				yield return null;
				continue;
			}
			if (!initialized) {
				initialized = true;
				Destroy(GameObject.Find("Walls/TemporaryWalls"));
				PlayerFunctionConstraint (true, true, true);
				pointers[0] = Instantiate (down_pointer, new Vector3 (6f, 1.5f, 0), Quaternion.identity);
				pointers[1] = Instantiate (down_pointer, new Vector3 (5f, 1.5f, 0), Quaternion.identity);
				pointers[2] = Instantiate (up_pointer, new Vector3 (-5f, -.5f, 0), Quaternion.identity);
				pointers[3] = Instantiate (up_pointer, new Vector3 (-6f, -.5f, 0), Quaternion.identity);
				InitAllDiagBox (text_1);
			}
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 6.0f &&
					players [i].GetComponent<GemInteraction> ().GetHolding()) {
					player_progress [i] = 6.5f;
					EditDialogBox (diag_box [i], text_2);
					Vector3 pos = pointers [i].gameObject.transform.position;
					Vector3 new_pos = new Vector3 (-pos.x, pos.y, pos.z);
					Destroy (pointers [i]);
					if (i < 2) {
						pointers [i] = Instantiate (down_pointer, new_pos, Quaternion.identity);
					} else {
						pointers [i] = Instantiate (up_pointer, new_pos, Quaternion.identity);
					}
				} 
				if (player_progress [i] == 6.5f) {
					if (!players [i].GetComponent<GemInteraction> ().GetHolding()) {
						player_progress [i] = 7.0f;
						EditDialogBox (diag_box [i], "");
						Destroy (pointers [i]);
					}
				}
			}
			if (CheckEveryPlayer (6)) {
				if (!check_finish_letter) {
					check_finish_letter = true;
					finish_letter = CreateLetter (new string[]{ text_5, text_6 }, new VideoClip[]{null, null});
				}
				if (check_finish_letter && !finish_letter) {
					SceneManager.LoadScene (0);
					break;
				}
			}
			yield return null;
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

	// Letter
	private GameObject CreateLetter(string[] text, VideoClip[] clips){
		GameObject rtn = Instantiate (letter, Vector3.zero, Quaternion.identity);
		rtn.GetComponent<LetterController> ().SetText (text);
		rtn.GetComponent<LetterController> ().SetVideo (clips);
		PlayerFunctionConstraint (false, false, false, -1);
		return rtn;
	}

	// Dialog box
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
