using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeController : MonoBehaviour {

	GameObject[] players;
	GameObject[] sprites;
	public Sprite button_A;
	public Sprite button_B;

	int[] choice;

	void Start () {
		players = new GameObject[4];
		sprites = new GameObject[4];
		choice = new int[4];
		for (int i = 0; i < 4; ++i) {
			players [i] = GameObject.Find ("Players/player" + (i + 1).ToString ());
			sprites [i] = GameObject.Find ("P" + (i + 1).ToString () + "Sprite");
			choice [0] = 0;
		}
	}
	
	void Update () {
		for (int i = 0; i < 4; ++i) {
			if (players [i].GetComponent<PlayerControl> ().GetInputDevice ().Action1) {
				choice [i] = 1;
				sprites [i].GetComponent<SpriteRenderer> ().sprite = button_A;
			}
			if (players [i].GetComponent<PlayerControl> ().GetInputDevice ().Action2) {
				choice [i] = 2;
				sprites [i].GetComponent<SpriteRenderer> ().sprite = button_B;
			}
		}
		int next = CheckProceed ();
		if (next == 1) {
			SceneManager.LoadScene ("tutorial");
		} else if (next == 2) {
			SceneManager.LoadScene ("main");
		}

	}

	int CheckProceed(){
		int result = choice [0];
		for (int i = 1; i < 4; ++i) {
			if (result != choice [i])
				return 0;
		}
		return result;
	}
}
