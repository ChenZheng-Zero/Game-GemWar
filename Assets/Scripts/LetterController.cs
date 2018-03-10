using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterController : MonoBehaviour {

	GameObject main_text;
	GameObject[] icons;
	bool[] confirmed;

	GameObject[] players;
	string[] texts;
	int index = 0;
	bool[] pressed;


	void Awake () {
		main_text = transform.Find ("Canvas/Text").gameObject;
		players = new GameObject[4];
		icons = new GameObject[4];
		confirmed = new bool[4];
		pressed = new bool[4];
		for (int i = 0; i < 4; ++i) {
			icons [i] = transform.Find ("icon_" + (i + 1).ToString ()).gameObject;
			icons [i].GetComponent<SpriteRenderer> ().enabled = false;
			players [i] = GameObject.Find ("Players/player" + (i + 1).ToString ());
			confirmed [i] = false;
			pressed [i] = false;
		}
	}

	void Update () {
		if (AllConfirmed ()) {
			index += 1;
			if (index < texts.Length) {
				main_text.GetComponent<Text> ().text = texts [index];
				for (int i = 0; i < 4; ++i) {
					confirmed [i] = false;
					icons [i].GetComponent<SpriteRenderer> ().enabled = false;
				}
			} else {
				Destroy (gameObject);
			}
		} else {
			for (int i = 0; i < 4; ++i) {
				if (!confirmed [i] && players [i].GetComponent<PlayerControl> ().GetInputDevice ().Action4 && !pressed[i]) {
					confirmed [i] = true;
					pressed [i] = true;
					icons [i].GetComponent<SpriteRenderer> ().enabled = true;
				}
			}
		}
		for (int i = 0; i < 4; ++i) {
			if (pressed [i] && !players [i].GetComponent<PlayerControl> ().GetInputDevice ().Action4) {
				pressed [i] = false;
			}
		}
	}

	public void SetText(string[] text){
		texts = text;
		main_text.GetComponent<Text> ().text = text [0];
	}

	bool AllConfirmed(){
		for (int i = 0; i < 4; ++i) {
			if (!confirmed [i])
				return false;
		}
		return true;
	}

}
