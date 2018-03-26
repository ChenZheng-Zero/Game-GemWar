using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reborn : MonoBehaviour {

	private bool reborning = false;
	private Vector3 reborn_position;
	private GameObject rock_bar;
	private Rigidbody rb;
	private Text time_left;
	private BoxCollider bc;
	private SpriteRenderer sr;
	private GemInteraction gem_interaction;
	private GridBaseMovement grid_base_movement;
	private PlayerDataController player_data_controller;

	public float reborn_duration = 5.0f;
	public GameObject reborn_place;
	public GameObject reborn_UI;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		bc = GetComponent<BoxCollider> ();
		sr = GetComponent<SpriteRenderer> ();
		time_left = reborn_UI.GetComponent<Text> ();
		gem_interaction = GetComponent<GemInteraction> ();
		grid_base_movement = GetComponent<GridBaseMovement> ();
		player_data_controller = GetComponent<PlayerDataController> ();

		reborn_position = reborn_place.transform.position;

		foreach (Transform child in transform) {
			if (child.name == "rock_bar") {
				rock_bar = child.gameObject;
			}
		}
	}

	public void StartRebornCoroutine() {
		player_data_controller.AddDeath ();
		gameObject.GetComponent<DeathController> ().StartDeadCoroutine();
		StartCoroutine (RebornCoroutine ());
	}

	private IEnumerator RebornCoroutine() {
		reborning = true;
		bc.enabled = false;
		sr.enabled = false;
		rock_bar.SetActive (false);

		if (gem_interaction.GetHolding ()) {
			gem_interaction.DropGemAfterHit ();
		}

		rb.velocity = Vector3.zero;
		grid_base_movement.SetOccupiedGridAfterReborn (reborn_position);

		time_left.text = ": " + reborn_duration.ToString ();
		reborn_UI.SetActive (true);
		for (float t = 0.0f; t < reborn_duration; t += Time.deltaTime) {
			time_left.text = ": " + Mathf.Round(reborn_duration - t).ToString ();
			yield return null;
		}

		transform.position = reborn_position;
		reborn_UI.SetActive (false);

		rock_bar.SetActive (true);
		sr.enabled = true;
		bc.enabled = true;
		reborning = false;
	}

	public bool GetReborning() {
		return reborning;
	}
}
