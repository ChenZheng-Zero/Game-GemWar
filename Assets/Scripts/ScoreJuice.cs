using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreJuice : MonoBehaviour {

	private ParticleSystem ps = null;

	void Start () {
		foreach (Transform child in transform) {
			if (child.name == "star_burst") {
				ps = child.GetComponent<ParticleSystem> ();
				break;
			}
		}
	}
	
	public void StarBurst() {
		ps.Emit (10);
	}
}
