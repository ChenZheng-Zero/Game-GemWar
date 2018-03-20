using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownDisplayer : MonoBehaviour {

	private int time_left;
	private Text text;
	private Vector3 original_position;
	private Vector3 central_position;
	private Text time_alert_time;
	private Text time_alert_text;
	private bool alerting = false;

	public int total_seconds = 99;
	public float time_alert_stay_time = 1.0f;
	public float time_alert_fade_time = 1.0f;
	public GameObject time_alert;

	void Start () {
		time_left = total_seconds;
		text = GetComponent<Text> ();
		original_position = gameObject.GetComponent<RectTransform> ().localPosition;
		central_position = new Vector3 (0f, -720.7f, 0);

		time_alert_time = time_alert.transform.GetChild (0).GetComponent<Text> ();
		time_alert_text = time_alert.transform.GetChild (1).GetComponent<Text> ();

		StartCoroutine (CountDownCoroutine ());
	}

	void Update () {
		text.text = time_left.ToString ();
//		if (time_left <= 10 || time_left % 15 == 0 || time_left % 15 == 1) {
//			gameObject.GetComponent<RectTransform> ().localPosition = central_position;
//		} else {
//			gameObject.GetComponent<RectTransform> ().localPosition = original_position;
//		}

		if ((time_left == 60 || time_left == 30 || time_left == 10) && !alerting) {
			StartCoroutine (TimeAlertCoroutine (time_left));
		}

		if (time_left <= 10) {
			text.color = Color.red;
		} else {
			text.color = Color.white;
		}
	}

	private IEnumerator TimeAlertCoroutine(int time_left) {
		alerting = true;
		time_alert_time.text = time_left.ToString ();
		time_alert_time.enabled = true;
		time_alert_text.enabled = true;

		yield return new WaitForSeconds (time_alert_stay_time);

		float color_a = time_alert_time.color.a;
		Color color = time_alert_time.color;

		for (float t = 0.0f; t < time_alert_fade_time; t += Time.deltaTime) {
			color.a = color_a * (1 - t / time_alert_fade_time);
			time_alert_time.color = color;
			time_alert_text.color = color;
			yield return null;
		}

		time_alert_time.enabled = false;
		time_alert_text.enabled = false;
		color.a = color_a;
		time_alert_time.color = color;
		time_alert_text.color = color;
		alerting = false;
	}

	private IEnumerator CountDownCoroutine() {
		while (time_left > 0) {
			yield return new WaitForSecondsRealtime (1.0f);
			--time_left;
		}
		GameController.instance.SetGameOver ();
	}
}
