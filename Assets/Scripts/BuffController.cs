﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour {

	private int speed_up = 0;
//	private int guardian = 0;
	private int super_rock = 0;
	private GameObject shield = null;
	private RockBarDisplayer rock_bar_displayer;

	public float speed_up_duration = 5.0f;
	public float speed_up_ratio = 2.0f;
	public float guardian_duration = 5.0f;
	public float super_rock_duration = 5.0f;
	public float super_rock_speed_ratio = 1.5f;

	void Start () {
		rock_bar_displayer = GetComponent<RockBarDisplayer> ();

		foreach (Transform child in transform) {
			if (child.name == "shield") {
				shield = child.gameObject;
				break;
			}
		}
	}

	// Speed Up

	private IEnumerator SpeedUpCoroutine() {
		speed_up += 1;
		yield return new WaitForSeconds (speed_up_duration);
		speed_up -= 1;
	}

	public void SpeedUp() {
		StartCoroutine (SpeedUpCoroutine ());
	}

	public float GetSpeedCoefficient() {
		if (speed_up > 0) {
			return speed_up_ratio;
		} else {
			return 1.0f;
		}
	}

	// Guardian

//	private IEnumerator GuardianCoroutine() {
//		guardian += 1;
//
//		for (float t = 0.0f; t < guardian_duration; t += Time.deltaTime) {
//			if (!shield) {
//				break;
//			}
//			yield return null;
//		}
//
//		if (shield) {
//			guardian -= 1;
//			if (guardian == 0) {
//				ResetGuardian ();
//			}
//		}
//	}

	public void Guardian() {
		if (!shield.activeSelf) {
			shield.SetActive (true);
			shield.GetComponent<ParticleSystem> ().Emit (1);
		}
	}

	public bool GetGuardian() {
		if (shield.activeSelf) {
			return true;
		} else {
			return false;
		}
	}

	public void ResetGuardian() {
		if (shield) {
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
