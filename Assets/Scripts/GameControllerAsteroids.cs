using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;

/// <summary>
/// Game controller for asteroids.
/// </summary>
public class GameControllerAsteroids : MonoBehaviour {

	private GameObject spaceship;
	private GameObject buttonContinue;
	private GameObject buttonQuit;
	private ParticleSystem particleLeft;
	private ParticleSystem particleRight;
	private ParticleSystem particleDown;
	private ParticleSystem shipExplosion;
	private Image progressBar; //shows how long asteroids are created
	private Vector3 shipPosition;
	private Text textYouLost; //text shown after you lose
	private float screenEdge; //position of screen edges to block the spaceship from being able to leave the screen
	private float progressTimer; //the time how long the game lasts
	private float currentTime; //this is the timer that counts down from the start to 0 to tell the current time
	private float audioClipStartTime; //the moment when the audio clip starts is saved here
	private int timerParticleLeft; //counts frames (updates of the engine) to control the rocket booster particle effect
	private int timerParticleRight;
	private bool playOnce; //play particle animation only once
	private bool movementSoundIsNotPlaying; //boolean to tell if moving sound is playing or not
	private bool launchSoundPlayed;
	private AudioSource audioSource;
	public AudioClip moveSound; //sound for moment left/right
	public AudioClip moveUpSound; //rocket launch sound when leaving asteroidfield

	/// <summary>
	/// Method ran at the start of the scene.
	/// </summary>
	void Start () {
		this.initializeVariables ();
		particleLeft.Stop (); //make it so that the particles aren't being emitted at the start
		particleRight.Stop ();
		particleDown.Stop ();
		shipExplosion.Stop ();
		buttonContinue.GetComponent<Button>().onClick.AddListener (() => buttonClicked ("continue")); //adds a mouse click listener to the button and sends string continue to the method buttonClicked when the click happens
		buttonQuit.GetComponent<Button>().onClick.AddListener (() => buttonClicked ("quit"));
		buttonContinue.SetActive (false); //hide the ui button by making it non active
		buttonQuit.SetActive (false);
		textYouLost.text = ""; //hide the ui text by making it empty
	}

	/// <summary>
	/// Initializes the variables.
	/// </summary>
	private void initializeVariables() {
		//initialize variables
		movementSoundIsNotPlaying = true;
		launchSoundPlayed = false;
		timerParticleLeft = 0; //counts frames, intialize at 0
		timerParticleRight = 0;
		screenEdge = 8.1f; //the edge of the screen to block the spaceship from going outside of the screen
		progressTimer = Random.Range(15,26); //the time how long the game lasts (from 15 to 25 seconds)
		Debug.Log("Game length: " + progressTimer);
		currentTime = progressTimer; 
		playOnce = false; //play particle animation only once set to false at start

		//initalize unity objects
		spaceship = GameObject.Find ("Spaceship"); 
		progressBar = GameObject.Find ("ProgressBarImage").GetComponent<Image>();
		textYouLost = GameObject.Find ("TextYouLost").GetComponent<Text>();
		buttonContinue = GameObject.Find ("ButtonContinue"); //set as GameObject instead of Button to be able to set it active/inactive easily
		buttonQuit = GameObject.Find ("ButtonQuit");
		particleLeft = GameObject.Find ("ParticleSparksLeft").GetComponent<ParticleSystem>();
		particleRight = GameObject.Find ("ParticleSparksRight").GetComponent<ParticleSystem>();
		particleDown = GameObject.Find ("ParticleSparksDown").GetComponent<ParticleSystem>();
		shipExplosion = GameObject.Find ("ParticleShipExploded").GetComponent<ParticleSystem> ();
		audioSource = FindObjectOfType<AudioSource> (); //finds the audio source that is created in the preload scene and not destroyed on load
	}

