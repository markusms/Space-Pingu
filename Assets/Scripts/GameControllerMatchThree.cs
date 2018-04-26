using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game controller for match3 game.
/// </summary>
public class GameControllerMatchThree : MonoBehaviour {
	//boolean that checks if the player makes a failed move that results in no gem getting destroyed
	private bool gemWasMovedButNoMatches = false;
	private bool failExplosion = false; //do the explosion only once
	private bool soundEffectOnce;
	private int frameCounter = 0;
	private int width = 6;
	private int height = 6;
	private int selected = 0; //how many gems are selected
	//how many times can the player move without destroying any gems before losing
	private int failedMovesLeft = 3;
	public int points = 0; //you get points for each gem destroyed
	private List <Gem> selectedGems = new List<Gem> (); //list of selected gems
	private List<Gem> gemsSameColorOverUnder = new List<Gem> (); //list of same color gems that are over and under the current gem
	private List<Gem> gemsSameColorLeftRight = new List<Gem> ();
	private List<Gem> gemsToBeDestroyed = new List<Gem> (); //gems that are going to be destroyed
	public List <Gem> gems = new List<Gem> (); //list of all gems
	public GameObject gemPrefab; //prefab Gem set in Unity
	private GameObject buttonContinue;
	private GameObject buttonQuit;
	private Text textYouLost;
	private Text scoreText;
	private Text failMoveText;
	private AudioSource audioSource;
	public AudioClip blockSound;

	/// <summary>
	/// Method ran once at the start.
	/// </summary>
	void Start () {
		this.variableInitialization ();
		this.createGameField ();
	}

	/// <summary>
	/// Initializes variables.
	/// </summary>
	private void variableInitialization() {
		soundEffectOnce = false;
		audioSource = FindObjectOfType<AudioSource> (); //finds the audio source that is created in the preload scene and not destroyed on load
		scoreText = GameObject.Find ("TextScore").GetComponent<Text> ();
		failMoveText = GameObject.Find ("TextFailedMoves").GetComponent<Text> ();
		scoreText.text = "Blocks destroyed: " + points + "/150 "; //sets score text
		failMoveText.text = "Moves that can fail: " + failedMovesLeft + " "; //sets amount of failed moves left text

		textYouLost = GameObject.Find ("TextYouLost").GetComponent<Text> ();
		buttonContinue = GameObject.Find ("ButtonContinue"); //set as GameObject instead of Button to be able to set it active/inactive easily
		buttonQuit = GameObject.Find ("ButtonQuit");

		buttonContinue.GetComponent<Button> ().onClick.AddListener (() => gameLostButtonClicked ("continue")); //adds a mouse click listener to the button and sends string continue to the method buttonClicked when the click happens
		buttonQuit.GetComponent<Button> ().onClick.AddListener (() => gameLostButtonClicked ("quit"));
		buttonContinue.SetActive (false); //hide the ui button by making it non active
		buttonQuit.SetActive (false);
		textYouLost.text = ""; //hide the ui text by making it empty
	}

	/// <summary>
	/// This method is called every frame.
	/// </summary>
	void Update () {
		//don't go to scoreTracker() every frame because the game needs time to calculate if gems were destroyed to make sure a move wasn't failed (gemWasMovedButNoMatches) 
		if (frameCounter == 30) {
			frameCounter = 0;
			this.scoreTracker ();
		}

		this.checkForSelectedGems ();

		foreach (Gem gem in gems) {
			gemsSameColorOverUnder = new List<Gem> (); //set the list empty
			gemsSameColorLeftRight = new List<Gem> (); 
			this.checkHowManySameColorGemsNextToAGem (gem, "none"); //find same color gem matches of 3+
			this.markForDestroy (gem); //mark the found gems to be destroyed after this foreach loop ends
		}
		//we destroy the gems here instead of inside the previous loop to avoid null exception errors that are caused by removing gems while we are still iterating through the gem list
		this.actuallyDestroyGems (); 
		frameCounter++;
		soundEffectOnce = false;
	}

	/// <summary>
	/// Goes through every gem and checks if they are selected and then moves them if needed.
	/// </summary>
	private void checkForSelectedGems() {
		foreach (Gem gem in gems) { //iterate through all gems
			//if gem is selected and it's not already in the list of selected gems
			if (gem.GetSelected () == true && !selectedGems.Contains (gem)) { 
				selectedGems.Add (gem); //add the gem to selected gems
				selected++;
				//if gem is unselected
			} else if (gem.GetSelected () == false && selectedGems.Contains (gem)) {
				selectedGems.Remove (gem);
				selected--;
			}
		}
		if (selected > 1) { //2 objects have been clicked
			foreach (Gem selectedGem in selectedGems) { //unselect all selected gems
				selectedGem.SetSelected (); //unselect
			}
			if (selectedGems [0].IsNextToYou (selectedGems [1])) { //true if the objects are next to each other, then swap their positions
				Vector3 tempSelectedGemPosition = selectedGems [0].transform.position; //save the position of the first gem
				selectedGems [0].transform.position = selectedGems [1].transform.position; //first gem is moved to the position of second gem
				selectedGems [1].transform.position = tempSelectedGemPosition; //second gem is moved to the original position of the first gem
				gemWasMovedButNoMatches = true; //This boolean is set to true when a move happens. If matches are found it is set back to false.
			}
			selected = 0; 
			selectedGems = new List<Gem> (); //empty the list
		}

	}

