using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuddenDeathController : MonoBehaviour {

	private bool sudden_death_mode = false;
	private GameObject[] players;

	private Text text;
	private bool warning = false;

	public float warning_max_alpha = 0.3f;
	public float warning_interval = 1.0f;
	public RawImage warning_panel;

	void Start () {
		text = GetComponent<Text> ();
		players = new GameObject[4];
		for (int i = 0; i < 4; ++i) {
			players [i] = GameObject.FindGameObjectWithTag ("player" + (i + 1).ToString ());
		}

		if (SceneManager.GetActiveScene ().name == "sudden_death") {
			sudden_death_mode = true;
		}

		if (sudden_death_mode) {
			Debug.Log ("sudden death");
//			GameObject countdown = GameObject.Find ("Canvas/Panel/CountDown");
//			countdown.GetComponent<Text> ().enabled = false;
//			countdown.GetComponent<CountDownDisplayer> ().enabled = false;
//			GameObject score_bar = GameObject.Find ("Canvas/Panel/ScoreBar");
//			score_bar.GetComponent<Image> ().enabled = false;
//			foreach (Transform child in score_bar.transform) {
//				child.gameObject.GetComponent<Image> ().enabled = false;
//			}
			StartCoroutine (EnterSuddenDeath ());
		}
	}

	void Update () {
		if (sudden_death_mode) {
			if (CheckHoldingStatus () == true || text.enabled == true) {
				if (!warning) {
					warning = true;
					StartCoroutine (WarningCoroutine ());
				}
			} 
			else {
				warning = false;
				int blue_score = ScoreDisplayer.instance.GetBlueScore ();
				int red_score = ScoreDisplayer.instance.GetRedScore ();
				if (blue_score != red_score){
					GameController.instance.SetGameOver ();
				}
			}
		}
	}

	private bool CheckHoldingStatus(){
		for (int i = 0; i < 4; i++) {
			if (players [i].GetComponent<GemInteraction> ().GetHolding ()) {
				return true;
			}
		}
		return false;
	}

	private IEnumerator EnterSuddenDeath() {
		DisablePlayers ();
		warning = true;
		text.text = "Sudden Death Mode";
		StartCoroutine (WarningCoroutine());
		yield return new WaitForSeconds (3f);
		text.enabled = false;
		warning = false;
		EnablePlayers ();
	}

	private IEnumerator WarningCoroutine() {
		Color color = warning_panel.color;

		while (warning) {
			for (float t = 0.0f; t < warning_interval; t += Time.deltaTime) {
				color.a = warning_max_alpha * t / warning_interval;
				warning_panel.color = color;
				yield return null;
			}

			for (float t = 0.0f; t < warning_interval; t += Time.deltaTime) {
				color.a = warning_max_alpha * (1 - t / warning_interval);
				warning_panel.color = color;
				yield return null;
			}
		}

		color.a = 0.0f;
		warning_panel.color = color;
	}

	private void DisablePlayers(){
		for (int i = 0; i < 4; ++i) {
			players [i].GetComponent<GridBaseMovement> ().enabled = false;
		}
	}

	private void EnablePlayers(){
		for (int i = 0; i < 4; ++i) {
			players [i].GetComponent<GridBaseMovement> ().enabled = true;
		}
	}
}
