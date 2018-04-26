using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Background music class
/// </summary>
public class BackgroundMusic: MonoBehaviour {

	/// <summary>
	/// When the gameobject is created.
	/// </summary>
	void Awake() {
		//This gameobject does not get destroyed when a new scene is loaded so the background music keeps playing from scene to another scene.
		//Makes it so that the audio source (background music) attached to this same gameobject won't stop on scene load
		DontDestroyOnLoad (transform.gameObject); 
	}

	/// <summary>
	/// Run at the start of the instance.
	/// </summary>
	void Start() {
		SceneManager.LoadScene ("MainMenu");
	}
}
