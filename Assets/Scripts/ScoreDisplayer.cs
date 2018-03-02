using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour {

	private int blue_score = 5;
	private int red_score = 5;

	public Text blue_score_text;
	public Text red_score_text;
	public static ScoreDisplayer instance;

	void Start () {
		if (instance != null && instance != this) {
			Destroy (this);
		} else {
			instance = this;
		}
	}

	void Update () {
		blue_score_text.text = blue_score.ToString ();
		red_score_text.text = red_score.ToString ();
	}
	
	public void ModifyBlueScore(int val) {
		blue_score += val;
	}

	public void ModifyRedScore(int val) {
		red_score += val;
	}

	public int GetBlueScore() {
		return blue_score;
	}

	public int GetRedScore() {
		return red_score;
	}
}
