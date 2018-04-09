using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

	private RectTransform curtain_left_rt;
	private RectTransform curtain_right_rt;

	public GameObject curtain_left;
	public GameObject curtain_right;
	public float open_x = 1500.0f;
	public float close_x = 500.0f;
	public float speed = 500.0f;
	public static SceneTransition instance;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this);
		} else {
			instance = this;
		}

		curtain_left_rt = curtain_left.GetComponent<RectTransform> ();
		curtain_right_rt = curtain_right.GetComponent<RectTransform> ();

		StartCoroutine (OpenCurtain ());
	}
	
	private IEnumerator CloseCurtain(string scene) {
		yield return new WaitForSeconds (0.2f);

		curtain_left_rt.anchoredPosition = -open_x * Vector2.right;
		curtain_right_rt.anchoredPosition = open_x * Vector2.right;

		Vector2 left_origin_pos = curtain_left_rt.anchoredPosition;
		Vector2 right_origin_pos = curtain_right_rt.anchoredPosition;

		float total_time = (open_x - close_x) / speed;

		for (float t = 0; t < total_time; t += Time.deltaTime) {
			curtain_left_rt.anchoredPosition = left_origin_pos + t * speed * Vector2.right;
			curtain_right_rt.anchoredPosition = right_origin_pos - t * speed * Vector2.right;
			yield return null;
		}

		curtain_left_rt.anchoredPosition = -close_x * Vector2.right;
		curtain_right_rt.anchoredPosition = close_x * Vector2.right;

		DestroyRocks ();
		SceneManager.LoadSceneAsync (scene);
	}

	private IEnumerator OpenCurtain() {
		curtain_left_rt.anchoredPosition = -close_x * Vector2.right;
		curtain_right_rt.anchoredPosition = close_x * Vector2.right;

		yield return new WaitForSeconds (0.2f);

		Vector2 left_origin_pos = curtain_left_rt.anchoredPosition;
		Vector2 right_origin_pos = curtain_right_rt.anchoredPosition;

		float total_time = (open_x - close_x) / speed;

		for (float t = 0; t < total_time; t += Time.deltaTime) {
			curtain_left_rt.anchoredPosition = left_origin_pos - t * speed * Vector2.right;
			curtain_right_rt.anchoredPosition = right_origin_pos + t * speed * Vector2.right;
			yield return null;
		}

		curtain_left_rt.anchoredPosition = -open_x * Vector2.right;
		curtain_right_rt.anchoredPosition = open_x * Vector2.right;
	}

	public void TranistionTo(string scene) {
		StartCoroutine (CloseCurtain (scene));
	}

	private void DestroyRocks() {
		foreach (GameObject rock in GameObject.FindGameObjectsWithTag ("rock_blue")) {
			Destroy (rock);
		}
		foreach (GameObject rock in GameObject.FindGameObjectsWithTag ("rock_red")) {
			Destroy (rock);
		}
	}
}
