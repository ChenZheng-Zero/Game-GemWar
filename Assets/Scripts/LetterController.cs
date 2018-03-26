using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LetterController : MonoBehaviour {

	GameObject main_text;
	GameObject video_player;
	GameObject[] icons;
	bool[] confirmed;

	GameObject[] players;
	string[] texts;
	VideoClip[] clips;
	int index = 0;
	bool[] pressed;


	void Awake () {
		main_text = transform.Find ("Canvas/Text").gameObject;
		video_player = transform.Find ("Canvas/Video Player").gameObject;

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
				if (clips [index]) {
					TurnOnVideo (true);
					video_player.GetComponent<VideoPlayer> ().clip = clips [index];
					SetPlaceHolder (clips [index].width, clips [index].height);
				} else {
					TurnOnVideo (false);
				}
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

	public void SetVideo(VideoClip[] _clips){
		clips = _clips;
		if (clips [0]) {
			video_player.GetComponent<VideoPlayer> ().clip = clips [0];
			SetPlaceHolder (clips [0].width, clips [0].height);
			TurnOnVideo (true);
		} else {
			TurnOnVideo (false);
		}
	}

	bool AllConfirmed(){
		for (int i = 0; i < 4; ++i) {
			if (!confirmed [i])
				return false;
		}
		return true;
	}

	void TurnOnVideo(bool on){
		transform.Find ("video_placeholder").GetComponent<SpriteRenderer> ().enabled = on;
	}

	void SetPlaceHolder(float width, float height){
		float ratio = width / height;
		transform.Find ("video_placeholder").transform.localScale = new Vector3 (1.3f * ratio, 1.3f, 1f);
	}
}
