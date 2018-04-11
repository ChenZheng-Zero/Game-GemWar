using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

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

	public GameObject panel_text;
	public GameObject panel_buttonA;
	public GameObject panel_buttonB;

	GameObject[] players;
	GameObject[] ticks;

	int everyone_progress = 0;
	float[] player_progress;
	bool go_next_level = true;
	public int start_level = 0; // For debug only
	GameObject pointer_set;

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

		panel_text.GetComponent<Text> ().text = "";
		panel_buttonA.GetComponent<Image> ().enabled = false;
		panel_buttonB.GetComponent<Image> ().enabled = false;
		pointer_set = GameObject.Find ("Pointers");
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

		string text_1 = "Tutorial 1: Box\n\nStep 1: Break red and blue Boxes\nStep 2: Shoot your box onto the opponent.";

		GameObject letter_box = CreateLetter (new string[]{ text_1 }, new VideoClip[]{video_clips[0]});
		GameObject[] fake_players = new GameObject[4];
		pointer_set.SetActive (false);

		bool initialized = false;
		while (true) {
			if (letter_box) {
				yield return null;
				continue;
			}
			if (!initialized) {
				initialized = true;
				fake_players[0] = Instantiate (fake_red_opponent, new Vector3 (-1f, 2f, 0), Quaternion.identity);
				fake_players[1] = Instantiate (fake_red_opponent, new Vector3 (1f, 2f, 0), Quaternion.identity);
				fake_players[2] = Instantiate (fake_blue_opponent, new Vector3 (-1f, -2f, 0), Quaternion.identity);
				fake_players[3] = Instantiate (fake_blue_opponent, new Vector3 (1f, -2f, 0), Quaternion.identity);
				panel_text.GetComponent<Text> ().text = "Place or push a box in the front.\nBreak the boxes.";
				panel_buttonA.GetComponent<Image> ().enabled = true;
				panel_buttonB.GetComponent<Image> ().enabled = true;
			}
			PlayerFunctionConstraint (true, false, true);
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 0.0f && !fake_players[i]) {
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

		string text_4 = "Tutorial 2: Carry Gem\n\nPress A to steal your opponents' gems and drop them in your base.";
		string text_5 = "Congratulations! You finish all tutorials.\n\nThe team carries more gems will win.";
		string text_6 = "BUFF";

		GameObject letter_box = CreateLetter (new string[]{text_4}, new VideoClip[]{video_clips[1]});
		GameObject finish_letter = null;
		bool check_finish_letter = false;

		pointer_set.SetActive (true);

		bool initialized = false;
		while (true) {
			if (letter_box) {
				yield return null;
				continue;
			}
			if (!initialized) {
				panel_text.GetComponent<Text> ().text = "Get and drop gems.";
				panel_buttonA.GetComponent<Image> ().enabled = true;
				panel_buttonB.GetComponent<Image> ().enabled = false;
				initialized = true;
				Destroy(GameObject.Find("Walls/TemporaryWalls"));
				PlayerFunctionConstraint (true, true, true);
			}
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 1.0f &&
					players [i].GetComponent<GemInteraction> ().GetHolding()) {
					player_progress [i] = 1.5f;
				} 
				if (player_progress [i] == 1.5f) {
					if (players [i].GetComponent<PlayerDataController> ().GetScore() > 0) {
						player_progress [i] = 2.0f;
					}
				}
			}
			if (CheckEveryPlayer (1)) {
				if (!check_finish_letter) {
					check_finish_letter = true;
					finish_letter = CreateLetter (new string[]{ text_5, text_6 }, new VideoClip[]{ null, null });
				}
				if (check_finish_letter && !finish_letter) {
					SceneTransition.instance.TranistionTo ("welcome");
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

	public void BUFF_pannel(){
		panel_text.GetComponent<Text> ().text = "Buffs in the game.";
		panel_buttonA.GetComponent<Image> ().enabled = false;
		panel_buttonB.GetComponent<Image> ().enabled = false;
	}


}
