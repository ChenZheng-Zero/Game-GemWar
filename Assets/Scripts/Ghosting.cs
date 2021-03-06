﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosting : MonoBehaviour {

	private Reborn reborn;
	private GameObject ghosting;
	private BuffController buff_controller;

	void Start () {
		reborn = GetComponent<Reborn> ();
		buff_controller = GetComponent<BuffController> ();

		foreach (Transform child in transform) {
			if (child.name == "ghosting") {
				ghosting = child.gameObject;
				break;
			}
		}
	}
	
	void Update () {
		if (reborn.GetReborning() || buff_controller.GetSpeedCoefficient () == 1.0f) {
			ghosting.SetActive (false);
		} else if (buff_controller.GetSpeedCoefficient () != 1.0f) {
			ghosting.SetActive (true);
		}
	}
}
