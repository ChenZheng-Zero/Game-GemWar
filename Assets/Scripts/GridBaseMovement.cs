﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GridBaseMovement : MonoBehaviour {

	private bool moving = false;
	private bool change_direction_moving = false;
	private Vector3 direction;
	private Vector3 previous_direction;
	private Rigidbody rb;
	private Animator animator;
	private InputDevice input_device;
	private Reborn reborn;
	private GemInteraction gem_interaction;
	private RockInteraction rock_interaction;

	public float movement_speed = 4.0f;
	public float input_magnitude_threshold = 0.8f;
	public float change_direction_threshold = 0.2f;
	public float holding_movement_speed = 2.0f;
	public Vector3 initial_direction = Vector3.down;

	void Start () {
		direction = initial_direction;
		previous_direction = initial_direction;
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		reborn = GetComponent<Reborn> ();
		gem_interaction = GetComponent<GemInteraction> ();
		rock_interaction = GetComponent<RockInteraction> ();
		input_device = GetComponent<PlayerControl> ().GetInputDevice ();
	}

	private Vector3 GetInputDirection() {
		float horizontal_input = input_device.LeftStickX;
		float vertical_input = input_device.LeftStickY;

		if (Mathf.Abs (horizontal_input) > Mathf.Abs (vertical_input)) {
			horizontal_input = 1.0f * Mathf.Sign (horizontal_input);
			vertical_input = 0.0f;
		} else {
			horizontal_input = 0.0f;
			vertical_input = 1.0f * Mathf.Sign (vertical_input);
		}

		return new Vector3 (horizontal_input, vertical_input, 0.0f);
	}

	private float GetInputMagnitude() {
		float horizontal_input_magnitude = Mathf.Abs (input_device.LeftStickX);
		float vertical_input_magnitude =  Mathf.Abs (input_device.LeftStickY);

		if (horizontal_input_magnitude > vertical_input_magnitude) {
			return horizontal_input_magnitude;
		} else {
			return vertical_input_magnitude;
		}
	}

	private bool CheckObstacle(Vector3 pos) {
		Collider collider = PublicFunctions.instance.FindObjectOnPosition (pos);

		if (collider && (collider.CompareTag("base_blue") || collider.CompareTag("base_red") || collider.CompareTag("gem_blue") || collider.CompareTag("gem_red") ||
			collider.CompareTag("wall") || collider.CompareTag("rock_blue") || collider.CompareTag("rock_red"))) {
			return true;
		} else {
			return false;
		}
	}

	void Update () {
		if (GameController.instance.GetGameOver ()) {
			rb.velocity = Vector3.zero;
			animator.speed = 0.0f;
			return;
		}

		previous_direction = direction;

		if (reborn.GetReborning() || rock_interaction.GetRemovingOpponentRock ()) {
			animator.speed = 0.0f;
			return;
		}

		if (change_direction_moving) {
			return;
		}


		Vector3 input_direction = GetInputDirection ();
		float input_magnitude = GetInputMagnitude ();

//		Vector3 input_direction = GetInput ();
//		if (input_direction == Vector3.zero) {
//			rb.velocity = Vector3.zero;
//			animator.speed = 0.0f;
//			if (input_device.DPadLeft) {
//				direction = Vector3.left;
//			} else if (input_device.DPadRight) {
//				direction = Vector3.right;
//			} else if (input_device.DPadUp) {
//				direction = Vector3.up;
//			} else if (input_device.DPadDown) {
//				direction = Vector3.down;
//			}
//			return;
//		}

		if (moving) {
			if (previous_direction != input_direction && input_magnitude >= input_magnitude_threshold) {
				float x_dec = transform.position.x - Mathf.Floor (transform.position.x);
				float y_dec = transform.position.y - Mathf.Floor (transform.position.y);
				float change_direction_offset = 0.0f;

				if (x_dec != 0.0f && y_dec != 0.0f) {
					if (x_dec < 0.001f || x_dec > 0.999f) {
						x_dec = 0.0f;
					} else if (y_dec < 0.001 || y_dec > 0.999f) {
						y_dec = 0.0f;
					} else {
						Debug.Log ("Player not on the grid!");
						Debug.Log (x_dec);
						Debug.Log (y_dec);
					}
				}

				if (x_dec != 0.0f && x_dec < change_direction_threshold && previous_direction != Vector3.left) {
					direction = Vector3.left;
					change_direction_offset = x_dec;
				} else if (x_dec > 1.0f - change_direction_threshold && previous_direction != Vector3.right) {
					direction = Vector3.right;
					change_direction_offset = 1.0f - x_dec;
				} else if (y_dec != 0.0f && y_dec < change_direction_threshold && previous_direction != Vector3.down) {
					direction = Vector3.down;
					change_direction_offset = y_dec;
				} else if (y_dec > 1.0f - change_direction_threshold && previous_direction != Vector3.up) {
					direction = Vector3.up;
					change_direction_offset = 1.0f - y_dec;
				}

				if (change_direction_offset != 0.0f) {
					animator.SetFloat ("horizontal", direction.x);
					animator.SetFloat ("vertical", direction.y);
					StartCoroutine (ChangheDirectionCoroutine (change_direction_offset));
				}
			}
		} else {
			if (!moving && input_magnitude != 0) {
				direction = input_direction;
			}

			animator.SetFloat ("horizontal", direction.x);
			animator.SetFloat ("vertical", direction.y);

			if (input_magnitude < input_magnitude_threshold) {
				animator.speed = 0.0f;
				return;
			}

			animator.speed = 1.0f;


			if (CheckObstacle (transform.position + direction)) {
				rb.velocity = Vector3.zero;
				return;
			}

			StartCoroutine (MoveCoroutine ());
		}
	}

	private IEnumerator MoveCoroutine() {
		moving = true;

		if (gem_interaction.GetHolding ()) {
			rb.velocity = holding_movement_speed * direction;
		} else {
			rb.velocity = movement_speed * direction;
		}

		Vector3 origin_velocity = rb.velocity;
		Vector3 origin_position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0.0f);
		Vector3 moving_direction = direction;

		bool blocked = false;
		bool stop = false;
		float moving_time = 1.0f / origin_velocity.magnitude;

		for (float t = 0.0f; t < moving_time; t += Time.deltaTime) {
			rb.velocity = origin_velocity;
			yield return null;
			if (CheckObstacle (origin_position + moving_direction)) {
				blocked = true;
				break;
			} else if (change_direction_moving) {
				stop = true;
				break;
			}
		}
		
		rb.velocity = Vector3.zero;

		if (blocked) {
			transform.position = origin_position;
		} else if (!stop) {
			transform.position = origin_position + moving_direction;
		}

		moving = false;
	}

	private IEnumerator ChangheDirectionCoroutine(float offset) {
		change_direction_moving = true;

		if (gem_interaction.GetHolding ()) {
			rb.velocity = holding_movement_speed * direction;
		} else {
			rb.velocity = movement_speed * direction;
		}

		Vector3 origin_velocity = rb.velocity;
		Vector3 target_position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0.0f);


		for (float t = 0.0f; t < offset / origin_velocity.magnitude; t += Time.deltaTime) {
			rb.velocity = origin_velocity;
			yield return null;
		}

		rb.velocity = Vector3.zero;
		transform.position = target_position;
		change_direction_moving = false;
	}

	public Vector3 GetDirection() {
		return direction;
	}
}
