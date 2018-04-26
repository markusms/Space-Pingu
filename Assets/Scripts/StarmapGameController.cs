using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;

/// <summary>
/// Starmap game controller.
/// </summary>
public class StarmapGameController : MonoBehaviour {
	
	private Planet mercury;
	private Planet venus;
	private Planet earth;
	private Planet mars;
	private Planet jupiter;
	private Planet saturn;
	private Planet uranus;
	private Planet neptune;
	private List<Planet> planets; //list of all the planets
	private RawImage rawImageSpaceship;
	private RawImage rawImageBlackhole;
	private RawImage rawImageMercuryBorder;
	private RawImage rawImageVenusBorder;
	private RawImage rawImageEarthBorder;
	private RawImage rawImageMarsBorder;
	private RawImage rawImageJupiterBorder;
	private RawImage rawImageSaturnBorder;
	private RawImage rawImageUranusBorder;
	private RawImage rawImageNeptuneBorder;
	private Button buttonMercury;
	private Button buttonVenus;
	private Button buttonEarth;
	private Button buttonMars;
	private Button buttonJupiter;
	private Button buttonSaturn;
	private Button buttonUranus;
	private Button buttonNeptune;
	private Planet currentPlanet;
	public static Vector3 blackholeSize = new Vector3(0.5f, 0.5f, 1f); //Vector for the size of the blackhole. Static because it gets bigger with each loss.
	private bool crossFadeBorderOnWrongClick; //boolean that checks if the player clicked an inactive planet and starts a crossfade to show the correct planets
	private float crossFadeTime;
	private float rotationBlackhole; //value of the rotation
	public static float rotationSpeedBlackhole = 10f; //public static value of the speed of blackhole rotation that changes every time the player loses a fight
	public static int chanceToLoseByGettingSuckedIntoABlackhole = 1; //Small chance to lose the game by getting sucked into a blackhole that rises every time you lose a fight

	/// <summary>
	/// Method ran at the start of the scene.
	/// </summary>
	void Start () {
		this.initializeVariables ();
		this.setPlanets ();

		if (Spaceship.lostLastGame) { //lost last game
			Spaceship.traveledToAPlanetButLost = Spaceship.currentPlanet; //saves the planet where we lost at so we can block movement to there
			Spaceship.currentPlanet = Spaceship.previousPlanet; //sets the previous planet where we left from to the current one
		}
		foreach (Planet planet in planets) { //iterate through all the planets
			if (planet.GetPlanetName().Equals(Spaceship.currentPlanet)) { //we found the Planet planet that matches with the currentPlanet string from the Spaceship static variable
				currentPlanet = planet;
			}
		}

		this.imageSettings ();
	}

	/// <summary>
	/// Initializes the variables.
	/// </summary>
	private void initializeVariables() {
		rotationBlackhole = 0;
		crossFadeTime = 1f;
		crossFadeBorderOnWrongClick = false; 

		buttonMercury = GameObject.Find ("ButtonMercury").GetComponent<Button> ();
		buttonVenus = GameObject.Find ("ButtonVenus").GetComponent<Button> ();
		buttonEarth = GameObject.Find ("ButtonEarth").GetComponent<Button> ();
		buttonMars = GameObject.Find ("ButtonMars").GetComponent<Button> ();
		buttonJupiter = GameObject.Find ("ButtonJupiter").GetComponent<Button> ();
		buttonSaturn = GameObject.Find ("ButtonSaturn").GetComponent<Button> ();
		buttonUranus = GameObject.Find ("ButtonUranus").GetComponent<Button> ();
		buttonNeptune = GameObject.Find ("ButtonNeptune").GetComponent<Button> ();
		rawImageSpaceship = GameObject.Find ("RawImageSpaceship").GetComponent<RawImage> ();
		rawImageBlackhole = GameObject.Find ("RawImageBlackhole").GetComponent<RawImage> ();
		rawImageMercuryBorder = GameObject.Find ("RawImageMercuryBorder").GetComponent<RawImage> ();
		rawImageVenusBorder = GameObject.Find ("RawImageVenusBorder").GetComponent<RawImage> ();
		rawImageEarthBorder = GameObject.Find ("RawImageEarthBorder").GetComponent<RawImage> ();
		rawImageMarsBorder = GameObject.Find ("RawImageMarsBorder").GetComponent<RawImage> ();
		rawImageJupiterBorder = GameObject.Find ("RawImageJupiterBorder").GetComponent<RawImage> ();
		rawImageSaturnBorder = GameObject.Find ("RawImageSaturnBorder").GetComponent<RawImage> ();
		rawImageUranusBorder = GameObject.Find ("RawImageUranusBorder").GetComponent<RawImage> ();
		rawImageNeptuneBorder = GameObject.Find ("RawImageNeptuneBorder").GetComponent<RawImage> ();
		rawImageBlackhole.transform.localScale = blackholeSize; //set the size of the blackhole to what the vector3 shipSize is
	}

