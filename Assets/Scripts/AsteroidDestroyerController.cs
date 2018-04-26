using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that destroys asteroids when they hit this asteroid destroyer. Makes it so that there isn't a huge amount of asteroids in the scene when they get out of camera.
/// </summary>
public class AsteroidDestroyerController : MonoBehaviour {

	/// <summary>
	/// On collision destroy the gameobject (asteroid) that collides with this gameobject.
	/// </summary>
	/// <param name="collision">Asteroid that collides with this gameobject</param>
	void OnCollisionEnter2D(Collision2D collision) { 
		Destroy(collision.gameObject); //Destroy the asteroid
	}
}
