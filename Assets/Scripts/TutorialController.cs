using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
	/* Tutorial event driven system
	 * 0: Collect rock tutorial->
	 * 1: Place rock tutorial
	 * 2: Push rock tutorial->
	 * 3: Break rock tutorial (self rock, opponent rock)
	 * 4: Kill a fake opponent->
	 * 5: Carry gem tutorial
	 */

	public GameObject rock_collectable;



	int everyone_progress = 0;
	int[] player_progress;

	bool go_next_level = true;

	GameObject[] players;

	void Start () {
		/* initialize player progress to 0 */
		player_progress = new int[4];
		for (int i = 0; i < 4; ++i) {
			player_progress [i] = 0;
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
		// TODO: Add more cases
		}
	}

	bool CheckEveryPlayer(int level){
		for (int i = 0; i < 4; ++i)
			if (player_progress [i] == level)
				return false;
		return true;
	}
		
		
	IEnumerator Level0(){
		Debug.Log ("Level 0");
		Instantiate (rock_collectable, new Vector3 (-6f, 2f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (6f, 2f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (-6f, -2f, 0), Quaternion.identity);
		Instantiate (rock_collectable, new Vector3 (6f, -2f, 0), Quaternion.identity);

		while (true) {
			for (int i = 0; i < 4; ++i) {
				if (player_progress [i] == 0 && !players [i].GetComponent<RockBarDisplayer> ().IfRockCountZero ()) {
					// TODO: add UI to tell player level finished.
					player_progress[i] += 1;
					// TODO: do other things to player
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
		yield return null;
	}


}
