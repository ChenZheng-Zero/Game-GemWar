using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class GameController : MonoBehaviour {

	private bool game_over = false;
	public static GameController instance;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this);
		} else {
			instance = this;
		}
		Screen.SetResolution (1920, 1200, true);
	}
	
	void Update () {
		if (game_over) {
			for (int i = 0; i < 4; ++i) {
				if (InputManager.Devices [i].Action3) {
					SceneManager.LoadSceneAsync ("main");
				}
			}
		}
	}

	public void SetGameOver() {
		int blue_score = ScoreDisplayer.instance.GetBlueScore ();
		int red_score = ScoreDisplayer.instance.GetRedScore ();

		if (blue_score == red_score) {
			Debug.Log ("Even");
			SceneManager.LoadSceneAsync ("sudden_death");
		} else {
			game_over = true;
//			WinningTextDisplayer.instance.ShowWinningText (blue_score, red_score);
			GetComponent<PlayerDataDisplayer> ().Display ();
		}
	}

	public bool GetGameOver() {
		return game_over;
	}
}
