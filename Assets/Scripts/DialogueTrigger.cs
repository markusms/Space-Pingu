using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dialogue trigger class
/// </summary>
public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue; //Creating dialogue

	/// <summary>
	/// Triggers the dialogue to start.
	/// </summary>
	public void TriggerDialogue()
	{
		//Execute StartDialogue method from DialogueController
		FindObjectOfType<DialogueController> ().StartDialogue (dialogue);
	}
}
