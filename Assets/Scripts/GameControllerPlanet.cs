using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerPlanet : MonoBehaviour {

	private float wait = 0.3f;
	private bool end = false;
	private bool nofight = false;
	private bool neptune = false;

	/// <summary>
	/// Starts the fight.
	/// </summary>
	public void StartFight () {
		if (Spaceship.currentPlanet.Equals ("jupiter") || Spaceship.currentPlanet.Equals ("venus")) {
			//No fight so empty traveledToAPlanetButLost
			Spaceship.traveledToAPlanetButLost = " "; 
			//If current planet is Jupiter or Venus, fight does not start
			nofight = true; 
			//Fast enough to get to actual neptune
		} else if (Spaceship.currentPlanet.Equals ("neptune") && Spaceship.speedOfShip > 8) {
			neptune = true; 
			//Not fast enough, stays at the spaceship
		} else if (Spaceship.currentPlanet.Equals ("neptune") && Spaceship.speedOfShip <= 8) {
			//No fight so empty traveledToAPlanetButLost
			Spaceship.traveledToAPlanetButLost = " "; 
			nofight = true;
		}
		end = true;
	}

	/// <summary>
	/// Ran once per frame.
	/// </summary>
	void Update() {
		if (end) {
			//Waiting a moment after the dialogue ends and before fight starts
			wait -= Time.deltaTime; 
		}
		if (wait < 0) {
			if (neptune) {
				//If you can enter Neptune, moves to leaderboard when dialogue ends
				SceneManager.LoadScene ("Leaderboard"); 
			} else if (nofight) {
				//If there is no fight, loading Startmap scene
				SceneManager.LoadScene ("Starmap"); 
			} else {
				//50% chance to go either Minesweeper or Match Three
				int rnd = Random.Range (0, 2); 
				if (rnd == 0) {
					SceneManager.LoadScene ("MineSettings");
				} else if (rnd == 1) {
					SceneManager.LoadScene ("MatchThree");
				}
			}
		}
	}
}

