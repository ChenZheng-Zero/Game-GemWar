using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

	public float shake_interval = 1.0f;
	public float still_time = 1.0f;
	public float max_rotation = 15.0f;
	public AnimationCurve shake_curve;

	void Start () {
		StartCoroutine (ShakeCoroutine ());
	}

	private IEnumerator ShakeCoroutine() {
		while (true) {
			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / shake_interval) {
				transform.localRotation = Quaternion.Euler (Vector3.forward * shake_curve.Evaluate (t) * max_rotation);
				yield return null;
			}
			transform.localRotation = Quaternion.Euler (Vector3.zero);
			yield return new WaitForSeconds (still_time);
		}
	}
}
