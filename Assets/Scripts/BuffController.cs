using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour {

	private int speed_up = 0;
//	private int guardian = 0;
	private int super_rock = 0;
	private GameObject shield = null;
	private GameObject temporary_shield = null;
	private RockBarDisplayer rock_bar_displayer;

	public float speed_up_duration = 5.0f;
	public float reborn_speed_up_duration = 3.0f;
	public float speed_up_ratio = 2.0f;
	public float temporary_guardian_duration = 3.0f;
	public float super_rock_duration = 5.0f;
	public float super_rock_speed_ratio = 1.5f;

	void Start () {
		rock_bar_displayer = GetComponent<RockBarDisplayer> ();

		foreach (Transform child in transform) {
			if (child.name == "shield") {
				shield = child.gameObject;
			} else if (child.name == "temporary_shield") {
				temporary_shield = child.gameObject;
			}
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) {
			StartCoroutine (TemporaryGuardianCoroutine ());
		}
	}

	// Speed Up

	private IEnumerator SpeedUpCoroutine(float duration) {
		speed_up += 1;
		yield return new WaitForSeconds (duration);
		speed_up -= 1;
	}

	public void SpeedUp() {
		StartCoroutine (SpeedUpCoroutine (speed_up_duration));
	}

	public void RebornSpeedUp() {
		StartCoroutine (SpeedUpCoroutine (reborn_speed_up_duration));
	}

	public float GetSpeedCoefficient() {
		if (speed_up > 0) {
			return speed_up_ratio;
		} else {
			return 1.0f;
		}
	}

	// Guardian

	private IEnumerator TemporaryGuardianCoroutine() {
		temporary_shield.SetActive (true);
		ParticleSystem ps = temporary_shield.GetComponent<ParticleSystem> ();
		ps.Emit (1);

		yield return new WaitForSeconds (temporary_guardian_duration / 2);

		for (float t = 0.0f; t < temporary_guardian_duration / 2; t += 0.3f) {
			if (!temporary_shield.activeSelf) {
				break;
			}
			ps.Stop ();
			yield return new WaitForSeconds (0.15f);

			if (!temporary_shield.activeSelf) {
				break;
			}
			ps.Play ();
			ps.Emit (1);
			yield return new WaitForSeconds (0.15f);
		}

		temporary_shield.SetActive (false);
	}

	public void Guardian() {
		if (temporary_shield.activeSelf) {
			temporary_shield.SetActive (false);
		} 

		if (!shield.activeSelf) {
			shield.SetActive (true);
			shield.GetComponent<ParticleSystem> ().Emit (1);
		}
	}

	public void RebornGuardian() {
		StartCoroutine (TemporaryGuardianCoroutine ());
	}

	public bool GetGuardian() {
		if (shield.activeSelf || temporary_shield.activeSelf) {
			return true;
		} else {
			return false;
		}
	}

	public void ResetGuardian() {
		if (temporary_shield.activeSelf) {
			temporary_shield.SetActive (false);
		} else {
			shield.SetActive (false);
		}
//		guardian = 0;
	}

	// Super Rock

	private IEnumerator SuperRockCoroutine() {
		super_rock += 1;
		yield return new WaitForSeconds (super_rock_duration);
		super_rock -= 1;
	}

	public void SuperRock() {
//		StartCoroutine (SuperRockCoroutine ());
		rock_bar_displayer.SuperRockBuff ();
	}

	public float GetRockSpeedCoefficient() {
		if (super_rock > 0) {
			return super_rock_speed_ratio;
		} else {
			return 1.0f;
		}
	}
}