	/// <summary>
	/// Update is called once per frame. Controls movement and checks if the game is won/lost.
	/// </summary>
	void Update () {
		if (GameObject.Find ("Spaceship") != null) { //checks if the spaceship exists (doesn't exist if it gets destroyed by asteroids)
			this.movement(); //control the movement of the spaceship
		} else { //spaceship doesn't exist
			this.gameEnds (); //game lost
		}
		if (!playOnce) { //has the explosion animation (that happens when the player's spaceship explodes) been played if yes then the player can't win by the time running out
			this.time (); //updates game time and progress indicator "bar"
		}
		timerParticleLeft++; //+1 every frame to make sure particles are played in intervals that make sense
		timerParticleRight++;
		if (movementSoundIsNotPlaying == false) { //sound is playing
			this.clipPlaying ();
		}
	}

	/// <summary>
	/// Handles the movement control of the spaceship and also particles related to the movement.
	/// </summary>
	private void movement() {
		if (timerParticleLeft > 10) { //if left hasn't been pressed in over 10 frames stops playing the rocket booster particle animation
			particleLeft.Stop ();

		}
		if (timerParticleRight > 10) {
			particleRight.Stop ();

		}

		shipPosition = spaceship.transform.position; //position of the spaceship this frame
		shipExplosion.transform.position = shipPosition; //ship's explosion particle system follows the ship's position
		if (Input.touchCount > 0) { //if touchscreen input happens
			Touch touch = Input.touches [0]; //get the first touch event (if multiple touches happen only read the first)
			if (touch.position.x < Screen.width/2 && shipPosition.x > -screenEdge) { //if left side of the touch screen is pressed and the player is not at the edge of the screen
				this.moveLeft ();
			} else if (touch.position.x >= Screen.width/2 && shipPosition.x < screenEdge) { //right side
				this.moveRight ();
			} else { //nothing is pressed so spaceship is slowed down
				this.deaccelerate ();
			}
		}
		if (Input.GetKey (KeyCode.LeftArrow) && shipPosition.x > -screenEdge) { //if left arrow  is pressed and the player is not at the edge of the screen
			this.moveLeft ();
		} else if (Input.GetKey (KeyCode.RightArrow) && shipPosition.x < screenEdge) { //right arrow
			this.moveRight ();
		} else {
			this.deaccelerate ();
		}

		if (shipPosition.x <= -screenEdge) { //player at the edge
			spaceship.GetComponent<Rigidbody2D> ().velocity = Vector3.zero; //set player's speed to 0
			while (spaceship.transform.position.x <= -screenEdge) { 
				//if the edge is hit with high speed the spaceship goes a slight bit too far "over the edge of the screen" which affects force control for a small moment 
				//so to fix that we move it back inside the edges
				spaceship.transform.Translate (0.005f, 0, 0);
			}
		} else if (shipPosition.x >= screenEdge) {
			spaceship.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			while (spaceship.transform.position.x >= screenEdge) {
				spaceship.transform.Translate (-0.005f, 0, 0);
			}
		}
	}

	/// <summary>
	/// Spaceship movement left.
	/// </summary>
	private void moveLeft() {
		spaceship.GetComponent<Rigidbody2D>().AddForce(new Vector2(-Spaceship.speedOfShip, 0)); //set force that pushes the spaceship to left according to shipspeed
		//Emit particles everytime at least 3 frames have passed to make it sure that there aren't too many particles on the screen
		if (timerParticleLeft > 3) { 
			particleLeft.Emit (3); //emit the rocket booster particles 
			timerParticleLeft = 0; //set the frame counter to 0
		}
		if (movementSoundIsNotPlaying) { //if movement sound is not playing
			audioSource.PlayOneShot (moveSound); //play movement sound
			audioClipStartTime = Time.time; //save the current start time of the movement sound clip to this variable
			movementSoundIsNotPlaying = false; //movement sound is playing
		}
	}

	/// <summary>
	/// Spaceship movement right.
	/// </summary>
	private void moveRight() {
		spaceship.GetComponent<Rigidbody2D>().AddForce(new Vector2(Spaceship.speedOfShip, 0)); //set force that pushes the spaceship to right according to shipspeed
		if (timerParticleRight > 3) {
			particleRight.Emit (3);
			timerParticleRight = 0;
		}
		if (movementSoundIsNotPlaying) {
			audioSource.PlayOneShot (moveSound);
			audioClipStartTime = Time.time; 
			movementSoundIsNotPlaying = false;
		}
	}

