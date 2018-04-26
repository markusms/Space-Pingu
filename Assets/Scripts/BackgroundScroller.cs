using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Background scroller script
/// </summary>
public class BackgroundScroller : MonoBehaviour {

	private float backgroundHeight; //height of the background sprite

	/// <summary>
	/// Ran once at the start of the instance.
	/// </summary>
	void Start () {
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -0.75f); //background's rigidbody is kinematic so only this velocity is what moves it
		backgroundHeight = this.GetComponent<SpriteRenderer> ().bounds.size.y; //get the size y of the background
	}
	
	/// <summary>
	/// Called once per frame.
	/// </summary>
	void Update () {
		//if the background has moved its full size down (meaning the background is not seen by the player anymore) then move it back up so that its scrolled back to be seen by the player
		if (transform.position.y < -backgroundHeight) { 
			this.moveBackground ();
		}
	}

	/// <summary>
	/// Moves the background.
	/// </summary>
	private void moveBackground() {
		Vector2 backgroundMove = new Vector2 (0, backgroundHeight * 2f); //Create a vector that moves the background twice its height upwards
		transform.position = (Vector2)transform.position + backgroundMove; //Moves the background twice its height upwards
	}
}
