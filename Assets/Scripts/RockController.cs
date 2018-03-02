using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour {

	private Rigidbody rb;
	private int team_number;
	private string opponent_rock_tag;
	private Vector3 direction = Vector3.zero;
	private GameObject time_bar = null;
	private Object rock_collectable_prefab;

	public float opponent_rock_removal_duration = 2.0f;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		rock_collectable_prefab = Resources.Load ("Prefabs/rock_collectable", typeof(GameObject));

		if (tag == "rock_blue") {
			team_number = 1;
			opponent_rock_tag = "rock_red";
		} else {
			team_number = 2;
			opponent_rock_tag = "rock_blue";
		}
	}

	void OnDestroy() {
		Instantiate (rock_collectable_prefab, transform.position, Quaternion.identity);
	}

	private bool CheckTag(string col_tag) {
		if (col_tag == tag || col_tag == "base_blue" || col_tag == "base_red" || col_tag == "wall" || col_tag == "gem" ||
			col_tag == "reborn_blue" || col_tag == "reborn_red" || team_number == PublicFunctions.instance.GetTeamNumber (col_tag)) {
			return true;
		} else {
			return false;
		}
	}


	void OnTriggerStay(Collider collider) {
//		if (direction != Vector3.zero) {
			if (team_number + PublicFunctions.instance.GetTeamNumber (collider.tag) == 3) {
				collider.GetComponent<Reborn> ().StartRebornCoroutine ();
				Destroy (gameObject);
			} else if (collider.tag == opponent_rock_tag) {
				Destroy (gameObject);
				Destroy (collider.gameObject);
			} else if (CheckTag(collider.tag)) {
				rb.velocity = Vector3.zero;
				transform.position = PublicFunctions.instance.RoundVector3 (transform.position);
				direction = Vector3.zero;
			}
//		}
	}

	public void SetMovingDirection(Vector3 _direction) {
		direction = _direction;
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
}
