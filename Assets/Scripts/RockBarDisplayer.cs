using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBarDisplayer : MonoBehaviour {

	private int rock_count = 0;
	private float length_per_rock;
	private GameObject rock_left_bar = null;

	public int max_rock = 3;

	void Start () {
		Transform rock_bar = null;
		foreach (Transform child in transform) {
			if (child.name == "rock_bar") {
				rock_bar = child;
			}
		}

		foreach (Transform child in rock_bar) {
			if (child.name == "rock_left") {
				rock_left_bar = child.gameObject;
			} else if (child.name == "background") {
				length_per_rock = child.localScale.x / max_rock;
			}
		}
	}
	
	void Update () {
		rock_left_bar.transform.localScale = new Vector3 (
			rock_count * length_per_rock,
			rock_left_bar.transform.localScale.y,
			rock_left_bar.transform.localScale.z
		);
	}

	public void ModifyRockCount(int val) {
		rock_count += val;
	}

	public bool IfRockCountZero() {
		return rock_count == 0;
	}

	public bool IfRockCountMax() {
		return rock_count == max_rock;
	}
}
