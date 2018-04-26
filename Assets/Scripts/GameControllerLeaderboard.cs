using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game controller leaderboard.
/// </summary>
public class GameControllerLeaderboard : MonoBehaviour {

	private Text textScores;
	private Text textScoresTitle;
	private InputField inputFieldName;
	private GameObject spriteBackground;
	private float time; 

	/// <summary>
	/// Method ran at the start of the creation.
	/// </summary>
	void Start () {
		time = Time.time; //the time that it took for the player to finish the game from the first click on the first scene (clicking on the asteroid)
		textScoresTitle = GameObject.Find ("TextScoresTitle").GetComponent<Text> ();
		textScores = GameObject.Find ("TextScores").GetComponent<Text> ();
		spriteBackground = GameObject.Find ("SpriteBackground");
		inputFieldName = GameObject.Find ("InputFieldName").GetComponent<InputField> ();
		inputFieldName.onEndEdit.AddListener (nameWritten); //input field listener that is called when the player has finished writing to the input field by clicking enter
	}

	/// <summary>
	/// Method called when the player has written their name. Creates and shows the leaderboard scores.
	/// </summary>
	/// <param name="name">Name.</param>
	private void nameWritten(string name) {
		//set SetNameWrittenToLeaderboards boolean to true to allow for mouse click on the background to move back to main menu after leaderboards are shown
		spriteBackground.GetComponent<SpriteBackgroundController> ().SetNameWrittenToLeaderboards ();
		ScoreKeeper scores = new ScoreKeeper (); //Create new instance of ScoreKeeper class
		scores.SaveScore (name, time); //save the player name and score
		GameObject.Find ("InputFieldName").SetActive (false); //have to be searched for again because only GameObject can be set inactive not the component InputField
		textScoresTitle.text = "Speedrun champions";
		textScores.text = scores.PrintScores (); //set the leaderboard results as the text
	}
}
