using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reborn : MonoBehaviour {

	private bool reborning = false;
	private Vector3 reborn_position;
	private GameObject rock_bar;
	private Rigidbody rb;
	private BoxCollider bc;
	private SpriteRenderer sr;
	private GemInteraction gem_interaction;

	public float reborn_duration = 5.0f;
	public GameObject reborn_place;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		bc = GetComponent<BoxCollider> ();
		sr = GetComponent<SpriteRenderer> ();
		gem_interaction = GetComponent<GemInteraction> ();

		reborn_position = reborn_place.transform.position;

		foreach (Transform child in transform) {
			if (child.name == "rock_bar") {
				rock_bar = child.gameObject;
			}
		}
	}
	
	public void StartRebornCoroutine() {
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
		yield return new WaitForSeconds (reborn_duration);
		transform.position = reborn_position;

		rock_bar.SetActive (true);
		sr.enabled = true;
		bc.enabled = true;
		reborning = false;
	}

	public bool GetReborning() {
		return reborning;
	}
}
