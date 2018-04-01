using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreJuice : MonoBehaviour {

	private ParticleSystem ps = null;

	public float max_bounce_offset = 0.3f;
	public float bounce_speed = 1.0f;
	public AnimationCurve bounce_curve;

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

	public void StartBounceCoroutine() {
		StartCoroutine (BounceCoroutine ());
	}

	private IEnumerator BounceCoroutine() {
		Vector3 origin_position = transform.position;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * bounce_speed) {
			transform.position = origin_position + Vector3.up * bounce_curve.Evaluate (t) * max_bounce_offset;
			yield return null;
		}
	}
}
