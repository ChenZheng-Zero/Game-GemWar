using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour {

	Text dialog_text;

	void Start () {
		dialog_text = GetComponentInChildren<Text> ();
		dialog_text.text = "?";
	}
	
	void Update () {
		
	}

	public void EditText(string text){
		dialog_text.text = text;
	}

	public void ChangePosition(Vector3 pos){
		gameObject.transform.position = pos;
	}
}
