using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game controller minesweeper.
/// </summary>
public class GameControllerMinesweeper : MonoBehaviour {

	public GameObject prefabButtonNotMine; //prefab ButtonNotMine is set from unity to this GameObject for cloning 
	public GameObject prefabButtonMine; 
	public GameObject prefabButtonNotMineSmall; //different size buttons are needed for scaling
	public GameObject prefabButtonMineSmall; 
	public GameObject prefabButtonNotMineBig;
	public GameObject prefabButtonMineBig; 
	private GameObject canvas;
	private GameObject buttonContinue;
	private GameObject buttonQuit;
	private GameObject clonedMinesParent; //empty gameobject that holds the buttons
	private GameObject panelYouLost;
	private Text textYouLost;
	public Sprite mineExplosionSprite;
	public Sprite evilPenguinSprite;
	public Sprite uiButtonSprite;
	private AudioSource source;
	public AudioClip explosion;
	private List<Mine> mines;
	private List<GameObject> notMineButtons;
	private List<GameObject> mineButtons;
	private Dictionary<GameObject, int> numberOfMines; //the number of mines that are next to the button
	private int gameSizeX;
	private int gameSizeY; 
	private int amountOfMinesDefused; //the amount of mines the player has marked
	private int amountOfNonMinesClicked; // the amount of buttons that are not mines that are clicked
	private float buttonSize; //scale the size of the buttons depending on the size of the game board



	/// <summary>
	/// Method used for intialization.
	/// </summary>
	void Start () {
		explosion = (AudioClip) Resources.Load("Sounds/Explosion-sound/Explosion-sound", typeof(AudioClip));
		source = GetComponent<AudioSource> ();
//		bool hasPlayerGivenGameSettings = false; //false when player has not finished giving the game size and the amount of mines settings.
		int amountOfMines = MineSettingsController.amountOfMines; //the amount of mines we want to create (static variable from previous scene)
		gameSizeX = MineSettingsController.gameSizeX; //size of the game board x-axis
		gameSizeY = MineSettingsController.gameSizeY; //size of the game board y-axis
		if (gameSizeX * gameSizeY <= 400 && gameSizeX * gameSizeY >= 101) {
			buttonSize = 50f;
		} else if (gameSizeX * gameSizeY < 101) {
			buttonSize = 100f;
		} else if (gameSizeX * gameSizeY > 400) {
			buttonSize = 30f;
		} 
		Debug.Log ("button size: " + buttonSize);
		amountOfMinesDefused = 0;
		amountOfNonMinesClicked = 0;
		mines = new List<Mine>();
		notMineButtons = new List<GameObject> ();
		mineButtons = new List<GameObject> ();
		numberOfMines = new Dictionary<GameObject, int> ();
		canvas = GameObject.Find("Canvas");
		textYouLost = GameObject.Find ("TextYouLost").GetComponent<Text>();
		buttonContinue = GameObject.Find ("ButtonContinue"); //set as GameObject instead of Button to be able to set it active/inactive easily
		buttonQuit = GameObject.Find ("ButtonQuit");
		clonedMinesParent = GameObject.Find ("ClonedMinesParent");
		panelYouLost = GameObject.Find ("PanelYouLost");

		buttonContinue.GetComponent<Button>().onClick.AddListener (() => gameLostButtonClicked ("continue")); //adds a mouse click listener to the button and sends string continue to the method buttonClicked when the click happens
		buttonQuit.GetComponent<Button>().onClick.AddListener (() => gameLostButtonClicked ("quit"));
		buttonContinue.SetActive (false); //hide the ui button by making it non active
		buttonQuit.SetActive (false);
		panelYouLost.SetActive (false);
		textYouLost.text = ""; //hide the ui text by making it empty

		this.createMines (amountOfMines); //creates the mines
		this.createGameField (); //creates the game field by creating nonmine and mine buttons
		this.calculateMinesNextToAButton (); //calculates how many mines are next to a non mine button
	}

