using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GemInteraction : MonoBehaviour {

	private bool holding = false;
	private bool key_down = false;
	private GameObject gem = null;
	private InputDevice input_device;
	private Reborn reborn;
	private GridBaseMovement grid_base_movement;
	private PlayerDataController player_data_controller;

	void Start () {
		reborn = GetComponent<Reborn> ();
		input_device = GetComponent<PlayerControl> ().GetInputDevice ();
		grid_base_movement = GetComponent<GridBaseMovement> ();
		player_data_controller = GetComponent<PlayerDataController> ();
	}

	void Update () {
		if (reborn.GetReborning() || GameController.instance.GetGameOver ()) {
			return;
		}

		if (input_device.Action1 && !key_down) {
			key_down = true;

			if (holding) {
				Vector3 facing_position = transform.position + grid_base_movement.GetDirection () + GetOffset ();
				Collider collider = PublicFunctions.instance.FindObjectOnPosition (facing_position);
				if (!collider || !(collider.CompareTag ("base_blue") || collider.CompareTag ("base_red") || collider.CompareTag ("gem_blue") || collider.CompareTag ("gem_red") || 
					collider.CompareTag ("wall") || collider.CompareTag ("rock_blue") || collider.CompareTag ("rock_red") || collider.CompareTag ("reborn_blue") || collider.CompareTag ("reborn_red") ||
					collider.CompareTag ("player1") || collider.CompareTag ("player2") || collider.CompareTag ("player3") || collider.CompareTag ("player4"))) {

					holding = false;
					gem.transform.parent = null;
					gem.transform.position = facing_position;
					gem.GetComponent<BoxCollider> ().enabled = true;
					GameObject.Find ("SoundController").GetComponent<PlaySound> ().PlayGemdownSound ();
				}
			} else {
				Collider collider = PublicFunctions.instance.FindObjectOnPosition (transform.position + grid_base_movement.GetDirection ());
				if (collider && (collider.CompareTag ("gem_blue") || collider.CompareTag ("gem_red"))) {
					collider.GetComponent<BoxCollider> ().enabled = false;
					collider.transform.position = transform.position + Vector3.up * 0.8f;
					Hold (collider.gameObject);
					GameObject.Find ("SoundController").GetComponent<PlaySound> ().PlayGemupSound ();
				}
			}
		}

		if (!input_device.Action1 && key_down) {
			key_down = false;
		}
	}

	private Vector3 GetOffset() {
		Vector3 direction = grid_base_movement.GetDirection ();

		if (direction == Vector3.up || direction == Vector3.down) {
			float dec = transform.position.y - Mathf.Floor (transform.position.y);
			if (dec > 0.0f && dec < 0.5f) {
				return new Vector3 (0.0f, 0.0f - dec, 0.0f);
			} else if (dec > 0.5f && dec < 1.0f) {
				return new Vector3 (0.0f, 1.0f - dec, 0.0f);
			} else {
				return Vector3.zero;
			}
		} else {
			float dec = transform.position.x - Mathf.Floor (transform.position.x);
			if (dec > 0.0f && dec < 0.5f) {
				return new Vector3 (0.0f - dec, 0.0f, 0.0f);
			} else if (dec > 0.5f && dec < 1.0f) {
				return new Vector3 (1.0f - dec, 0.0f, 0.0f);
			} else {
				return Vector3.zero;
			}
		}
	}
	
	public void Hold(GameObject _gem) {
		holding = true;
		gem = _gem;
		gem.transform.parent = transform;
		player_data_controller.AddGemPickUp ();
	}

	public void Remove() {
		if (gem != null) {
			Destroy (gem);
		}
		holding = false;
	}

	public void DropGemAfterHit() {
		if (holding) {
			gem.transform.parent = null;
			gem.GetComponent<BoxCollider> ().enabled = true;
			gem.transform.position = PublicFunctions.instance.RoundVector3 (transform.position);
			holding = false;
		}
	}

	public bool GetHolding() {
		return holding;
	}

	public string GetHoldingGemColor() {
		if (gem == null) {
			return "";
		} else if (gem.tag == "gem_blue") {
			return "blue";
		} else {
			return "red";
		}
	}
}
