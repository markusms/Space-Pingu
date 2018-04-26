using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that saves spaceship variables between scenes and also checks if the player clicks ESC and closes the game when that happens.
/// </summary>
public class Spaceship : MonoBehaviour {

	public static string currentPlanet = "earth"; //the current planet we are at
	public static string previousPlanet = "earth"; //the planet where we were
	public static string traveledToAPlanetButLost = " "; //The planet you try to go to but a lose a fight at
	//Spaceship speed variable that is public static so that it can be accessed from other scenes (used to check if the player's spaceship is good enough to get to saturn and to move in asteroid game)
	public static float speedOfShip = 5f; 
	//Checks if the player lost the last game and forces him to go backwards if true
	public static bool lostLastGame = false; 

	/// <summary>
	/// When the gameobject is created.
	/// </summary>
	void Awake() {
		//Make it so that this script is available in every scene by not destroying the gameobject when a new scene is loaded
		DontDestroyOnLoad (transform.gameObject); 
	}

	/// <summary>
	/// Called once per frame. Checks if ESC is pressed.
	/// </summary>
	void Update() {
		//This gameobject is not destroyed so ESC will close the game in every situation
		if (Input.GetKeyDown(KeyCode.Escape)) { 
			Application.Quit();
		}
	}
}

