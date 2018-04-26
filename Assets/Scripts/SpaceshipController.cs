using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Spaceship controller class that checks if it collides with asteroids and plays sound/particlefs when that happens.
/// </summary>
public class SpaceshipController : MonoBehaviour {

	private int health;
	private Text shipHealthText;
	private ParticleSystem particleExplosion;
	private AudioSource audioSource;
	public AudioClip blastSound;

	/// <summary>
	/// Ran once at the start of the instance.
	/// </summary>
	void Start () {
		//find the audio source that is created once at the start of the game and not destroyed on load
		audioSource = FindObjectOfType<AudioSource> (); 
		this.health = 3; //amount of hits the spaceship can take
		particleExplosion = GameObject.Find ("ParticleExplosion").GetComponent<ParticleSystem>();
		shipHealthText = GameObject.Find ("ShipHealthText").GetComponent<Text> ();
		shipHealthText.text = "Ship health: " + health;
		particleExplosion.Stop (); //Makes it so that the explosion won't happen at the start of the game.
	}

	/// <summary>
	/// When the spaceship collides with an asteroid a sound is played and explosion particles are shown. 
	/// Also health is lowered by one and if the health goes to 0 the spaceship is destroyed.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter2D(Collision2D collision) { //happens when spaceship collides with something
		audioSource.PlayOneShot (blastSound);
		//stop at the start so that if a previous explosion animation is still going it will stopped and another explosion is created.
		particleExplosion.Stop(); 
		Destroy(collision.gameObject); //destroy the asteroid that hits the ship
		particleExplosion.Play(); //show explosion particles
		this.health--;
		shipHealthText.text = "Ship health: " + health;
		if (this.health == 0) {
			Destroy (gameObject); //destroy the ship when health is 0
		}
	}
}
