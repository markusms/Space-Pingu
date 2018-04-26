using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game controller blackhole
/// </summary>
public class GameControllerBlackhole : MonoBehaviour {

	private float rotationBlackhole; //value of the rotation
	private float rotationSpeedSpaceship;
	private GameObject spaceBackground;
	private GameObject spaceship;
	private GameObject buttonContinue;
	private GameObject buttonQuit;
	private Text textLost;
	private Vector3 shipSize; //vector for shipsize

	/// <summary>
	/// Ran at the start of this instance. Intializes variables.
	/// </summary>
	void Start () {
		rotationSpeedSpaceship = 20f;
		spaceBackground = GameObject.Find ("Blackhole");
		spaceship = GameObject.Find ("Spaceship");
		textLost = GameObject.Find ("TextLost").GetComponent<Text>();
		buttonContinue = GameObject.Find ("ButtonContinue"); //set as GameObject instead of Button to be able to set it active/inactive easily
		buttonQuit = GameObject.Find ("ButtonQuit");
		//adds a mouse click listener to the button and sends string continue to the method buttonClicked when the click happens
		buttonContinue.GetComponent<Button>().onClick.AddListener (() => gameLostButtonClicked ("continue")); 
		buttonQuit.GetComponent<Button>().onClick.AddListener (() => gameLostButtonClicked ("quit"));
		buttonContinue.SetActive (false); //hide the ui button by making it non active
		buttonQuit.SetActive (false);
		textLost.text = ""; //hide the ui text by making it empty
		shipSize = new Vector3(0.1f, 0.1f, 1f); //initial shipsize
	}
	
	/// <summary>
	/// Called once per frame. Rotates the blakchole and moves and rotates the player's spaceship into the blackhole.
	/// </summary>
	void Update () {
		rotationBlackhole += Time.deltaTime * 750f; //change the rotation (variable float) of the blackhole depending on the time
		spaceBackground.transform.rotation = Quaternion.Euler(0,0,-rotationBlackhole); //actually change the rotation of the blackhole here
		spaceship.transform.RotateAround(Vector3.zero, new Vector3(0,0,-3f), rotationSpeedSpaceship * Time.deltaTime); //rotate spaceship around the blackhole
		rotationSpeedSpaceship += 1f;
		spaceship.transform.Translate(new Vector3(+0.007f, +0.007f, 0)); //move the spaceship closer to the blackhole
		shipSize = new Vector3 (shipSize.x - 0.0002f, shipSize.y - 0.0002f, 1f); //make the size of ship getting smaller and smaller when it gets sucked into the blackhole
		spaceship.transform.localScale = shipSize; //set the size of the spaceship to what the vector3 shipSize is

		//compare the absolute distance of spaceship and blackhole and when it's close enough lose the game
		if ((Mathf.Abs(spaceship.transform.position.x) - Mathf.Abs(spaceBackground.transform.position.x)) <= 0.01 && (Mathf.Abs(spaceship.transform.position.y) - Mathf.Abs(spaceBackground.transform.position.y)) <= 0.01) {
			spaceship.SetActive (false);
			textLost.text = "You were never seen again..."; //write "A mine was triggered :(" to the screen
			buttonContinue.SetActive (true); //set the button the be active and be shown to the player
			buttonQuit.SetActive (true);
		}
	}
		
	/// <summary>
	/// Game lost and "continue" or "quit" button was clicked.
	/// </summary>
	/// <param name="button">"continue" or "quit" button clicked</param>
	private void gameLostButtonClicked(string button) { //a method that is ran when a button is clicked
		if (button.Equals("continue")) {
			SceneManager.LoadScene ("MainMenu"); //go back to starmap
		} else {
			Application.Quit(); //quit the game
		}
	}
}
