using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Mine settings controller that gets the settings for minesweeper.
/// </summary>
public class MineSettingsController : MonoBehaviour {

	public static int amountOfMines; //static variable that is used in the "Minesweeper" scene
	public static int gameSizeX; //static variable that is used in the "Minesweeper" scene
	public static int gameSizeY; //static variable that is used in the "Minesweeper" scene
	private Button buttonGameSizeXUp;
	private Button buttonGameSizeXDown;
	private Button buttonGameSizeYUp;
	private Button buttonGameSizeYDown;
	private Button buttonAmountOfMinesUp;
	private Button buttonAmountOfMinesDown;
	private Button buttonPlay;
	private Text textGameSizeX; 
	private Text textGameSizeY; 
	private Text textAmountOfMines; 
	private Text textError;

	/// <summary>
	/// Ran at the start of the instance.
	/// </summary>
	void Start () {
		amountOfMines = 30; //default amount of mines
		gameSizeX = 20; //default game size x-axis
		gameSizeY = 20; //default game size y-axis
		buttonGameSizeXUp = GameObject.Find("ButtonGameSizeXUp").GetComponent<Button>();
		buttonGameSizeXDown = GameObject.Find("ButtonGameSizeXDown").GetComponent<Button>();
		buttonGameSizeYUp = GameObject.Find("ButtonGameSizeYUp").GetComponent<Button>();
		buttonGameSizeYDown = GameObject.Find("ButtonGameSizeYDown").GetComponent<Button>();
		buttonAmountOfMinesUp = GameObject.Find("ButtonAmountOfMinesUp").GetComponent<Button>();
		buttonAmountOfMinesDown = GameObject.Find("ButtonAmountOfMinesDown").GetComponent<Button>();
		buttonPlay = GameObject.Find("ButtonPlay").GetComponent<Button>();
		textGameSizeX = GameObject.Find("TextGameSizeX").GetComponent<Text>();
		textGameSizeY = GameObject.Find("TextGameSizeY").GetComponent<Text>();
		textAmountOfMines = GameObject.Find("TextAmountOfMines").GetComponent<Text>();
		textError = GameObject.Find ("TextError").GetComponent<Text> ();

		buttonGameSizeXUp.onClick.AddListener (() => buttonListener ("xUp"));
		buttonGameSizeXDown.onClick.AddListener (() => buttonListener ("xDown"));
		buttonGameSizeYUp.onClick.AddListener (() => buttonListener ("yUp"));
		buttonGameSizeYDown.onClick.AddListener (() => buttonListener ("yDown"));
		buttonAmountOfMinesUp.onClick.AddListener (() => buttonListener ("minesUp"));
		buttonAmountOfMinesDown.onClick.AddListener (() => buttonListener ("minesDown"));
		buttonPlay.onClick.AddListener (() => buttonListener ("Play"));
	}

	/// <summary>
	/// Listener method for when the buttons are clicked. Chooses the size of the game field and the amount of mines. Also starts minesweeper.
	/// </summary>
	/// <param name="button">Button.</param>
	private void buttonListener(string button) {
		if (button.Equals ("xUp")) {//if gameSizeX button was clicked up
			if (gameSizeX < 33) {
				gameSizeX += 2;
			}
			textGameSizeX.text = "Game size X: " + gameSizeX;
		} else if (button.Equals ("xDown")) {
			if (gameSizeX > 2) { //do not let the game size go too small
				gameSizeX -= 2;
			}
			textGameSizeX.text = "Game size X: " + gameSizeX;
		} else if (button.Equals ("yUp")) {
			if (gameSizeY < 33) {
				gameSizeY += 2;
			}
			textGameSizeY.text = "Game size Y: " + gameSizeY;
		} else if (button.Equals ("yDown")) {
			if (gameSizeY > 2) {
				gameSizeY -= 2;
			}
			textGameSizeY.text = "Game size Y: " + gameSizeY;
		} else if (button.Equals ("minesUp")) {
			if (amountOfMines < gameSizeX*gameSizeY-2) { //maximum amount of mines is the size of the gamefield-2 (it would be really stupid to make a game like this but we'll allow it)
				amountOfMines += 2;
			}
			textAmountOfMines.text = "Amount of mines: " + amountOfMines;
		} else if (button.Equals ("minesDown")) {
			if (amountOfMines > 2) {
				amountOfMines -= 2;
			}
			textAmountOfMines.text = "Amount of mines: " + amountOfMines;
		} else if (button.Equals ("Play")) {
			if (gameSizeX*gameSizeY >= 25 && amountOfMines >= 10) { //won't let the player start the game if it's too easy
				SceneManager.LoadScene ("Minesweeper");
			} 
		}

		if (gameSizeX * gameSizeY >= 25 && amountOfMines >= 10) { //start by clearing the error messasge if it is not needed anymore
			textError.text = "";
		} else {
			if (gameSizeX * gameSizeY < 25 && amountOfMines < 10) {
				textError.text = "The game field is too small and there aren't enough mines.";
			} else if (amountOfMines < 10) {
				textError.text = "There aren't enough mines.";
			} else {
				textError.text = "The game field is too small.";
			}
		}
	}
}
