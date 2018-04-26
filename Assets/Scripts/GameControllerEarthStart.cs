using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game controller of the earth start scene.
/// </summary>
public class GameControllerEarthStart : MonoBehaviour {
	private Button buttonSpaceShip;
	private GameObject pinkki;
	private GameObject teardrop;
	private bool clicked; 
	private bool soundEffectOnce;
	private AudioSource audioSource;
	public AudioClip rocketLaunch;

	/// <summary>
	/// Ran at the start of the instance.
	/// </summary>
	void Start () {
		soundEffectOnce = false; //No sound effect
		audioSource = FindObjectOfType<AudioSource> ();
		clicked = false;
		teardrop = GameObject.Find ("Teardrop");
		pinkki = GameObject.Find ("Pinkki");
		buttonSpaceShip = GameObject.Find ("ButtonSpaceship").GetComponent<Button> ();
		buttonSpaceShip.onClick.AddListener (() => spaceshipClicked ());
	}

	/// <summary>
	/// Ran once per frame.
	/// </summary>
	void Update () {
		if (clicked) { //When ButtonSpaceship is clicked
			teardrop.SetActive (false); //Teardrop disappears
			//If Pinkkis position is not in the spaceship, it moves there
			if (pinkki.transform.position.x < 6.4) {
				pinkki.transform.Translate (0.05f, 0, 0); 
			} else {
				//Pinkki disappears when it's behind the spaceship
				pinkki.SetActive (false); 
				//RocketLaunch audio plays once
				if (!soundEffectOnce) {
					audioSource.PlayOneShot (rocketLaunch);
					soundEffectOnce = true;
				}
				//Spaceship leaves the earth
				buttonSpaceShip.transform.Translate (0, 2f, 0); 
				//When spaceship is high enough, move to Starmap scene
				if (buttonSpaceShip.transform.position.y > 800) {
					SceneManager.LoadScene ("Starmap"); 
				} 
			}
		}
	}
		
	/// <summary>
	/// When spaceship is clicked, bool clicked turn true.
	/// </summary>
	private void spaceshipClicked(){ 
		clicked = true; 
	}
}
