using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour {

	public GameObject dead_prefab;
	private float reborn_duration;

	// Use this for initialization
	void Start () {
		reborn_duration = gameObject.GetComponent<Reborn> ().reborn_duration;
	}

	public void StartDeadCoroutine(){
		StartCoroutine (DeadCoroutine ());
	}

	private IEnumerator DeadCoroutine(){
		Debug.Log ("Dead Coroutine");
		GameObject dead_object = Instantiate (dead_prefab, transform.position, Quaternion.identity);
		for (float time = 0; time <= reborn_duration / 2; time += Time.deltaTime) {
			dead_object.transform.position = dead_object.transform.position + new Vector3 (0f, 0.1f, 0f);
			Color col = dead_object.GetComponent<SpriteRenderer> ().color;
			if (col.a >= 0) {
				col.a = col.a - 0.05f;
				dead_object.GetComponent<SpriteRenderer> ().color = col;
			}
			yield return null;
		}
		Destroy (dead_object);
		yield return null;
	}
}