	/// <summary>
	/// Creates the random positions where mines will be created.
	/// </summary>
	/// <param name="amountOfMines">The number of mines that the player wants to create.</param>
	private void createMines(int amountOfMines) {
		int randX;
		int randY;
		bool mineAlreadyCreated = false;

		for (int i = 0; i < amountOfMines; i++) {
			do {
				mineAlreadyCreated = false;
				randX = (int)System.Math.Ceiling (Random.Range (-gameSizeX / 2f, gameSizeX / 2f)); //random from 0 to gameSizeX-1
				randY = (int)System.Math.Ceiling (Random.Range (-gameSizeY / 2f, gameSizeY / 2f));

				foreach (Mine mine in mines) { //make sure no mines are created in the same place where another mine already exists
					if (mine.GetX () == randX && mine.GetY () == randY) {
						mineAlreadyCreated = true;
					}
				}
				if (mineAlreadyCreated == false) { //no mine exists in those coordinates already so we can create a mine there
					Mine mine = new Mine (randX, randY); //create a new Mine-type object
//				Debug.Log(mine.GetX() + " " + mine.GetY());
					mines.Add (mine); //add the mine to the list of mines
				}
			} while (mineAlreadyCreated);
		}
	}

	/// <summary>
	/// Creates the game field. Depending on the size of the game field either clones small, medium or big prefab mine buttons and prefab notmine buttons.
	/// </summary>
	private void createGameField() {
		int test = 1, test2 = 1; //counting mines and objects that are under the mines
		//first buttons that are not mines are created
		for (int i = (int)System.Math.Ceiling(-gameSizeY/2f)+1; i < (int)System.Math.Ceiling(gameSizeY/2f)+1; i++) { //y-axis gamefield
			for (int j = (int)System.Math.Ceiling(-gameSizeY/2f)+1; j < (int)System.Math.Ceiling(gameSizeX/2f)+1; j++) { //x-axis gamefield
				Debug.Log("y: " + i + " x: " + j);
				GameObject notMineButtonClone = new GameObject ();
				if (buttonSize == 50) { 
					notMineButtonClone = Instantiate (prefabButtonNotMine, new Vector3 (canvas.transform.position.x + j * buttonSize, canvas.transform.position.y + i * buttonSize, canvas.transform.position.z + 0), Quaternion.identity) as GameObject; //clone the prefab ButtonNotMine
				} else if (buttonSize == 100) {
					notMineButtonClone = Instantiate (prefabButtonNotMineBig, new Vector3 (canvas.transform.position.x + j * buttonSize, canvas.transform.position.y + i * buttonSize, canvas.transform.position.z + 0), Quaternion.identity) as GameObject; //clone the prefab ButtonNotMine
				} else {
					notMineButtonClone = Instantiate (prefabButtonNotMineSmall, new Vector3 (canvas.transform.position.x + j * buttonSize, canvas.transform.position.y + i * buttonSize, canvas.transform.position.z + 0), Quaternion.identity) as GameObject; //clone the prefab ButtonNotMine
				}

//				notMineButtonClone.transform.SetParent (canvas.transform); //set clone's parent as canvas 
				notMineButtonClone.transform.SetParent (clonedMinesParent.transform); //make it a child of an empty gameobject
				notMineButtonClone.transform.position = clonedMinesParent.transform.position;
				notMineButtonClone.transform.Translate ((j-1) * buttonSize, i * buttonSize, 0); //move the clone to the desired position
//				notMineButtonClone.transform.Translate (clonedMinesParent.transform.position.x + j * buttonSize, clonedMinesParent.transform.position.y + i * buttonSize, 0);
				notMineButtonClone.AddComponent<MouseButtonController>(); //add the script that checks if mouse button is held down instead of a click
				notMineButtonClone.GetComponent<Button>().onClick.AddListener (() => buttonNotMineClicked (notMineButtonClone)); //create a mouse button listener for the button
				notMineButtons.Add (notMineButtonClone);
			}
		}
//		Debug.Log ("Amount of non mine buttons at the start: " + notMineButtons.Count);
		foreach (Mine mine in mines) { //buttons that are mines are created over the non mine ones
			GameObject mineButtonClone = new GameObject ();
			if (buttonSize == 50) { 
				mineButtonClone = Instantiate (prefabButtonMine, new Vector3 (canvas.transform.position.x + mine.GetX() * buttonSize, canvas.transform.position.y + mine.GetY() * buttonSize, canvas.transform.position.z + 0), Quaternion.identity) as GameObject;
			} else if (buttonSize == 100) {
				mineButtonClone = Instantiate (prefabButtonMineBig, new Vector3 (canvas.transform.position.x + mine.GetX() * buttonSize, canvas.transform.position.y + mine.GetY() * buttonSize, canvas.transform.position.z + 0), Quaternion.identity) as GameObject;
			} else {
				mineButtonClone = Instantiate (prefabButtonMineSmall, new Vector3 (canvas.transform.position.x + mine.GetX() * buttonSize, canvas.transform.position.y + mine.GetY() * buttonSize, canvas.transform.position.z + 0), Quaternion.identity) as GameObject;
			}
			//			mineButtonClone.transform.SetParent (canvas.transform);
			mineButtonClone.transform.SetParent (clonedMinesParent.transform); //make it a child of an empty gameobject
			mineButtonClone.transform.position = clonedMinesParent.transform.position;
//			mineButtonClone.transform.Translate (canvas.transform.position.x + mine.GetX() * 25-470, canvas.transform.position.y + mine.GetY() * 25-220, canvas.transform.position.z + 0);
			mineButtonClone.transform.Translate((mine.GetX()-1) * buttonSize, mine.GetY() * buttonSize, 0);
			mineButtonClone.AddComponent<MouseButtonController>(); //add the script that checks if mouse button is held down instead of a click
			mineButtonClone.GetComponent<Button>().onClick.AddListener (() => buttonMineClicked (mineButtonClone));
			mineButtons.Add (mineButtonClone);

			//remove a button that is not a mine from under the mine button
			foreach (GameObject gameobj in notMineButtons.Reverse<GameObject>()) { //Destroy all buttons that are located under the mine buttons. To destroy objects from a list while we are iterating through it we have to iterate through a reversed list.
				if (gameobj.transform.position.x > mineButtonClone.transform.position.x-1 && gameobj.transform.position.x < mineButtonClone.transform.position.x+1) { //small tolerance to the numbers to fix floating point errors
					if (gameobj.transform.position.y > mineButtonClone.transform.position.y - 1 && gameobj.transform.position.y < mineButtonClone.transform.position.y + 1) {
//						Debug.Log (test + " Button: " + gameobj + "\nMine: " + mineButtonClone);
						test++;
						notMineButtons.Remove (gameobj); //remove the button from the list before it is destroyed
						Destroy (gameobj); //destroy the button
					}
				}
			}
//			Debug.Log (test2);
			test2++;
		}
//		Debug.Log ("Amount of non mine buttons after some of them are destroyed: " + notMineButtons.Count);
	}

