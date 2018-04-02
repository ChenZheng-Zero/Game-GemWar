using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

	public AudioClip explode;
	public AudioClip putdown;
	public AudioClip push;
	public AudioClip death;
	public AudioClip powerup;
	public AudioClip gemup;
	public AudioClip gemdown;
	public AudioClip score;
	public AudioClip BGM;

	float timestamp = 0f;

	void Start () {
//		Debug.Log (BGM.length.ToString ());
	}
	
	void Update () {
		if (timestamp == 0f) {
			AudioSource.PlayClipAtPoint (BGM, Camera.main.transform.position, 0.2f);
			timestamp += Time.deltaTime;
		} else{
			if (timestamp > BGM.length) {
				timestamp = 0f;
			} else {
				timestamp += Time.deltaTime;
			}
		}
	}

	public void PlayExplodeSound(){
		AudioSource.PlayClipAtPoint (explode, Camera.main.transform.position, 0.1f);

	}

	public void PlayPutdownSound(){
		AudioSource.PlayClipAtPoint (putdown, Camera.main.transform.position, 0.2f);
	}

	public void PlayPushSound(){
		AudioSource.PlayClipAtPoint (push, Camera.main.transform.position, 0.2f);
	}

	public void PlayDeathSound(){
		AudioSource.PlayClipAtPoint (death, Camera.main.transform.position, 5.0f);
	}

	public void PlayPowerupSound(){
		AudioSource.PlayClipAtPoint (powerup, Camera.main.transform.position, 0.2f);
	}

	public void PlayGemupSound(){
		AudioSource.PlayClipAtPoint (gemup, Camera.main.transform.position);
	}

	public void PlayGemdownSound(){
		AudioSource.PlayClipAtPoint (gemdown, Camera.main.transform.position);
	}

	public void PlayScoreSound(){
		AudioSource.PlayClipAtPoint (score, Camera.main.transform.position);
	}
}