	/// <summary>
	/// Sets the neighbour planets and adds them click listeners.
	/// </summary>
	private void setPlanets() {
		planets = new List<Planet> (); //new list of planets
		mercury = new Planet ("mercury", "mercurypic"); //Create a new object of the class Planet called mercury
		planets.Add (mercury); //add mercury to planets
		venus = new Planet ("venus", "venuspic");
		planets.Add (venus);
		earth = new Planet ("earth", "earthpic");
		planets.Add (earth);
		mars = new Planet ("mars", "marspic");
		planets.Add (mars);
		jupiter = new Planet ("jupiter", "jupiterpic");
		planets.Add (jupiter);
		saturn = new Planet ("saturn", "saturnpic");
		planets.Add (saturn);
		uranus = new Planet ("uranus", "uranuspic");
		planets.Add (uranus);
		neptune = new Planet ("neptune", "neptunepic");
		planets.Add (neptune);

		mercury.SetPreviousPlanet (venus); //set the previous planet of mercury
		mercury.SetNextPlanet (venus); //set the next planet of mercury
		venus.SetPreviousPlanet (mercury);
		venus.SetNextPlanet (earth);
		earth.SetPreviousPlanet (venus);
		earth.SetNextPlanet (mars);
		mars.SetPreviousPlanet (earth);
		mars.SetNextPlanet (jupiter);
		jupiter.SetPreviousPlanet (mars);
		jupiter.SetNextPlanet (saturn);
		saturn.SetPreviousPlanet (jupiter);
		saturn.SetNextPlanet (uranus);
		uranus.SetPreviousPlanet (saturn);
		uranus.SetNextPlanet (neptune);
		neptune.SetPreviousPlanet (uranus);
		neptune.SetNextPlanet (uranus);

		buttonMercury.onClick.AddListener (() => planetClicked ("mercury")); //add listener method for clicking on mercury
		buttonVenus.onClick.AddListener (() => planetClicked ("venus"));
		buttonEarth.onClick.AddListener (() => planetClicked ("earth"));
		buttonMars.onClick.AddListener (() => planetClicked ("mars"));
		buttonJupiter.onClick.AddListener (() => planetClicked ("jupiter"));
		buttonSaturn.onClick.AddListener (() => planetClicked ("saturn"));
		buttonUranus.onClick.AddListener (() => planetClicked ("uranus"));
		buttonNeptune.onClick.AddListener (() => planetClicked ("neptune"));
	}

	/// <summary>
	/// Handle the button clicking and check if the click is allowed 
	/// </summary>
	/// <param name="clickedPlanet">Name of the planet clicked.</param>
	private void planetClicked (string clickedPlanet) { 
		//mercury and neptune are the last planets on the edge of the Solar System (can't stop movement even if loss)
		if (currentPlanet.GetPlanetName ().Equals ("mercury") || currentPlanet.GetPlanetName ().Equals ("neptune") && (currentPlanet.GetNextPlanet ().GetPlanetName ().Equals (clickedPlanet) || currentPlanet.GetPreviousPlanet ().GetPlanetName ().Equals (clickedPlanet))) {
			this.startNextScene (clickedPlanet);
			//check if the clicked planet is the current planet's next or previous planet and that the clicked planet is not the same as we just tried to travel to and failed at
		} else if ((currentPlanet.GetNextPlanet ().GetPlanetName ().Equals (clickedPlanet) || currentPlanet.GetPreviousPlanet ().GetPlanetName ().Equals (clickedPlanet)) && (!clickedPlanet.Equals (Spaceship.traveledToAPlanetButLost))) {
			//as long as we didn't lose a game there we can go there
			if (!(Spaceship.lostLastGame == true && Spaceship.traveledToAPlanetButLost.Equals (clickedPlanet))) {
				this.startNextScene (clickedPlanet);
			}
		} else { //the player clicked on a planet that is not allowed to be traveled to so we show the player the correct planets
			this.showClickablePlanets ();
		}
	}

