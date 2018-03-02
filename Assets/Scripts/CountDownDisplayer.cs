using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownDisplayer : MonoBehaviour {

	private int time_left;
	private Text text;

	public int total_seconds = 99;

	void Start () {
		time_left = total_seconds;
		text = GetComponent<Text> ();
		StartCoroutine (CountDownCoroutine ());
	}
	
	void Update () {
		text.text = time_left.ToString ();
	}

	private IEnumerator CountDownCoroutine() {
		while (time_left > 0) {
			yield return new WaitForSecondsRealtime (1.0f);
			--time_left;
		}
		GameController.instance.SetGameOver ();
	}
}
