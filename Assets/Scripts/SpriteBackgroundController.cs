using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sprite background controller. Checks if the player clicks on the background of the leaderboard after scores are shown and then throws him to the main menu.
/// </summary>
public class SpriteBackgroundController : MonoBehaviour {

	private bool nameWrittenToLeaderboards; //boolean that checks if the player has written his name to the leaderboards

	/// <summary>
	/// Run at the start of this instance.
	/// </summary>
	void Start () {
		nameWrittenToLeaderboards = false;
	}

	/// <summary>
	/// When the backkground is clicked. Background has a box collider so clicks will be noticed.
	/// </summary>
	void OnMouseDown() { 
		if (nameWrittenToLeaderboards) { //name has been written so we can move to main menu on mouse click
			SceneManager.LoadScene ("MainMenu");
		}
	}

	/// <summary>
	/// Sets the boolean nameWrittenToLeaderboards to true. 
	/// </summary>
	public void SetNameWrittenToLeaderboards() { //when the player written his name in the leaderboards this method is called and the boolean is set to true
		nameWrittenToLeaderboards = true;
	}
}
