using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCreater : MonoBehaviour {

	private Object speed_up_prefab;
	private Object guardian_prefab;
	private Object super_rock_prefab;
	private GameObject buff = null;

	public float buff_creation_interval = 15.0f;

	// Use this for initialization
	void Start () {
		speed_up_prefab = Resources.Load ("Prefabs/speed_up_buff", typeof(GameObject));
		guardian_prefab = Resources.Load ("Prefabs/guardian_buff", typeof(GameObject));
		super_rock_prefab = Resources.Load ("Prefabs/super_rock_buff", typeof(GameObject));

		StartCoroutine (BuffCreateCoroutine ());
	}
	
	private IEnumerator BuffCreateCoroutine() {
		while (!GameController.instance.GetGameOver ()) {
			yield return new WaitForSeconds (buff_creation_interval);

			if (buff) {
				Destroy (buff);
			}

			float rand = Random.Range (0.0f, 1.0f);
			if (rand < 0.33f) {
				buff = (GameObject)Instantiate (speed_up_prefab, transform.position, Quaternion.identity);
			} else if (rand < 0.66f) {
				buff = (GameObject)Instantiate (guardian_prefab, transform.position, Quaternion.identity);
			} else {
				buff = (GameObject)Instantiate (super_rock_prefab, transform.position, Quaternion.identity);
			}
		}
	}
}
