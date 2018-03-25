using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaController : MonoBehaviour {


	float time = 0.0f;

	void Start () {
		
	}
	
	void Update () {
		time += Time.deltaTime;
		if (time > 2.0f) {
			Destroy (gameObject);
		}
	}
}
