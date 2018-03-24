using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicFunctions : MonoBehaviour {

	public static PublicFunctions instance;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this);
		} else {
			instance = this;
		}
	}
	
	public Collider FindObjectOnPosition(Vector3 pos) {
		pos.z += 1;
		RaycastHit hit;
		if (Physics.Raycast(pos, Vector3.back, out hit, 2)){
			return hit.collider;
		} else {
			return null;
		}
	}

	public int GetTeamNumber(string tag) {
		if (tag == "player1" || tag == "player2" || tag == "player3" || tag == "player4") {
			return (tag [6] - '0' + 1) / 2;
		} else {
			return -1;
		}
	}

	public Vector3 RoundVector3(Vector3 v) {
		return new Vector3 (
			Mathf.Round(v.x),
			Mathf.Round(v.y),
			Mathf.Round(v.z)
		);
	}
}
