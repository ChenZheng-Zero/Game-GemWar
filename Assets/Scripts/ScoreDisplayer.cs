using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour {

	private int blue_score = 5;
	private int red_score = 5;
	private int blue_potential_lose = 0;
	private int red_potential_lose = 0;
	private Sprite score_block_blue_sprite;
	private Sprite score_block_red_sprite;
	private Sprite score_block_white_sprite;
	private List<GameObject> score_blocks = new List<GameObject> ();
	private List<bool> shake = new List<bool> ();

//	public Text blue_score_text;
//	public Text red_score_text;
	public float shake_interval = 0.1f;
	public float max_shake_degree = 5.0f;
	public float grow_and_shrink_duration = 0.5f;
	public float max_scale = 1.5f;
	public GameObject score_bar;
	public static ScoreDisplayer instance;

	void Start () {
		if (instance != null && instance != this) {
			Destroy (this);
		} else {
			instance = this;
		}

		score_block_blue_sprite = (Sprite)Resources.Load<Sprite> ("Sprites/score_block_blue");
		score_block_red_sprite = (Sprite)Resources.Load<Sprite> ("Sprites/score_block_red");
		score_block_white_sprite = (Sprite)Resources.Load<Sprite> ("Sprites/score_block_white");

		foreach (Transform child in score_bar.transform) {
			score_blocks.Add (child.gameObject);
			shake.Add (false);
		}
	}

	void Update () {
//		blue_score_text.text = blue_score.ToString ();
//		red_score_text.text = red_score.ToString ();

//		if (Input.GetKeyDown (KeyCode.Z)) {
//			BlueWinScore ();
//		} else if (Input.GetKeyDown (KeyCode.X)) {
//			RedWinScore ();
//		} else if (Input.GetKeyDown (KeyCode.C)) {
//			BlueAvoidPotentialLose ();
//		} else if (Input.GetKeyDown (KeyCode.V)) {
//			RedAvoidPotentialLose ();
//		} else if (Input.GetKeyDown (KeyCode.B)) {
//			BluePotentialLoseScore ();
//		} else if (Input.GetKeyDown (KeyCode.N)) {
//			RedPotentialLoseScore ();
//		}
	}




	private IEnumerator ShakeCoroutine(int index) {
		shake [index] = true;

		Image target_image = score_blocks [index].GetComponent<Image> ();
		Sprite origin_sprite = target_image.sprite;
		while (shake [index]) {
			for (float t = 0.0f; t < shake_interval / 4; t += Time.deltaTime) {
				score_blocks [index].transform.localRotation = Quaternion.Euler(Vector3.forward * (max_shake_degree * t / (shake_interval / 4)));
				yield return null;
			}

			for (float t = 0.0f; t < shake_interval / 2; t += Time.deltaTime) {
				score_blocks [index].transform.localRotation = Quaternion.Euler((Vector3.forward * (max_shake_degree - max_shake_degree * t / (shake_interval / 4))));
				yield return null;
			}

			for (float t = 0.0f; t < shake_interval / 4; t += Time.deltaTime) {
				score_blocks [index].transform.localRotation = Quaternion.Euler((Vector3.forward * (max_shake_degree * t / (shake_interval / 4) - max_shake_degree)));
				yield return null;
			}


//			target_image.sprite = score_block_white_sprite;
//			yield return new WaitForSeconds (blink_interval);
//			target_image.sprite = origin_sprite;
//			yield return new WaitForSeconds (blink_interval);
		}

//		if (blue_score - 1 >= index) {
//			target_image.sprite = score_block_blue_sprite;
//		} else {
//			target_image.sprite = score_block_red_sprite;
//		}

		score_blocks [index].transform.localRotation = Quaternion.Euler(Vector3.zero);
	}


	private IEnumerator GrowAndShrinkCoroutine(int index) {
		shake [index] = true;

		for (float t = 0.0f; t < grow_and_shrink_duration / 2; t += Time.deltaTime) {
			score_blocks [index].transform.localScale = Vector3.one * ((t / (grow_and_shrink_duration / 2) * (max_scale - 1)) + 1);
			yield return null;
		}

		for (float t = 0.0f; t < grow_and_shrink_duration / 2; t += Time.deltaTime) {
			score_blocks [index].transform.localScale = Vector3.one * (max_scale - (t / (grow_and_shrink_duration / 2) * (max_scale - 1)));
			yield return null;
		}

		score_blocks [index].transform.localScale = Vector3.one;
	}

	public void BlueWinScore() {
		blue_score += 1;
		red_score -= 1;
		red_potential_lose -= 1;

		score_blocks [blue_score - 1].GetComponent<Image> ().sprite = score_block_blue_sprite;
		StartCoroutine (GrowAndShrinkCoroutine (blue_score - 1));
		if (blue_potential_lose > 0) {
			shake [blue_score - blue_potential_lose - 1] = false;
		} else {
			shake [blue_score - 1] = false;
		}
	}

	public void RedWinScore() {
		blue_score -= 1;
		red_score += 1;
		blue_potential_lose -= 1;

		score_blocks [blue_score].GetComponent<Image> ().sprite = score_block_red_sprite;
		StartCoroutine (GrowAndShrinkCoroutine (blue_score));
		if (red_potential_lose > 0) {
			shake [blue_score + red_potential_lose] = false;
		} else {
			shake [blue_score] = false;
		}
	}

	public void BlueAvoidPotentialLose() {
		blue_potential_lose -= 1;
		shake [blue_score - blue_potential_lose - 1] = false;
	}

	public void RedAvoidPotentialLose() {
		red_potential_lose -= 1;
		shake [blue_score + red_potential_lose] = false;
	}

	public void BluePotentialLoseScore() {
		blue_potential_lose += 1;
		StartCoroutine (ShakeCoroutine (blue_score - blue_potential_lose));
	}

	public void RedPotentialLoseScore() {
		red_potential_lose += 1;
		StartCoroutine (ShakeCoroutine (blue_score + red_potential_lose - 1));
	}










//	public void ModifyBlueScore(int val) {
//		blue_score += val;
//	}
//
//	public void ModifyRedScore(int val) {
//		red_score += val;
//	}

	public int GetBlueScore() {
		return blue_score;
	}

	public int GetRedScore() {
		return red_score;
	}
}
