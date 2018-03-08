using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GridBaseMovement : MonoBehaviour {

	private bool moving = false;
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

		animator.SetFloat ("horizontal", direction.x);
		animator.SetFloat ("vertical", direction.y);

		if (moving) {
			return;
		}

		if (reborn.GetReborning() || rock_interaction.GetRemovingOpponentRock ()) {
			animator.speed = 0.0f;
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


		previous_direction = direction;
		if (input_magnitude != 0) {
			direction = input_direction;
		}

		if (input_magnitude < input_magnitude_threshold) {
			animator.speed = 0.0f;
			return;
		}

		animator.speed = 1.0f;


		if (CheckObstacle (transform.position + direction)) {
			rb.velocity = Vector3.zero;
			return;
		}

		if (gem_interaction.GetHolding ()) {
			rb.velocity = holding_movement_speed * direction;
		} else {
			rb.velocity = movement_speed * direction;
		}

		StartCoroutine (MoveCoroutine ());
	}

	private IEnumerator MoveCoroutine() {
		moving = true;
		float origin_speed = rb.velocity.magnitude;
		Vector3 origin_position = transform.position;
		Vector3 moving_direction = direction;

//		bool cancel = false;
//		if (moving_direction != previous_direction) {
//			for (float t = 0.0f; t < 0.1f; t += Time.deltaTime) {
//				if (GetInput () != moving_direction) {
//					cancel = true;
//					break;
//				}
//				yield return null;
//			}
//		}
//
//		if (cancel) {
//			Debug.Log ("cancel");
//			transform.position = origin_position;
//		} else {
			bool blocked = false;
			float moving_time = 1.0f / origin_speed;
//			if (moving_direction != previous_direction) {
//				moving_time -= 0.1f;
//			}

			for (float t = 0.0f; t < moving_time; t += Time.deltaTime) {
				yield return null;
				if (CheckObstacle (origin_position + direction)) {
					blocked = true;
					break;
				}
			}
			
			if (blocked) {
				transform.position = origin_position;
			} else {
				transform.position = origin_position + direction;
			}
//		}

		rb.velocity = Vector3.zero;
		moving = false;
	}

	public Vector3 GetDirection() {
		return direction;
	}
}
