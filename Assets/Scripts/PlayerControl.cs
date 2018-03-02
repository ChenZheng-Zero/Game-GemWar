using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerControl : MonoBehaviour {

	private bool player_out = false;
	private string own_color;
	private string opponent_color;
	private InputDevice input_device;

	void Awake () {
		int player_number = tag[6] - '0';
		int team_number = (player_number + 1) / 2;
//		int team_number = player_number;

		if (team_number == 1) {
			own_color = "blue";
			opponent_color = "red";
		} else {
			own_color = "red";
			opponent_color = "blue";
		}
		input_device = InputManager.Devices [player_number - 1];
	}

	void Update () {
		
	}

	public string GetOwnColor() {
		return own_color;
	}

	public string GetOpponentColor() {
		return opponent_color;
	}

	public InputDevice GetInputDevice() {
		return input_device;
	}

	public void PlayerOut() {
		player_out = true;
	}
}
