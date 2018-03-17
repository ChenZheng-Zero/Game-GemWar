using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownDisplayer : MonoBehaviour {

	private int time_left;
	private Text text;
	private Vector3 original_position;
	private Vector3 central_position;
	public int total_seconds = 99;

	void Start () {
		time_left = total_seconds;
		text = GetComponent<Text> ();
		original_position = gameObject.GetComponent<RectTransform> ().localPosition;
		central_position = new Vector3 (0f, -720.7f, 0);
		StartCoroutine (CountDownCoroutine ());
	}

	void Update () {
		text.text = time_left.ToString ();
		if (time_left <= 10 || time_left % 15 == 0 || time_left % 15 == 1) {
			gameObject.GetComponent<RectTransform> ().localPosition = central_position;
		} else {
			gameObject.GetComponent<RectTransform> ().localPosition = original_position;
		}
		if (time_left <= 10) {
			text.color = Color.red;
		} else {
			text.color = Color.white;
		}
	}

	private IEnumerator CountDownCoroutine() {
		while (time_left > 0) {
			yield return new WaitForSecondsRealtime (1.0f);
			--time_left;
		}
		GameController.instance.SetGameOver ();
	}
}