	/// <summary>
	/// Deaccelerate the spaceship's movement.
	/// </summary>
	private void deaccelerate() {
		//slow down the spaceship when left or right button aren't clicked
		spaceship.GetComponent<Rigidbody2D>().velocity = spaceship.GetComponent<Rigidbody2D>().velocity - ((spaceship.GetComponent<Rigidbody2D>().velocity * 2f) * .02f); 
	}

	/// <summary>
	/// Check and update if the player has survived through the asteroid field. If the player survives through it play the win animation and change scene.
	/// </summary>
	private void time() {
		if (currentTime > 0) { //if there's still game time left
			currentTime -= Time.deltaTime; //remove from the current time from the time it took to do the last frame
			progressBar.fillAmount = currentTime / progressTimer; //make the image show up a smaller and smaller part of it (a visual represntation of the progress timer going down)
		}
		if (currentTime <= 0) { //if timer runs out (player wins)
			FindObjectOfType<AsteroidController> ().stopCreatingAsteroids(); //stops invoking the method that clones prefab asteroids on the gamefield
			if (GameObject.Find("Asteroid(Clone)") == null) { //check if all asteroids have been destroyed by the asteroid destroyer
				spaceship.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation; //unlocks y-axis lock that is set from unity and only locks rotation z-axis
				spaceship.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 20f)); //make the spaceship flyupwards away from the camera's view
				particleDown.Emit (3); //emit particles downwards from the ship
				if (!launchSoundPlayed) { //only play launch sound once
					audioSource.PlayOneShot (moveUpSound); //play launch sound
					launchSoundPlayed = true;
				}
				if (spaceship.transform.position.y > 12f) { //game won
					if (Spaceship.currentPlanet.Equals ("earth")) { //at earth we don't do anything
						SceneManager.LoadScene ("Starmap");
					} else if (Spaceship.currentPlanet.Equals ("neptune") && Spaceship.speedOfShip < 9f) { //not enough speed to get to neptune
						SceneManager.LoadScene ("InsideSpaceship"); //start the scene outside of neptune
					} else { //every other planet
						StringBuilder planetScene = new StringBuilder (); //new StringBuilder that is used to create the name of the next scene we want to start
						//create a string that is the planet name with first letter capitalized and then Scene
						planetScene.Append (Spaceship.currentPlanet.First ().ToString ().ToUpper () + Spaceship.currentPlanet.Substring (1)).Append ("Scene"); 
						SceneManager.LoadScene (planetScene.ToString ()); //start the scene
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Game ends, play explosion and show buttons.
	/// </summary>
	private void gameEnds() { //the game ended by losing all health
		if (!playOnce) { //has the explosion animation been played?
			shipExplosion.Play (); //play the explosion animation
			playOnce = true;
			textYouLost.text = "You lost"; //write "You lost" to the screen
			buttonContinue.SetActive (true); //set the button the be active and be shown to the player
			buttonQuit.SetActive (true);
		}
	}

	/// <summary>
	/// Game ending button continue or quit clicked.
	/// </summary>
	/// <param name="button">"continue" or "quit" button clicked</param>
	private void buttonClicked(string button) { //method that is ran when a button is clicked
		if (button.Equals("continue")) { //continue clicked
			StarmapGameController.rotationSpeedBlackhole += 50f; //set blackhole to rotate faster
			StarmapGameController.chanceToLoseByGettingSuckedIntoABlackhole += 5; //raises the chance to lose by getting sucked into a blackhole by 5%
			StarmapGameController.blackholeSize = new Vector3 (StarmapGameController.blackholeSize.x + 0.25f, StarmapGameController.blackholeSize.y + 0.25f, 1f); //change the size of the blackhole
			Spaceship.lostLastGame = true; //we lost last game
			SceneManager.LoadScene ("Starmap"); //change scene
		} else { //quit clicked
			Application.Quit(); //quit the game
		}
	}

	/// <summary>
	/// Audio clip of movement sound is playing
	/// </summary>
	private void clipPlaying(){
		if ((Time.time-audioClipStartTime) >= moveSound.length){
			Debug.Log ("Set audio to true");
			movementSoundIsNotPlaying = true;
		}
	}
}

