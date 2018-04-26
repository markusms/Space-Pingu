using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Asteroid controller that creates asteroids.
/// </summary>
public class AsteroidController : MonoBehaviour {

	public float time = 0.5f; //asteroid creation time
	public GameObject asteroid; //prefab asteroid set from Unity

	/// <summary>
	/// Ran at the start of this instance.
	/// </summary>
	void Start () {
		//keeps calling the method CreateAsteroid after time has passed
		InvokeRepeating ("CreateAsteroid", time, time*4.5f); 
	}

	/// <summary>
	/// Clones an asteroid.
	/// </summary>
	private void CreateAsteroid () {
		//creates a clone of the asteroid in a random position and rotates it
		Instantiate (asteroid, new Vector2 (Random.Range (-8, 8), 8), Quaternion.identity); 
	}

	/// <summary>
	/// Stop calling the CreateAsteroid() method
	/// </summary>
	public void stopCreatingAsteroids() {
		CancelInvoke ();
	}

}
