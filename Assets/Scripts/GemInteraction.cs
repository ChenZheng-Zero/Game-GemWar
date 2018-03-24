using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GemInteraction : MonoBehaviour {

	private bool holding = false;
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

		if (input_device.Action1 && !holding) {
			Collider collider = PublicFunctions.instance.FindObjectOnPosition (transform.position + grid_base_movement.GetDirection ());
			Debug.Log (collider.tag);
			if (collider && (collider.CompareTag ("gem_blue") || collider.CompareTag ("gem_red"))) {
				collider.GetComponent<BoxCollider> ().enabled = false;
				collider.transform.position = transform.position + Vector3.up * 0.8f;
				Hold (collider.gameObject);
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
