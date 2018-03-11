using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSensorController : MonoBehaviour {

	int id;
	bool is_stand_sensor;

	public bool is_standing;

	void Start () {
		is_standing = false;
	}
	
	void Update () {
		
	}

	public void SetProperty(int _id, bool _is_stand_sensor){
		id = _id;
		is_stand_sensor = _is_stand_sensor;
	}

	void OnTriggerEnter(Collider other){
		if (is_stand_sensor) {
			if (other.CompareTag ("player" + (id + 1).ToString ())) {
				is_standing = true;
			}
		} else {
			if (other.CompareTag ("rock_red") || other.CompareTag ("rock_blue")) {
				is_standing = true;
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (is_stand_sensor) {
			if (other.CompareTag ("player" + (id + 1).ToString ())) {
				is_standing = false;
			}
		} else {
			if (other.CompareTag ("rock_red") || other.CompareTag ("rock_blue")) {
				is_standing = false;
			}
		}
	}

	public bool IsStanding(){
		return is_standing;
	}
}
