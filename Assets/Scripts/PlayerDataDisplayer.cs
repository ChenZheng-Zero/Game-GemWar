using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataDisplayer : MonoBehaviour {

	public GameObject player_data_board;
	public GameObject blue_score;
	public GameObject red_score;
	public List<GameObject> players = new List<GameObject> ();
	public List<GameObject> player_scores = new List<GameObject> ();
	public AnimationCurve slide_curve;
	public float slide_speed_coefficient = 1.0f;

	void Start () {
//		StartCoroutine (SlideCoroutine ());
	}
	
	void Update () {
//		UpdateData ();
	}

	public void Display () {
		UpdateData ();
		StartCoroutine (SlideCoroutine ());
	}

	private void UpdateData() {
		blue_score.GetComponent<Text> ().text = ScoreDisplayer.instance.GetBlueScore ().ToString ();
		red_score.GetComponent<Text> ().text = ScoreDisplayer.instance.GetRedScore ().ToString ();

		for (int i = 0; i < 4; ++i) {
			PlayerDataController data = players [i].GetComponent<PlayerDataController> ();
			player_scores [i].transform.GetChild (0).GetComponent<Text> ().text = data.GetKill ().ToString ();
			player_scores [i].transform.GetChild (1).GetComponent<Text> ().text = data.GetScore ().ToString ();
			player_scores [i].transform.GetChild (2).GetComponent<Text> ().text = data.GetShoot ().ToString ();
			player_scores [i].transform.GetChild (3).GetComponent<Text> ().text = data.GetDeath ().ToString ();
		}
	}

	private IEnumerator SlideCoroutine() {
		RectTransform board_rect = player_data_board.GetComponent<RectTransform> ();
		Vector3 pos = board_rect.anchoredPosition;
		float x_offset = pos.x;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * slide_speed_coefficient) {
			pos.x = x_offset * slide_curve.Evaluate (t);
			board_rect.anchoredPosition = pos;
			yield return null;
		}

		board_rect.anchoredPosition = Vector3.zero;
	}
}
