using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Class that holds the player name and the score and tells how to sort Scores. 
/// </summary>
public class Score : IComparable<Score> {

	private string name;
	private float result;

	/// <summary>
	/// Initializes a new instance of the <see cref="Score"/> class.
	/// </summary>
	/// <param name="name">name of the player</param>
	/// <param name="result">result time of the player</param>
	public Score(string name, float result) {
		this.name = name;
		this.result = result;
	}

	/// <summary>
	/// Gets the name.
	/// </summary>
	/// <returns>Player's name</returns>
	public string GetName() {
		return this.name;
	}

	/// <summary>
	/// Gets the result.
	/// </summary>
	/// <returns>Player's time</returns>
	public float GetResult() {
		return this.result;
	}
		
	/// <summary>
	/// Compare this result to another result to tell a list later how to sort itself
	/// </summary>
	/// <returns>Returns positive if the score is smaller, negative if the score is bigger and 0 if scores are the same.</returns>
	/// <param name="other">Another Score object</param>
	public int CompareTo(Score other) { 
		return this.result.CompareTo (other.GetResult());
	}
}