	/// <summary>
	/// Starts the next scene.
	/// </summary>
	/// <param name="clickedPlanet">Clicked planet.</param>
	private void startNextScene(string clickedPlanet) { //start the scene of the next planet or go to asteroid avoiding game
		Spaceship.lostLastGame = false; //we are going to the next planet so set the variable that checks if we lost the last game to false
		//Small chance to lose the game by getting sucked into a blackhole that rises every time you lose a fight (starts at 1%, roll is from 1 to 100)
		if (Random.Range (1, 101) <= StarmapGameController.chanceToLoseByGettingSuckedIntoABlackhole) { 
			SceneManager.LoadScene ("Blackhole");
		} else { //didn't get sucked by a blackhole
			int rnd = Random.Range (0, 2); //50% chance to have to avoid asteroids while travelling to a planet
			Spaceship.previousPlanet = Spaceship.currentPlanet;  //we are traveling so change the previous planet
			foreach (Planet planet in planets) {
				if (planet.GetPlanetName ().Equals (clickedPlanet)) {
					Spaceship.currentPlanet = planet.GetPlanetName (); //change the current planet
					break;
				}
			}
			if (rnd == 0) { //50%
				SceneManager.LoadScene ("Asteroids");
			} else { //50%
				if (clickedPlanet.Equals ("earth")) { //if earth then no scene
					SceneManager.LoadScene ("Starmap");
				} else if (clickedPlanet.Equals ("neptune") && Spaceship.speedOfShip < 9f) { //not enough speed to get to Neptune (at least 4 wins (+=1f) needed to get to Neptune)
					SceneManager.LoadScene ("InsideSpaceship");
				} else {
					StringBuilder planetScene = new StringBuilder(); //new StringBuilder that is used to create the name of the next scene we want to start
					//create a string that is the planet name with first letter capitalized and then Scene
					planetScene.Append (clickedPlanet.First ().ToString ().ToUpper () + clickedPlanet.Substring (1)).Append("Scene"); //create a string that is the planet name with first letter capitalized and then Scene
					SceneManager.LoadScene (planetScene.ToString());

				}
			}
		}
	}

	/// <summary>
	/// RawImage settings. Show the player's spaceship in correct position and show the clicking help border on only those planets to which the player is allowed to travel to.
	/// </summary>
	private void imageSettings() {
		rawImageMercuryBorder.enabled = false;
		rawImageVenusBorder.enabled = false;
		rawImageEarthBorder.enabled = false;
		rawImageMarsBorder.enabled = false;
		rawImageJupiterBorder.enabled = false;
		rawImageSaturnBorder.enabled = false;
		rawImageUranusBorder.enabled = false;
		rawImageNeptuneBorder.enabled = false;

		switch (Spaceship.currentPlanet) { //switch case that goes through every planet, checking for the settings according to our current planet
		case "mercury": //case "mercury"
			rawImageSpaceship.transform.position = rawImageMercuryBorder.transform.position; //move the spaceship image to the center of a planet
			rawImageSpaceship.transform.Translate (0, 45, 0); //raise the position of the spaceship a bit on the y-axis to make it look better
			rawImageVenusBorder.enabled = true; //last planet on the edge (can't stop movement even if loss so we always show venus as clickable)
			break;
		case "venus": 
			rawImageSpaceship.transform.position = rawImageVenusBorder.transform.position;
			rawImageSpaceship.transform.Translate (0, 50, 0);
			if (!Spaceship.traveledToAPlanetButLost.Equals ("mercury")) { //if we didn't lost at mercury
				rawImageMercuryBorder.enabled = true; //show mercury border
			} if (!Spaceship.traveledToAPlanetButLost.Equals ("earth")) { //if we didn't lose at earth
				rawImageEarthBorder.enabled = true; //show earth border
			}
			break;
		case "earth": 
			Debug.Log("earth image case");
			rawImageSpaceship.transform.position = rawImageEarthBorder.transform.position;
			rawImageSpaceship.transform.Translate (0, 55, 0);
			if (!Spaceship.traveledToAPlanetButLost.Equals ("venus")) {
				rawImageVenusBorder.enabled = true;
			}
			if (!Spaceship.traveledToAPlanetButLost.Equals ("mars")) {
				rawImageMarsBorder.enabled = true;
			}
			break;
		case "mars":
			rawImageSpaceship.transform.position = rawImageMarsBorder.transform.position;
			rawImageSpaceship.transform.Translate (0, 60, 0);
			if (!Spaceship.traveledToAPlanetButLost.Equals ("earth")) {
				rawImageEarthBorder.enabled = true;
			} 
			if (!Spaceship.traveledToAPlanetButLost.Equals ("jupiter")) {
				rawImageJupiterBorder.enabled = true;
			}
			break;
		case "jupiter":
			rawImageSpaceship.transform.position = rawImageJupiterBorder.transform.position;
			rawImageSpaceship.transform.Translate (0, 75, 0);
			if (!Spaceship.traveledToAPlanetButLost.Equals ("mars")) {
				rawImageMarsBorder.enabled = true;
			} 
			if (!Spaceship.traveledToAPlanetButLost.Equals ("saturn")) {
				rawImageSaturnBorder.enabled = true;
			}
			break;
		case "saturn":
			rawImageSpaceship.transform.position = rawImageSaturnBorder.transform.position;
			rawImageSpaceship.transform.Translate (0, 75, 0);
			if (!Spaceship.traveledToAPlanetButLost.Equals ("jupiter")) {
				rawImageJupiterBorder.enabled = true;
			} 
			if (!Spaceship.traveledToAPlanetButLost.Equals ("uranus")) {
				rawImageUranusBorder.enabled = true;
			}
			break;
		case "uranus":
			rawImageSpaceship.transform.position = rawImageUranusBorder.transform.position;
			rawImageSpaceship.transform.Translate (0, 65, 0);
			if (!Spaceship.traveledToAPlanetButLost.Equals ("saturn")) {
				rawImageSaturnBorder.enabled = true;
			} 
			if (!Spaceship.traveledToAPlanetButLost.Equals ("neptune")) {
				rawImageNeptuneBorder.enabled = true;
			}
			break;
		case "neptune":
			rawImageSpaceship.transform.position = rawImageNeptuneBorder.transform.position;
			rawImageSpaceship.transform.Translate (0, 65, 0);
			rawImageUranusBorder.enabled = true;
			break;
		}
	}

