using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseScoreController: MonoBehaviour {

	private string own_color;
	private ScoreJuice score_juice;
	private List<GameObject> scores = new List<GameObject> ();

	public int current_score = 3;

	void Start () {
		score_juice = GetComponent<ScoreJuice> ();

		foreach (Transform child in transform) {
			if (child.name != "star_burst") {
				scores.Add (child.gameObject);
			}
		}

		if (tag == "base_blue") {
			own_color = "blue";
		} else {
			own_color = "red";
		}
	}

	void Update () {
		if (current_score == scores.Count && SceneManager.GetActiveScene ().name == "main") {
			GameController.instance.SetGameOver ();
		}
	}

	public void AddScore(string color) {
		score_juice.StarBurst ();
		score_juice.StartBounceCoroutine ();

		++current_score;
		scores [current_score - 1].GetComponent<SpriteRenderer> ().enabled = true;

		if (SceneManager.GetActiveScene ().name == "sudden_death") {
			if (own_color == "blue") {
				ScoreDisplayer.instance.BlueWinScore ();
			} else {
				ScoreDisplayer.instance.RedWinScore ();
			}
			GameController.instance.SetGameOver ();
		} else {
			if (own_color == "blue" && color == "blue") {
				ScoreDisplayer.instance.BlueAvoidPotentialLose ();
			} else if (own_color == "blue" && color == "red") {
				ScoreDisplayer.instance.BlueWinScore ();
			} else if (own_color == "red" && color == "blue") {
				ScoreDisplayer.instance.RedWinScore ();
			} else {
				ScoreDisplayer.instance.RedAvoidPotentialLose ();
			}
		}
	}

	public void LoseScore() {
		--current_score;
		scores [current_score].GetComponent<SpriteRenderer> ().enabled = false;

		if (own_color == "blue") {
			ScoreDisplayer.instance.BluePotentialLoseScore ();
		} else {
			ScoreDisplayer.instance.RedPotentialLoseScore ();
		}
	}


	public int GetScore() {
		return current_score;
	}
}