	/// <summary>
	/// Calculates the amount of mines next to a button.
	/// </summary>
	private void calculateMinesNextToAButton() {
		foreach (GameObject button in notMineButtons) { //iterate through every button that is not a mine
			int numbOfMines = 0;
			foreach (GameObject mine in mineButtons) { //iterate through every button that is a mine
				for (float y = -buttonSize; y <= buttonSize; y += buttonSize) { //25f is the size of the side of a square button look for 3 different rows: the row the button is on and the rows that are on top and below it
					for (float x = -buttonSize; x <= buttonSize; x += buttonSize) {
						if (mine.transform.position.x > button.transform.position.x + x - 1f && mine.transform.position.x < button.transform.position.x + x + 1f) {
							if (mine.transform.position.y > button.transform.position.y + y - 1f && mine.transform.position.y < button.transform.position.y + y + 1f) {
								numbOfMines++;
//								Debug.Log ("Mine found: " + numbOfMines);
							}
						}
					}
				}
			}
			numberOfMines.Add(button, numbOfMines);
		}
	}

	/// <summary>
	/// Handles what happens when the player clicks a button that is a mine. 
	/// </summary>
	/// <param name="button">The mine button that was clicked.</param>
	private void buttonMineClicked(GameObject button) {
		if (button.GetComponent<MouseButtonController> ().GetButtonTimer () > 20) { //check if mouse button has been held down for over 20 frames (not a click but a hold then)
			if (!button.GetComponent<Button> ().image.sprite.name.Equals ("evilPenguin")) { //check that button is not marked as a mine already
				amountOfMinesDefused++; //add 1 mine as "defused"
				button.GetComponent<Button> ().image.sprite = evilPenguinSprite;
			} else { //the button has already been marked as a mine so we take the sprite away
				amountOfMinesDefused--; //remove 1 mine from being defused
				button.GetComponent<Button> ().image.sprite = uiButtonSprite;
			}
		} else {
			if (!button.GetComponent<Button> ().image.sprite.name.Equals ("evilPenguin")) { //Don't allow a click on marked buttons.
				button.GetComponent<Button> ().image.sprite = mineExplosionSprite;
				source.mute = false;
				source.PlayOneShot (explosion, 1F);
				source.Play ();
//			Debug.Log ("testii" + source.isPlaying.ToString());
				this.gameEnds ();
			}
		}
		//check if game is won
		this.isGameWon();
	}