	/// <summary>
	/// Shows the clickable planets by fading the borders of the clickable planets down.
	/// </summary>
	private void showClickablePlanets() {
		crossFadeBorderOnWrongClick = true; //we started fading the borders away
		rawImageMercuryBorder.CrossFadeAlpha (0.1f, 1f, false);
		rawImageVenusBorder.CrossFadeAlpha (0.1f, 1f, false);
		rawImageEarthBorder.CrossFadeAlpha (0.1f, 1f, false);
		rawImageMarsBorder.CrossFadeAlpha (0.1f, 1f, false);
		rawImageJupiterBorder.CrossFadeAlpha (0.1f, 1f, false);
		rawImageSaturnBorder.CrossFadeAlpha (0.1f, 1f, false);
		rawImageUranusBorder.CrossFadeAlpha (0.1f, 1f, false);
		rawImageNeptuneBorder.CrossFadeAlpha (0.1f, 1f, false);
	}

	/// <summary>
	/// Method ran every frame. Checks if crossfading down has started happening. Also rotates blackhole.
	/// </summary>
	void Update() {
		if (crossFadeBorderOnWrongClick) { //fade the planet indicators (that tell which planets can be clicked) back to visible if they have been faded to invisble
			this.crossFadeUp ();
		}
		rotationBlackhole += Time.deltaTime * StarmapGameController.rotationSpeedBlackhole; //change the rotation (variable float) of the blackhole depending on the time
		rawImageBlackhole.transform.rotation = Quaternion.Euler(0,0,-rotationBlackhole); //actually change the rotation of the blackhole here
	}

	/// <summary>
	/// If crossfade down timer has reached 0 then start crossfading the borders back up
	/// </summary>
	private void crossFadeUp() {
		crossFadeTime -= Time.deltaTime;
		if (crossFadeTime <= 0) {
			rawImageMercuryBorder.CrossFadeAlpha (1f, 1f, false);
			rawImageVenusBorder.CrossFadeAlpha (1f, 1f, false);
			rawImageEarthBorder.CrossFadeAlpha (1f, 1f, false);
			rawImageMarsBorder.CrossFadeAlpha (1f, 1f, false);
			rawImageJupiterBorder.CrossFadeAlpha (1f, 1f, false);
			rawImageSaturnBorder.CrossFadeAlpha (1f, 1f, false);
			rawImageUranusBorder.CrossFadeAlpha (1f, 1f, false);
			rawImageNeptuneBorder.CrossFadeAlpha (1f, 1f, false);
			crossFadeTime = 1f;
			crossFadeBorderOnWrongClick = false;
		}
	}

}
