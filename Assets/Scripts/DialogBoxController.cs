using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour {

	Text text_field;

	void Awake () {
		text_field = transform.Find ("Canvas/Text").GetComponent<Text> ();
		text_field.text = "?";
	}
	
	void Update () {
		
	}

	public void EditText(string text){
		text_field.text = text;
	}

	public void ChangePosition(Vector3 pos){
		gameObject.transform.position = pos;
	}
}
