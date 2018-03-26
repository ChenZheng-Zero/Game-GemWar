using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBarDisplayer : MonoBehaviour {

	private int rock_count = 0;
//	private float length_per_rock;
//	private GameObject rock_left_bar = null;
	private List<SpriteRenderer> rocks = new List<SpriteRenderer>();
	private Sprite normal_rock_sprite;
	private Sprite super_rock_sprite;

	public int max_rock = 3;

	void Start () {
		normal_rock_sprite = Resources.Load<Sprite> ("Sprites/" + GetComponent<PlayerControl> ().GetOwnColor ());
		super_rock_sprite = Resources.Load<Sprite> ("Sprites/orange");

		Transform rock_bar = null;
		foreach (Transform child in transform) {
			if (child.name == "rock_bar") {
				rock_bar = child;
			}
		}

		foreach (Transform child in rock_bar) {
//			if (child.name == "rock_left") {
//				rock_left_bar = child.gameObject;
//			} else if (child.name == "background") {
//				length_per_rock = child.localScale.x / max_rock;
//			}

			if (child.name == "rock") {
				rocks.Add (child.GetComponent<SpriteRenderer> ());
			}
		}
	}
	
	void Update () {
//		rock_left_bar.transform.localScale = new Vector3 (
//			rock_count * length_per_rock,
//			rock_left_bar.transform.localScale.y,
//			rock_left_bar.transform.localScale.z
//		);
	}

	public void AddRock() {
		if (IfRockCountMax ()) {
			Debug.Log ("Something wrong happens");
			return;
		}

		rocks [rock_count].sprite = normal_rock_sprite;
		rocks [rock_count].enabled = true;
		++rock_count;
	}

	public void UseRock() {
		if (IfRockCountZero ()) {
			Debug.Log ("Something wrong happens");
			return;
		}

		--rock_count;
		for (int i = 0; i < rock_count; ++i) {
			rocks [i].sprite = rocks [i + 1].sprite;
		}
		rocks [rock_count].enabled = false;
	}

	public int GetRockCount() {
		return rock_count;
	}

	public bool IfNextSuperRock() {
		if (rock_count == 0) {
			Debug.Log ("Something wrong happens");
			return false;
		}

		if (rocks [0].sprite == super_rock_sprite) {
			return true;
		} else {
			return false;
		}
	}

	public void SuperRockBuff() {
		for (int i = 0; i < rock_count; ++i) {
			rocks [i].sprite = super_rock_sprite;
		}
	}

	public bool IfRockCountZero() {
		return rock_count == 0;
	}

	public bool IfRockCountMax() {
		return rock_count == max_rock;
	}
}
