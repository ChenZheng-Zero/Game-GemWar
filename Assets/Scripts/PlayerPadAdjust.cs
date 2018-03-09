using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerPadAdjust : MonoBehaviour {

	private InputDevice input_device;
	Vector3 pos;

	void Start () {
		input_device = GetComponent<PlayerControl> ().GetInputDevice ();
		pos = gameObject.transform.position;
	}
	
	void Update () {
		gameObject.transform.position = pos + 0.5f*GetInput ();
	}

	private Vector3 GetInput() {
		float horizontal_input = input_device.LeftStickX;
		float vertical_input = input_device.LeftStickY;

		if (Mathf.Abs(horizontal_input) < 0.1 && Mathf.Abs(vertical_input) < 0.1) {
			horizontal_input = 0.0f;
			vertical_input = 0.0f;
		} 
//		else if (Mathf.Abs(horizontal_input) > Mathf.Abs(vertical_input)) {
//			horizontal_input = 1.0f * Mathf.Sign(horizontal_input);
//			vertical_input = 0.0f;
//		} else {
//			horizontal_input = 0.0f;
//			vertical_input = 1.0f * Mathf.Sign(vertical_input);
//		}

		return new Vector3 (horizontal_input, vertical_input, 0.0f);
	}
}
