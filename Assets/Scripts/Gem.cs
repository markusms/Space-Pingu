using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class Gem
/// </summary>
public class Gem : MonoBehaviour {

	private List<Gem> gemsNear = new List<Gem>(); //list of gems near the game found out with Physics2D.OverlapCircleAll
	private Collider2D[] hitColliders; //the colliders that Physics2D.OverlapCircleAll touches
	private float radius = 0.6f; //radius of the Physics2D.OverlapCircleAll
	private bool selected = false; //is the gem selected
	private Vector3 position; //position of this gem

	/// <summary>
	/// Chooses a color for the gem.
	/// </summary>
	void Start () {
		int color = Random.Range (1, 5); //randomly choose a color for the gem
		if (color == 1) { //red
			this.GetComponent<SpriteRenderer> ().color = Color.red;
			this.tag = "red";
		} else if (color == 2) { //blue
			this.GetComponent<SpriteRenderer> ().color = Color.blue;
			this.tag = "blue";
		} else if (color == 3) { //green
			this.GetComponent<SpriteRenderer> ().color = Color.green;
			this.tag = "green";
		} else { //yellow
			this.GetComponent<SpriteRenderer> ().color = Color.yellow;
			this.tag = "yellow";
		}
	}

	/// <summary>
	/// Method called every frame.
	/// </summary>
	void Update() {
		if (!selected) { //gameobject is not clicked
			this.transform.GetChild (0).GetComponent<ParticleSystem> ().Stop (); //stop playing selection particles
		} else { //gameobject is clicked
			if (this.transform.GetChild (0).GetComponent<ParticleSystem> ().isStopped) { //only Play it once, no need to try and play every frame
				this.transform.GetChild (0).GetComponent<ParticleSystem> ().Play (); //play the highlight particles
			}
		}
	}

	/// <summary>
	/// Method called every time physics are calculated (can happen more than once per frame). Physics related things in FixedUpdate.
	/// </summary>
	void FixedUpdate () { 
		position = this.transform.position; //save the position of the gameobject here
		//We cast a circle in the middle of the gameobject and every collider that is hit by the circle is saved into the hitColliders array. 
		//Basically we use this to check if there are objects over, under, left and right side of the gameobject. 
		//It also adds itself because the gameobject itself has a collider.
		hitColliders = Physics2D.OverlapCircleAll (transform.position, radius); 
		foreach (Collider2D collider in hitColliders) { //go through all the gameobjects that we collide with
			//we save all the gameobjects that are next to us that is not the gameobject itself to a list gemsNear
			if (!gemsNear.Contains (collider.gameObject.GetComponent<Gem>()) && collider.gameObject.GetComponent<Gem>() != gameObject.GetComponent<Gem> ()) { 
				gemsNear.Add (collider.gameObject.GetComponent<Gem> ()); //add the gem to the list of gems next to us
			}
		}
		//Check if a gem is still next to it. gemsNear is 1 less because hit colliders detects the gameobject itself too not just others next to it.
		if (hitColliders.Length - 1 != gemsNear.Count) { 
			bool gemFound; //variable to save if a gem is still near us and it didn't move/get destroyed...
			foreach (Gem gem in gemsNear.Reverse<Gem>()) { //iterate through reverse list to be able to remove gameobject while iterating
				gemFound = false;
				foreach (Collider2D collider in hitColliders) {
					if (collider.gameObject.GetComponent<Gem> () == gem) { //the gem is still near us
						gemFound = true;
					}
				}
				if (gemFound == false) { //gem is not near the gameobject anymore so it is removed from the list
					gemsNear.Remove (gem);
				}
			}
		}
	}

	/// <summary>
	/// Return a list of gems near us.
	/// </summary>
	/// <returns>The gems near</returns>
	public List<Gem> GetGemsNear() {
		return gemsNear;
	}

	/// <summary>
	/// Return a list of neighbours with same color.
	/// </summary>
	/// <returns>The gems near with same color.</returns>
	public List<Gem> GetGemsNearWithSameColor() {
		List<Gem> gemsNearWithSameColor = new List<Gem> (); //create a new list of gems with same color that are next to us
		foreach (Gem gem in gemsNear) { //iterate through the list of gems near us
			if (gem != null) {
				if (gem.tag == this.tag) {
					gemsNearWithSameColor.Add (gem); //the gem was same color so we add it to the list
				}
			}
		}
		return gemsNearWithSameColor;
	}

	/// <summary>
	/// Returns if the gem is selected.
	/// </summary>
	/// <returns><c>true</c>, if selected <c>false</c> otherwise.</returns>
	public bool GetSelected() {
		return selected;
	}

	/// <summary>
	/// Toggle selection false/true
	/// </summary>
	public void SetSelected() {
		selected = (!selected);  //toggle selection
	}

	//return the position of this gem.
	public Vector3 GetPosition() {
		return this.position;
	}

	/// <summary>
	/// Returns if the parameter gem is next to the gem.
	/// </summary>
	/// <returns><c>true</c> if the gem is next to us and otherwise <c>false</c>.</returns>
	/// <param name="gem">Gem.</param>
	public bool IsNextToYou(Gem gem) {
		return gemsNear.Contains (gem);
	}

	/// <summary>
	/// When the gem is clicked. Toggle selection. (works because the gem has a box collider)
	/// </summary>
	void OnMouseDown() {
		selected = (!selected); //toggle selection on/off when clicked
	}
}
