using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosting : MonoBehaviour {

	private bool actived = false;
	private Reborn reborn;
	private ParticleSystem ps;
	private BuffController buff_controller;

	void Start () {
		reborn = GetComponent<Reborn> ();
		ps = GetComponent<ParticleSystem> ();
		buff_controller = GetComponent<BuffController> ();

		ps.Stop ();
	}
	
	void Update () {
		if (buff_controller.GetSpeedCoefficient () != 1.0f && !actived) {
			actived = true;
			ps.Play ();
		} else if ((reborn.GetReborning() || buff_controller.GetSpeedCoefficient () == 1.0f) && actived) {
			actived = false;
			ps.Stop ();
		}
	}
}
