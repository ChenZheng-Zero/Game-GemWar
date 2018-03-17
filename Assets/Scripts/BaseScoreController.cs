using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScoreController: MonoBehaviour {

	private string own_color;
	private int current_score = 5;
	private List<GameObject> scores = new List<GameObject> ();

	void Start () {
		foreach (Transform child in transform) {
			scores.Add (child.gameObject);
		}

		if (tag == "base_blue") {
			own_color = "blue";
		} else {
			own_color = "red";
		}
	}

	void Update () {
		
	}

	public void AddScore(string color) {
		++current_score;
		scores [current_score - 1].GetComponent<SpriteRenderer> ().enabled = true;

//		if (color != own_color) {
//			if (own_color == "blue") {
//				ScoreDisplayer.instance.ModifyBlueScore (1);
//				ScoreDisplayer.instance.ModifyRedScore (-1);
//			} else {
//				ScoreDisplayer.instance.ModifyBlueScore (-1);
//				ScoreDisplayer.instance.ModifyRedScore (1);
//			}
//		}

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
