using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Transforming object states into a format that Unity can store and reconstruct later.
[System.Serializable]

/// <summary>
/// Dialogue class
/// </summary>
public class Dialogue {

	public string name; //Characters name

	/// <summary>
	/// The sentences character says.
	/// </summary>
	[TextArea(3, 10)]
	public string[] sentences;
}
