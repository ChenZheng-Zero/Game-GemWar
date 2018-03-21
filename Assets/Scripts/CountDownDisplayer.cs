using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownDisplayer : MonoBehaviour {

	private int time_left;
	private Text text;
	private Text time_alert_time;
	private Text time_alert_text;
	private bool alerting = false;
	private bool warning = false;

	public int total_seconds = 99;
	public float time_alert_stay_time = 1.0f;
	public float time_alert_fade_time = 1.0f;
	public float warning_max_alpha = 0.3f;
	public float warning_interval = 1.0f;
	public GameObject time_alert;
	public RawImage warning_panel;

	void Start () {
		time_left = total_seconds;
		text = GetComponent<Text> ();

		time_alert_time = time_alert.transform.GetChild (0).GetComponent<Text> ();
		time_alert_text = time_alert.transform.GetChild (1).GetComponent<Text> ();

		StartCoroutine (CountDownCoroutine ());
	}

	void Update () {
		text.text = time_left.ToString ();

		if ((time_left == 60 || time_left == 30 || time_left == 10) && !alerting) {
			StartCoroutine (TimeAlertCoroutine (time_left));
		}

		if (time_left <= 10) {
			if (!warning) {
				StartCoroutine (WarningCoroutine ());
				text.color = Color.red;
			}
		} else {
			text.color = Color.white;
		}
	}

	private IEnumerator WarningCoroutine() {
		warning = true;
		Color color = warning_panel.color;
		Debug.Log (color);

		while (time_left != 0) {
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

		warning = false;
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
