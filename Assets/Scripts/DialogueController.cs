using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dialogue controller
/// </summary>
public class DialogueController : MonoBehaviour {

	public Text nameText; //Characters name in dialogue box
	public Text dialogueText; //Dialogue text
	private GameObject startButton; //Button that starts dialogue

	public Animator animator; //Dialogue box animates in and out of sight

	private Queue<string> sentences; //Queue of the sentences character says

	/// <summary>
	/// Ran at the start of the instance.
	/// </summary>
	void Start () {
		startButton = GameObject.Find ("StartButton"); //Creating the startbutton
		sentences = new Queue<string> (); //Creating the queue of sentences
	}

	/// <summary>
	/// Starts the dialogue.
	/// </summary>
	/// <param name="dialogue">Dialogue.</param>
	public void StartDialogue (Dialogue dialogue)
	{
		startButton.SetActive (false); //Startbutton disappears when dialogue starts
		animator.SetBool ("IsOpen", true); //Dialoguebox is shown
		nameText.text = dialogue.name;
		sentences.Clear (); //Goes through all sentences in sentences list


		//Adds sentences in to queue called sentences
		foreach (string sentence in dialogue.sentences) {
			sentences.Enqueue (sentence);
		}

		DisplayNextSentence (); //Displays next sentence
	}

	/// <summary>
	/// Displays the next sentence.
	/// </summary>
	public void DisplayNextSentence()
	{
		//When list of sentences is empty, execute EndDialogue method
		if (sentences.Count == 0) {
			EndDialogue ();
			return; 
		}

		//Stops all coroutines and starts typing the sentence
		string sentence = sentences.Dequeue ();
		StopAllCoroutines ();
		StartCoroutine (TypeSentence (sentence));
	}

	/// <summary>
	/// Types the sentence letter by letter.
	/// </summary>
	/// <returns>The sentence.</returns>
	/// <param name="sentence">Sentence.</param>
	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			dialogueText.text += letter;
			yield return null;
		}
	}
	/// <summary>
	/// Ends the dialogue.
	/// </summary>
	void EndDialogue(){
		animator.SetBool ("IsOpen", false); //Dialogue box leaving sight
		//Execute StartFight method from GameControllerPlanet
		FindObjectOfType<GameControllerPlanet> ().StartFight (); 

	}


}