	/// <summary>
	/// Handles what happens when the player clicks a button that is a not a mine. 
	/// </summary>
	/// <param name="button">The not mine button that was clicked.</param>
	private void buttonNotMineClicked(GameObject button) {
		if (button.GetComponent<MouseButtonController> ().GetButtonTimer () > 20) { //check if mouse button has been held down for over 20 frames (not a click but a hold then)
//			Debug.Log (button.GetComponent<Button> ().image.sprite.name);
			if (!button.GetComponent<Button> ().image.sprite.name.Equals("evilPenguin")) { //check that button is not marked as a mine already
				button.GetComponent<Button> ().image.sprite = evilPenguinSprite;
			} else { //the button has already been marked as a mine so we take the sprite away
				button.GetComponent<Button> ().image.sprite = uiButtonSprite;
			}
		} else { //mouse button wasn't held down, it was a click
			if (!button.GetComponent<Button> ().image.sprite.name.Equals ("evilPenguin")) { //Don't allow a click on marked buttons.
				amountOfNonMinesClicked++;
				button.GetComponent<Button> ().interactable = false; //make the button not clickable
				button.GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "" + numberOfMines [button];
				if (numberOfMines [button] == 0) { 
					this.searchForZeroMineNeighbourButtons (button);
				}
			}
		}
		//check if game is won
		this.isGameWon();
	}
		
	/// <summary>
	/// Searchs for neighbour buttons that have 0 mines next to them.
	/// </summary>
	/// <param name="button">The button that was not a mine that was clicked and has 0 mines next to it.</param>
	private void searchForZeroMineNeighbourButtons(GameObject button) { //check all neighbour mines for other 0s and mark them as clicked too
		foreach (GameObject notMineButton in notMineButtons) {
			if (notMineButton.GetComponent<Button>().IsInteractable() == true) {
				//Check if there are 0s on top of the button, below the button, on the left side of the button and on the right side of the button
				//top
				if (notMineButton.transform.position.y > button.transform.position.y + buttonSize - 1f && notMineButton.transform.position.y < button.transform.position.y + buttonSize + 1f) {
					if (notMineButton.transform.position.x > button.transform.position.x - 1f && notMineButton.transform.position.x < button.transform.position.x + 1f) {
						if (numberOfMines [notMineButton] == 0) { 
							notMineButton.GetComponent<Button> ().interactable = false; //make the button not clickable
							notMineButton.GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "" + numberOfMines [notMineButton];
							amountOfNonMinesClicked++;
							this.searchForZeroMineNeighbourButtons (notMineButton);
						}

					}
				}
				//bottom
				if (notMineButton.transform.position.y > button.transform.position.y - buttonSize - 1f && notMineButton.transform.position.y < button.transform.position.y - buttonSize + 1f) {
					if (notMineButton.transform.position.x > button.transform.position.x - 1f && notMineButton.transform.position.x < button.transform.position.x + 1f) {
						if (numberOfMines [notMineButton] == 0) { 
							notMineButton.GetComponent<Button> ().interactable = false; //make the button not clickable
							notMineButton.GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "" + numberOfMines [notMineButton];
							amountOfNonMinesClicked++;
							this.searchForZeroMineNeighbourButtons (notMineButton);
						}

					}
				}
				//left
				if (notMineButton.transform.position.x > button.transform.position.x - buttonSize - 1f && notMineButton.transform.position.x < button.transform.position.x - buttonSize + 1f) {
					if (notMineButton.transform.position.y > button.transform.position.y - 1f && notMineButton.transform.position.y < button.transform.position.y + 1f) {
						if (numberOfMines [notMineButton] == 0) { 
							notMineButton.GetComponent<Button> ().interactable = false; //make the button not clickable
							notMineButton.GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "" + numberOfMines [notMineButton];
							amountOfNonMinesClicked++;
							this.searchForZeroMineNeighbourButtons (notMineButton);
						}

					}
				}
				//right
				if (notMineButton.transform.position.x > button.transform.position.x + buttonSize - 1f && notMineButton.transform.position.x < button.transform.position.x + buttonSize + 1f) {
					if (notMineButton.transform.position.y > button.transform.position.y - 1f && notMineButton.transform.position.y < button.transform.position.y + 1f) {
						if (numberOfMines [notMineButton] == 0) { 
							notMineButton.GetComponent<Button> ().interactable = false; //make the button not clickable
							notMineButton.GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "" + numberOfMines [notMineButton];
							amountOfNonMinesClicked++;
							this.searchForZeroMineNeighbourButtons (notMineButton);
						}
					}
				}
			}
		}
	}
		
