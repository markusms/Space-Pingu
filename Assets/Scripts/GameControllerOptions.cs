using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game controller options
/// </summary>
public class GameControllerOptions : MonoBehaviour {
	private Button backButton;

	/// <summary>
	/// Ran at the start of an instance.
	/// </summary>
	void Start () {
		backButton = GameObject.Find ("ButtonBackMain").GetComponent<Button> ();
		backButton.onClick.AddListener (()=> BackClicked ());
		
	}

	/// <summary>
	/// If BackButton is clicked, load MainMenu scene
	/// </summary>
	private void BackClicked(){
		SceneManager.LoadScene ("MainMenu");
	}
}
