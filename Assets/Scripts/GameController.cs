using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class GameController : MonoBehaviour {

	private bool game_over = false;
	public static GameController instance;

	void Start () {
		if (instance != null && instance != this) {
			Destroy (this);
		} else {
			instance = this;
		}
		Screen.SetResolution (1920, 1080, true);
	}
	
	void Update () {
		if (game_over) {
			for (int i = 0; i < 4; ++i) {
				if (InputManager.Devices [i].Action3) {
					SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
				}
			}
		}
	}

	public void SetGameOver() {
		game_over = true;
		WinningTextDisplayer.instance.ShowWinningText ();
	}

	public bool GetGameOver() {
		return game_over;
	}
}