	/// <summary>
	/// Creates the game field of gems.
	/// </summary>
	private void createGameField ()	{
		for (int y = 0; y < height; y++) { //height of the game field
			for (int x = 0; x < width; x++) { //width of the game field
				//clone a preFab gem in the correct position of (x,y)
				GameObject gem = Instantiate (gemPrefab, new Vector3 (x, y, 0), Quaternion.identity) as GameObject; 
				//make it a child of the gamecontroller gameobject to easily position all gems in the middle of the game field
				gem.transform.parent = gameObject.transform; 
				gems.Add (gem.GetComponent<Gem> ()); //add it to the list of gems
			}
		}
		gameObject.transform.position = new Vector3 (-2.5f, -2.5f, 0); //center the game field
	}

	/// <summary>
	/// Checks the how many same color gems there are next to the Gem gem.
	/// </summary>
	/// <param name="gem">This gem is the one we are looking for same color neighbours for.</param>
	/// <param name="direction">direction "none" means that we check for gems all around the gem. "up" means we only check for those on top of the gem. "left" means we check for gems on the left side of the gem...</param>
	private void checkHowManySameColorGemsNextToAGem (Gem gem, string direction) { //check the gems that are over and under Gem gem
		foreach (Gem gemNear in gem.GetGemsNearWithSameColor()) {
			//top (check if the gem near to us is on top of us)
			//floating point inaccuracies taken into account when we check if the gem's position is on top of the position of the parameter gem
			if (gemNear.transform.position.y - gem.transform.position.y > 0.1f && (direction.Equals ("none") || direction.Equals ("up")) && gemNear.transform.position.x - gem.transform.position.x < 0.1f && gemNear.transform.position.x - gem.transform.position.x > -0.1f) { 
				gemsSameColorOverUnder.Add (gemNear); //gem was on top of us so we add it to the list of gems on top or below us
				//now we check if there is another same color gem on top of the same color gem that is on top of the original gem that we started checking for
				this.checkHowManySameColorGemsNextToAGem (gemNear, "up"); 
			}
			//bottom
			if (gemNear.transform.position.y - gem.transform.position.y < -0.1f && (direction.Equals ("none") || direction.Equals ("down")) && gemNear.transform.position.x - gem.transform.position.x < 0.1f && gemNear.transform.position.x - gem.transform.position.x > -0.1f) {
				gemsSameColorOverUnder.Add (gemNear);
				this.checkHowManySameColorGemsNextToAGem (gemNear, "down");
			}
			//left
			if (gemNear.transform.position.x - gem.transform.position.x < -0.1f && (direction.Equals ("none") || direction.Equals ("left")) && gemNear.transform.position.y - gem.transform.position.y < 0.1f && gemNear.transform.position.y - gem.transform.position.y > -0.1f) {
				gemsSameColorLeftRight.Add (gemNear);
				this.checkHowManySameColorGemsNextToAGem (gemNear, "left");
			}
			//right
			if (gemNear.transform.position.x - gem.transform.position.x > 0.1f && (direction.Equals ("none") || direction.Equals ("right")) && gemNear.transform.position.y - gem.transform.position.y < 0.1f && gemNear.transform.position.y - gem.transform.position.y > -0.1f) {
				gemsSameColorLeftRight.Add (gemNear);
				this.checkHowManySameColorGemsNextToAGem (gemNear, "right");
			}
		}
	}