	/// <summary>
	/// The game ended by the player clicking on a mine.
	/// </summary>
	private void gameEnds() {
		StarmapGameController.rotationSpeedBlackhole += 50f; //set blackhole to rotate faster
		StarmapGameController.chanceToLoseByGettingSuckedIntoABlackhole += 5; //raises the chance to lose by getting sucked into a blackhole by 5%
		StarmapGameController.blackholeSize = new Vector3 (StarmapGameController.blackholeSize.x + 0.25f, StarmapGameController.blackholeSize.y + 0.25f, 1f); //change the size of the blackhole
		Spaceship.lostLastGame = true;

		foreach (GameObject notMineButton in notMineButtons) { //set all notMineButtons to not interactable and show what was under them
			if (notMineButton.GetComponent<Button> ().IsInteractable () == true) {
				notMineButton.GetComponent<Button> ().interactable = false;
				notMineButton.GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "" + numberOfMines [notMineButton];
			}
		}
		foreach (GameObject mine in mineButtons) { //set all mine buttons to show a picture of an evil penguin so the player sees where there were mines
			if (mine.GetComponent<Button> ().image.sprite != mineExplosionSprite) { //do not change the exploded mine into an evil penguin
				mine.GetComponent<Button> ().image.sprite = evilPenguinSprite;
				mine.GetComponent<Button> ().interactable = false;
			}
		}

		textYouLost.text = "A mine was triggered :("; //write "A mine was triggered :(" to the screen
		buttonContinue.SetActive (true); //set the button the be active and be shown to the player
		buttonQuit.SetActive (true);
		panelYouLost.SetActive (true);
	}

	/// <summary>
	/// Player lost the game and clicked either "continue" or "quit".
	/// </summary>
	/// <param name="button">Button.</param>
	private void gameLostButtonClicked(string button) { //a method that is ran when a button is clicked
		if (button.Equals ("continue")) {
			SceneManager.LoadScene ("Starmap"); //go back to starmap
		} else {
			Application.Quit (); //quit the game
		}
		
	}
		
	/// <summary>
	/// Checks if the player won by marking all the mines or by clicking on every non mine button.
	/// </summary>
	private void isGameWon() { //checks if the player has won
//		Debug.Log ("Mines defused: " + amountOfMinesDefused + "\nNot mines clicked: " + amountOfNonMinesClicked);
		if (amountOfMinesDefused == mineButtons.Count) { //check if every mines has been marked
			Spaceship.speedOfShip += 1f; //survived a planet and got a ship part as a reward that helps make the ship's motor stronger
			Spaceship.traveledToAPlanetButLost = " "; //we won so empty traveledToAPlanetButLost
			SceneManager.LoadScene ("Starmap");
		}
		if (amountOfNonMinesClicked == notMineButtons.Count) { //check if every non mine has been clicked
			Spaceship.speedOfShip += 1f; //survived a planet and got a ship part as a reward that helps make the ship's motor stronger
			Spaceship.traveledToAPlanetButLost = " "; //we won so empty traveledToAPlanetButLost
			SceneManager.LoadScene ("Starmap");
		}
	}
}
