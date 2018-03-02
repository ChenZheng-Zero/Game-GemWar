using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour {

	private RockBarDisplayer rock_bar_displayer;

	void Start () {
		rock_bar_displayer = GetComponent<RockBarDisplayer> ();
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag ("rock_collectable") && !rock_bar_displayer.IfRockCountMax ()) {
			rock_bar_displayer.ModifyRockCount (1);
			Destroy (collider.gameObject);
		}
	}
}
