using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScoreController: MonoBehaviour {

	private string own_color;
	private int current_score = 5;
	private int potential_losing_score = 0;
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

		if (color != own_color) {
			if (own_color == "blue") {
				ScoreDisplayer.instance.ModifyBlueScore (1);
				ScoreDisplayer.instance.ModifyRedScore (-1);
			} else {
				ScoreDisplayer.instance.ModifyBlueScore (-1);
				ScoreDisplayer.instance.ModifyRedScore (1);
			}
		}

		scores [current_score - 1].GetComponent<SpriteRenderer> ().enabled = true;
	}

//	public void PotentialLoseScore() {
//		++potential_losing_score;
//	}

	public void LoseScore() {
		--current_score;
//		--potential_losing_score;
		scores [current_score].GetComponent<SpriteRenderer> ().enabled = false;
	}


	public int GetScore() {
		return current_score;
	}
}
