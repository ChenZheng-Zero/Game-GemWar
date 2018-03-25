using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour {

	private BuffController buff_controller;
	private RockBarDisplayer rock_bar_displayer;

	void Start () {
		buff_controller = GetComponent<BuffController> ();
		rock_bar_displayer = GetComponent<RockBarDisplayer> ();
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag ("rock_collectable") && !rock_bar_displayer.IfRockCountMax ()) {
			rock_bar_displayer.AddRock ();
			Destroy (collider.gameObject);
		} else if (collider.CompareTag ("speed_up_buff")) {
			buff_controller.SpeedUp ();
			Destroy (collider.gameObject);
		} else if (collider.CompareTag ("guardian_buff")) {
			buff_controller.Guardian ();
			Destroy (collider.gameObject);
		} else if (collider.CompareTag ("super_rock_buff")) {
			buff_controller.SuperRock ();
			Destroy (collider.gameObject);
		}
	}
}
