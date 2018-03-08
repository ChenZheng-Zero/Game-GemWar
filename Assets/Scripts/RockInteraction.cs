using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class RockInteraction : MonoBehaviour {

	private bool action1_key_down = false;
	private bool action2_key_down = false;
	private bool removing_opponent_rock = false;
	private string own_rock_tag;
	private string opponent_rock_tag;
	private Object own_rock_prefab;
	private InputDevice input_device;
	private Reborn reborn;
	private GemInteraction gem_interaction;
	private GridBaseMovement grid_base_movement;
	private RockBarDisplayer rock_bar_displayer;

	public float rock_speed = 6.0f;

	void Start () {
		reborn = GetComponent<Reborn> ();
		gem_interaction = GetComponent<GemInteraction> ();
		grid_base_movement = GetComponent<GridBaseMovement> ();
		rock_bar_displayer = GetComponent<RockBarDisplayer> ();
		input_device = GetComponent<PlayerControl> ().GetInputDevice ();
		own_rock_tag = "rock_" + GetComponent<PlayerControl> ().GetOwnColor ();
		opponent_rock_tag = "rock_" + GetComponent<PlayerControl> ().GetOpponentColor ();
		own_rock_prefab = Resources.Load ("Prefabs/" + own_rock_tag, typeof(GameObject));
	}


	private int CheckObstacle(Vector3 pos) {
		Collider collider = PublicFunctions.instance.FindObjectOnPosition (pos);

		if (collider && collider.CompareTag (own_rock_tag)) {
			return 1;
		} else if (collider && (collider.CompareTag("base_blue") || collider.CompareTag("base_red") || collider.CompareTag(opponent_rock_tag) ||
			collider.CompareTag("reborn_red") || collider.CompareTag("reborn_blue") || collider.CompareTag("wall") || collider.CompareTag("gem_blue") || collider.CompareTag("gem_red") ||
			PublicFunctions.instance.GetTeamNumber(tag) == PublicFunctions.instance.GetTeamNumber(collider.tag))) {
			return -1;
		} else {
			return 0;
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
	
	void Update () {
		if (reborn.GetReborning() || GameController.instance.GetGameOver ()) {
			return;
		}

		if (input_device.Action1 && !action1_key_down && !gem_interaction.GetHolding ()) {
			action1_key_down = true;

			Vector3 direction = grid_base_movement.GetDirection ();
			Vector3 offset = GetOffset ();
			Vector3 facing_position = transform.position + direction + offset;

			Collider collider = PublicFunctions.instance.FindObjectOnPosition (facing_position);

			if (collider && collider.CompareTag (own_rock_tag)) {
				collider.GetComponent<Rigidbody> ().velocity = rock_speed * direction;
				collider.GetComponent<RockController> ().SetMovingDirection (direction);
			} else if (!rock_bar_displayer.IfRockCountZero () && collider && PublicFunctions.instance.GetTeamNumber(tag) + PublicFunctions.instance.GetTeamNumber(collider.tag) == 3) {
				rock_bar_displayer.ModifyRockCount (-1);
				GameObject rock = (GameObject)Instantiate (own_rock_prefab, transform.position + direction + offset, Quaternion.identity);
				collider.GetComponent<Reborn> ().StartRebornCoroutine ();
				StartCoroutine (DelayDestroyCoroutine (rock));
			} else if (!rock_bar_displayer.IfRockCountZero () && !(collider && (collider.CompareTag("base_blue") || collider.CompareTag("wall") || collider.CompareTag("base_red") || collider.CompareTag("gem_blue") || collider.CompareTag("gem_red") || collider.CompareTag(opponent_rock_tag) ||
				collider.CompareTag("reborn_red") || collider.CompareTag("reborn_blue") || PublicFunctions.instance.GetTeamNumber(tag) == PublicFunctions.instance.GetTeamNumber(collider.tag)))) {

				rock_bar_displayer.ModifyRockCount (-1);
				Instantiate (own_rock_prefab, facing_position, Quaternion.identity);
			}
		}

		if (input_device.Action2 && !action2_key_down && !gem_interaction.GetHolding ()) {
			action2_key_down = true;

			Vector3 direction = grid_base_movement.GetDirection ();
			Vector3 offset = GetOffset ();

			Collider collider = PublicFunctions.instance.FindObjectOnPosition (transform.position + direction + offset);

			if (collider && collider.CompareTag (own_rock_tag)) {
				Destroy (collider.gameObject);
			} else if (collider && collider.CompareTag (opponent_rock_tag)) {
				removing_opponent_rock = true;
				collider.GetComponent<RockController> ().RemoveByPlayer (gameObject);
			}
		}

		if (!input_device.Action1 && action1_key_down) {
			action1_key_down = false;
		}

		if (!input_device.Action2 && action2_key_down) {
			action2_key_down = false;
			removing_opponent_rock = false;
		}

		if (removing_opponent_rock) {
			if (reborn.GetReborning ()) {
				removing_opponent_rock = false;
			} else {
				Vector3 direction = grid_base_movement.GetDirection ();
				Vector3 offset = GetOffset ();
				Collider collider = PublicFunctions.instance.FindObjectOnPosition (transform.position + direction + offset);

				if (!collider || !collider.CompareTag (opponent_rock_tag) || collider.transform.position != transform.position + direction + offset) {
					removing_opponent_rock = false;
				}
			}
		}
	}

	private IEnumerator DelayDestroyCoroutine(GameObject rock) {
		yield return null;
		Destroy (rock);
	}

	public bool GetRemovingOpponentRock() {
		return removing_opponent_rock;
	}
}
