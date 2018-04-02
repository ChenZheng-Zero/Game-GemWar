using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class BaseInteraction : MonoBehaviour {

	private bool key_down = false;
	private string own_base_tag;
	private string opponent_base_tag;
	private Object opponent_gem_prefab;
	private InputDevice input_device;
	private Reborn reborn;
	private GemInteraction gem_interaction;
	private GridBaseMovement grid_base_movement;
	private PlayerDataController player_data_controller;

	void Start () {
		reborn = GetComponent<Reborn> ();
		gem_interaction = GetComponent<GemInteraction> ();
		grid_base_movement = GetComponent <GridBaseMovement> ();
		input_device = GetComponent<PlayerControl> ().GetInputDevice ();
		player_data_controller = GetComponent<PlayerDataController> ();

		string own_color =  GetComponent<PlayerControl> ().GetOwnColor ();
		string opponent_color =  GetComponent<PlayerControl> ().GetOpponentColor ();

		own_base_tag = "base_" + own_color;
		opponent_base_tag = "base_" + opponent_color;
		opponent_gem_prefab = Resources.Load ("Prefabs/gem_" + opponent_color, typeof(GameObject));
	}

	private bool CheckBase(Vector3 pos) {
		Collider collider = PublicFunctions.instance.FindObjectOnPosition (pos);
		if (collider && collider.tag == own_base_tag) {
			if (gem_interaction.GetHolding ()) {
				gem_interaction.Remove ();
				player_data_controller.AddScore ();
				collider.GetComponent<BaseScoreController> ().AddScore (gem_interaction.GetHoldingGemColor ());
				GameObject.Find ("SoundController").GetComponent<PlaySound> ().PlayGemdownSound ();
			}
			return true;
		} else if (collider && collider.tag == opponent_base_tag) {
			if (!gem_interaction.GetHolding () && collider.GetComponent<BaseScoreController>().GetScore () > 0) {
				GameObject gem = (GameObject)Instantiate (opponent_gem_prefab, transform.position + Vector3.up * 0.8f, Quaternion.identity);
				gem_interaction.Hold (gem);
				collider.GetComponent<BaseScoreController> ().LoseScore ();
				GameObject.Find ("SoundController").GetComponent<PlaySound> ().PlayGemupSound ();
			}
			return true;
		}
		return false;
	}

	void Update () {
		if (reborn.GetReborning() || GameController.instance.GetGameOver ()) {
			return;
		}

		if (input_device.Action1 && !key_down) {
			key_down = true;
			Vector3 direction = grid_base_movement.GetDirection ();
			CheckBase (transform.position + direction * 0.7f);
		}

		if (!input_device.Action1 && key_down) {
			key_down = false;
		}
	}
}
