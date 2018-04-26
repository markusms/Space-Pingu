using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Main Menu game controller
/// </summary>
public class GameController : MonoBehaviour {
	private Button optionsButton;
	private Button playButton;
	private Button moreButton;
	private Button quitButton;

	/// <summary>
	/// Ran at the start of this instance.
	/// </summary>
	void Start () {
		optionsButton = GameObject.Find ("ButtonOptions").GetComponent<Button> (); //Find the GameObject button and it's component Button
		playButton = GameObject.Find ("ButtonPlay").GetComponent<Button> ();
		moreButton = GameObject.Find ("ButtonMore").GetComponent<Button> ();
		quitButton = GameObject.Find ("ButtonQuit").GetComponent<Button> ();
		playButton.onClick.AddListener (() => playClicked ()); //When a playButton is clicked goto method playClicked
		optionsButton.onClick.AddListener (() => optionsClicked ());
		moreButton.onClick.AddListener (() => moreClicked ());
		quitButton.onClick.AddListener (() => quitClicked ());
	
		//initialize variables that do not get reset if the player loses and chooses "try again"
		StarmapGameController.blackholeSize = new Vector3(0.5f, 0.5f, 1f); 
		StarmapGameController.rotationSpeedBlackhole = 10f;
		StarmapGameController.chanceToLoseByGettingSuckedIntoABlackhole = 1;
		Spaceship.currentPlanet = "earth";
		Spaceship.previousPlanet = "earth";
		Spaceship.traveledToAPlanetButLost = " ";
		Spaceship.lostLastGame = false;
		Spaceship.speedOfShip = 5f;
	}

	/// <summary>
	/// Play button clicked. Starts "Earthstart" scene.
	/// </summary>
	private void playClicked(){
		SceneManager.LoadScene ("Earthstart");
	}

	/// <summary>
	/// Options button clicked. Starts "OptionsMenu" scene.
	/// </summary>
	private void optionsClicked(){
		SceneManager.LoadScene ("OptionsMenu");		
	}

	/// <summary>
	/// More button clicked. Starts "MorePage" scene.
	/// </summary>
	private void moreClicked(){
		SceneManager.LoadScene ("MorePage");
	}

	/// <summary>
	/// Quit button clicked. Closes the program.
	/// </summary>
	private void quitClicked(){
		Application.Quit (); //quit the game
	}
}