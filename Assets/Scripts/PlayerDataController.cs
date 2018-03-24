using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController : MonoBehaviour {

	private int kill = 0;
	private int death = 0;
	private int shoot = 0;
	private int score = 0;
	private int gem_pick_up = 0;

	// Set functions

	public void AddKill() {
		++kill;
	}

	public void AddDeath() {
		++death;
	}

	public void AddShoot() {
		++shoot;
	}

	public void AddScore() {
		++score;
	}

	public void AddGemPickUp() {
		++gem_pick_up;
	}

	// Get functions

	public int GetKill() {
		return kill;
	}

	public int GetDeath() {
		return death;
	}

	public int GetShoot() {
		return shoot;
	}

	public int GetScore() {
		return score;
	}

	public int GetGemPickUp() {
		return gem_pick_up;
	}
}
