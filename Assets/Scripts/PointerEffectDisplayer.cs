using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerEffectDisplayer : MonoBehaviour {

	Color original;

	void Start () {
		original = GetComponent<SpriteRenderer> ().color;
		StartCoroutine (changeColor ());
	}
	
	void Update () {
		
	}

	IEnumerator changeColor(){
		while (true) {
			if (GetComponent<SpriteRenderer> ().color == original) {
				GetComponent<SpriteRenderer> ().color = Color.gray;
			} else {
				GetComponent<SpriteRenderer> ().color = original;
			}
			yield return new WaitForSeconds (.3f);
		}
	}


}
