using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningTextDisplayer : MonoBehaviour {

	public Text winning_text_object;
	public Text restart_text_object;
	public float scale_grow_duration = 0.5f;
	public float scale_shrink_duration = 0.5f;
	public float max_scale = 4.0f;
	public float final_scale = 2.0f;
	public static WinningTextDisplayer instance;

	void Start () {
		if (instance != null && instance != this) {
			Destroy (this);
		} else {
			instance = this;
		}
	}

	public void ShowWinningText() {
		string winning_text;
		int blue_score = ScoreDisplayer.instance.GetBlueScore ();
		int red_score = ScoreDisplayer.instance.GetRedScore ();

		if (blue_score == red_score) {
			winning_text = "~ Even ~";
		} else if (blue_score > red_score) {
			winning_text = "Team Blue Win!!!";
		} else {
			winning_text = "Team Red Win!!!";
		}

		StartCoroutine (TextScaleCoroutine (winning_text));
	}

	private IEnumerator TextScaleCoroutine(string winning_text) {
		winning_text_object.text = winning_text;
		winning_text_object.transform.localScale = Vector3.zero;
		winning_text_object.GetComponent<Text>().enabled = true;

		for (float t = 0.0f; t < scale_grow_duration; t += Time.deltaTime) { 
			winning_text_object.transform.localScale = t / scale_grow_duration * max_scale * Vector3.one;
			yield return null;
		}

		for (float t = 0.0f; t < scale_shrink_duration; t += Time.deltaTime) { 
			winning_text_object.transform.localScale = (final_scale + (1 - t / scale_grow_duration) * (max_scale - final_scale)) * Vector3.one;
			yield return null;
		}

		restart_text_object.GetComponent<Text>().enabled = true;
	}
}
