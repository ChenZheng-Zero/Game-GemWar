using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour {

	private int team_number;
	private string opponent_rock_tag;
	private Vector3 direction = Vector3.zero;
	private GameObject time_bar = null;
	private Object rock_collectable_prefab;
	private Rigidbody rb;
	private Renderer rend;
	private GameObject shot_by = null;
	private bool horizontal_bouncing = false;
	private bool vertical_bouncing = false;

	public float rock_speed = 6.0f;
	public float exist_time = 5.0f;
	public float blink_time = 4.0f;
	public float check_distance = 0.6f;
	public float speed_coefficient = 1.0f;
	public float opponent_rock_removal_duration = 2.0f;
	public AnimationCurve bounce_curve;
	public float curve_speed_coefficient = 2.0f;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		rend = GetComponent<Renderer> ();
		rock_collectable_prefab = Resources.Load ("Prefabs/rock_collectable", typeof(GameObject));

		if (tag == "rock_blue") {
			team_number = 1;
			opponent_rock_tag = "rock_red";
		} else {
			team_number = 2;
			opponent_rock_tag = "rock_blue";
		}
	}

	void Update () {
		if (direction != Vector3.zero) {
			Collider collider = PublicFunctions.instance.FindObjectOnPosition (transform.position + direction * check_distance * speed_coefficient);

			if (collider && CheckStopingTag (collider.tag)) {
				rb.velocity = Vector3.zero;
				transform.position = PublicFunctions.instance.RoundVector3 (transform.position);
				direction = Vector3.zero;

				//Vanishing Code
				//StartCoroutine (BlinkCoroutine ());
			}
		} 
	}

	void OnDestroy() {
		Instantiate (rock_collectable_prefab, transform.position, Quaternion.identity);
	}

	private bool CheckStopingTag(string col_tag) {
		if (col_tag == tag || col_tag == "base_blue" || col_tag == "base_red" || col_tag == "wall" || col_tag == "gem_blue" || col_tag == "gem_red" ||
			col_tag == "reborn_blue" || col_tag == "reborn_red" || team_number == PublicFunctions.instance.GetTeamNumber (col_tag)) {
			return true;
		} else {
			return false;
		}
	}


	void OnTriggerStay(Collider collider) {
		if (team_number + PublicFunctions.instance.GetTeamNumber (collider.tag) == 3) {
			BuffController collider_buff_controller = collider.GetComponent<BuffController> ();
			if (collider_buff_controller.GetGuardian ()) {
				collider_buff_controller.ResetGuardian ();
			} else {
				if (speed_coefficient != 1.0f && collider.GetComponent<GemInteraction> ().GetHolding ()) {
					rb.velocity = Vector3.zero;
					Vector3 round_position = PublicFunctions.instance.RoundVector3 (transform.position);
					if (round_position == PublicFunctions.instance.RoundVector3 (collider.transform.position)) {
						transform.position = round_position - direction;
					} else {
						transform.position = round_position;
					}
					direction = Vector3.zero;
				}

				if (shot_by) {
					shot_by.GetComponent<PlayerDataController> ().AddKill ();
				}
				collider.GetComponent<Reborn> ().StartRebornCoroutine ();
			}

			if (speed_coefficient == 1.0f) {
				Destroy (gameObject);
			}
		} else if (collider.tag == opponent_rock_tag) {
			if (speed_coefficient == 1.0f || direction == Vector3.zero || collider.GetComponent<RockController> ().IfSuperRock ()) {
				Destroy (gameObject);
			}
		} else if (CheckStopingTag(collider.tag)) {
			speed_coefficient = 1.0f;
			direction = Vector3.zero;
			rb.velocity = Vector3.zero;
			transform.position = PublicFunctions.instance.RoundVector3 (transform.position);
			//Vanishing Code
			//StartCoroutine (BlinkCoroutine ());
		}
	}

	public void SetPlayer(GameObject player) {
		shot_by = player;
	}

	public void SetMovingDirection(Vector3 _direction) {
		direction = _direction;
		rb.velocity = direction * rock_speed * speed_coefficient;
		if (speed_coefficient == 1.0f && (direction == Vector3.up || direction == Vector3.down) && !vertical_bouncing) {
			StartCoroutine (VerticalBounceCoroutine ());
		} else if (speed_coefficient == 1.0f && (direction == Vector3.left || direction == Vector3.right) && !horizontal_bouncing) {
			StartCoroutine (HorizontalBounceCoroutine ());
		}
	}

	public void RemoveByPlayer(GameObject player) {
		if (time_bar == null) {
			Object time_bar_prefab = Resources.Load ("Prefabs/time_bar", typeof(GameObject));
			time_bar = (GameObject)Instantiate (time_bar_prefab, transform.position, Quaternion.identity);
		}
		StartCoroutine (RemoveCoroutine (player));
	}

	private IEnumerator RemoveCoroutine(GameObject player) {
		RockInteraction rock_interaction = player.GetComponent<RockInteraction>();

		time_bar.transform.parent = transform;
		GameObject time_left = null;
		GameObject total_time = null;

		foreach (Transform child in time_bar.transform) {
			if (child.CompareTag ("time_left")) {
				time_left = child.gameObject;
			} else if (child.CompareTag ("total_time")) {
				total_time = child.gameObject;
			}
		}

		float total_length = total_time.transform.localScale.x;
		while (time_left.transform.localScale.x > 0.0f) {
			if (!rock_interaction.GetRemovingOpponentRock ()) {
				break;
			}

			time_left.transform.localScale = new Vector3 (
				time_left.transform.localScale.x - total_length * Time.deltaTime / opponent_rock_removal_duration, 
				time_left.transform.localScale.y, 
				time_left.transform.localScale.z
			);

			yield return null;
		}

		if (time_left.transform.localScale.x <= 0.0f) {
			Destroy (gameObject);
		}

	}

	private IEnumerator BlinkCoroutine() {
		yield return new WaitForSeconds(exist_time);

		float end_time = Time.time + blink_time;

		while(Time.time < end_time){
			rend.enabled = true;
			yield return new WaitForSeconds(0.2f);
			rend.enabled = false;
			yield return new WaitForSeconds(0.2f);
		}
		Destroy (gameObject);
	}

	private IEnumerator HorizontalBounceCoroutine() {
		horizontal_bouncing = true;

		float origin_scale_x = transform.localScale.x;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * curve_speed_coefficient) {
			transform.localScale = new Vector3(
				origin_scale_x * bounce_curve.Evaluate (t), 
				transform.localScale.y,
				transform.localScale.z);
			yield return null;
		}

		transform.localScale = new Vector3(
			origin_scale_x, 
			transform.localScale.y,
			transform.localScale.z);

		horizontal_bouncing = false;
	}

	private IEnumerator VerticalBounceCoroutine() {
		vertical_bouncing = true;

		float origin_scale_y = transform.localScale.y;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * curve_speed_coefficient) {
			transform.localScale = new Vector3(
				transform.localScale.x, 
				origin_scale_y * bounce_curve.Evaluate (t),
				transform.localScale.z);
			yield return null;
		}

		transform.localScale = new Vector3(
			transform.localScale.x, 
			origin_scale_y,
			transform.localScale.z);

		vertical_bouncing = false;
	}

	public bool IfSuperRock() {
		if (speed_coefficient == 1.0f) {
			return false;
		} else {
			return true;
		}
	}
}
