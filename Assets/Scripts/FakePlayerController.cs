using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayerController : MonoBehaviour {

	bool is_alive;

	void Awake () {
		is_alive = true;
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("rock_red") || other.CompareTag ("rock_blue")) {
			is_alive = false;
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}

	public bool IsAlive(){
		return is_alive;
	}
}