	/// <summary>
	/// If matches of 3 or higher were found it's time to destroy the gems.
	/// </summary>
	/// <param name="gem">The gem we started checking matches of same color gems around it.</param>
	private void markForDestroy (Gem gem) { 
		if (gemsSameColorOverUnder.Count > 1 || gemsSameColorLeftRight.Count > 1) { //check if gems need to be destroyed
			//updown
			//at least a match of 3 was found (the actual amount of gems is gemsSameColorOverUnder.Count+1 so if gemsSameColorOverUnder.Count is over 1 aka at least 2 a match of at least 3 gems was found
			if (gemsSameColorOverUnder.Count > 1) { 
				foreach (Gem gemRemoveOverUnder in gemsSameColorOverUnder) {
					//if this gem is already not in the list of gems to be destroyed then it is added
					if (!gemsToBeDestroyed.Contains (gemRemoveOverUnder)) { 
						gemsToBeDestroyed.Add (gemRemoveOverUnder);
					}
				}
			}
			//leftright
			if (gemsSameColorLeftRight.Count > 1) {
				foreach (Gem gemRemoveLeftRight in gemsSameColorLeftRight) {
					if (!gemsToBeDestroyed.Contains (gemRemoveLeftRight)) {
						gemsToBeDestroyed.Add (gemRemoveLeftRight);
					}
				}
			}
			if (!gemsToBeDestroyed.Contains (gem)) { //add the original gem to be destroyed too
				gemsToBeDestroyed.Add (gem);
			}
			gemWasMovedButNoMatches = false; //a match was found so this is set back to false
		}
		gemsSameColorOverUnder = new List<Gem> (); //initialize the list again
		gemsSameColorLeftRight = new List<Gem> ();
	}

	/// <summary>
	/// Destroying the gems happens here.
	/// </summary>
	private void actuallyDestroyGems () { 
		foreach (Gem gem in gemsToBeDestroyed) {
			//we have to spawn a new gem everytime a gem is destroyed and this gives the position where
			Vector3 newGemCoordinate = gem.GetPosition (); 
			gems.Remove (gem); //remove from the list first
			Destroy (gem.gameObject);
			this.createGem (newGemCoordinate); //create a new gem
			points++; //add a point for every destroyed gem
			if (!soundEffectOnce) { //only play the sound effect of a gem getting destroyed once per cycle
				audioSource.PlayOneShot (blockSound);
				soundEffectOnce = true;
			}
		}
		gemsToBeDestroyed = new List<Gem> (); //empty the list
	}

	/// <summary>
	/// Creates a new gem clone.
	/// </summary>
	/// <param name="position">Position where the gem is created.</param>
	private void createGem (Vector3 position) {
		//create a gem clone on top of the position of the older gem
		GameObject gem = Instantiate (gemPrefab, new Vector3 (position.x, position.y + 7f, 0), Quaternion.identity) as GameObject;
		gem.transform.parent = gameObject.transform; //make it a child of the gamecontroller gameobject 
		gems.Add (gem.GetComponent<Gem> ()); //add it to the list of gems
	}

	/// <summary>
	/// Keeps track of the score and checks if the player lost the game.
	/// </summary>
	private void scoreTracker () {
		if (!failExplosion) { //only update this if the end "explosion" hasn't happened
			scoreText.text = "Blocks destroyed: " + points + "/150 "; //update the score text
			if (points >= 150) { //won the game
				Spaceship.speedOfShip += 1f; //survived a planet and got a ship part as a reward that helps make the ship's motor stronger
				SceneManager.LoadScene ("Starmap"); //change scene
			}
			if (gemWasMovedButNoMatches) { //a failed move happened
				failedMovesLeft--;
				gemWasMovedButNoMatches = false;
			}
			failMoveText.text = "Moves that can fail: " + failedMovesLeft + " "; //update the amount of failed moves

			if (failedMovesLeft == 0) { //lost the game
				StarmapGameController.rotationSpeedBlackhole += 50f; //set blackhole to rotate faster
				StarmapGameController.chanceToLoseByGettingSuckedIntoABlackhole += 5; //raises the chance to lose by getting sucked into a blackhole by 5%
				StarmapGameController.blackholeSize = new Vector3 (StarmapGameController.blackholeSize.x + 0.25f, StarmapGameController.blackholeSize.y + 0.25f, 1f); //change the size of the blackhole
				Spaceship.lostLastGame = true;
				foreach (Gem gem in gems) {
					gem.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None; //unlocks y-axis lock that is set from unity and only locks rotation z-axis
					//adds a random force to each gem to make the hole game field explode at the end (fun ending animation)
					gem.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (-5f, 5f), Random.Range (10f, 50f)), ForceMode2D.Impulse);
					textYouLost.text = "Out of moves!"; //write "A mine was triggered :(" to the screen
					buttonContinue.SetActive (true); //set the button the be active and be shown to the player
					buttonQuit.SetActive (true);
				}
				failExplosion = true;
			}
		}
	}

	/// <summary>
	/// Game was lost and either continue or quit button was clicked.
	/// </summary>
	/// <param name="button">"continue" or "quit" button clicked</param>
	private void gameLostButtonClicked (string button) { //a method that is ran when a button is clicked
		if (button.Equals ("continue")) {
			SceneManager.LoadScene ("Starmap"); //go back to starmap
		} else {
			Application.Quit (); //quit the game
		}
	}
}
