using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;

/// <summary>
/// Class that saves and returns the leaderboards.
/// </summary>
public class ScoreKeeper {

	/// <summary>
	/// Method that saves the score to a text file "scores.txt"
	/// </summary>
	/// <param name="name">Name the player gave.</param>
	/// <param name="score">The time it took to finish playing the game.</param>
	public void SaveScore(string name, float score) { //save the score to a file
		//write a new line at the end of a file called scores.txt or if it doesn't exist create the file
		using (TextWriter textWriter = new StreamWriter ("scores.txt", true)) { 
			textWriter.WriteLine (name + "-" + score); //write a new line to the file with name-score
		}
	}

	/// <summary>
	/// Reads the scores.txt file, sorts it and then returns it.
	/// </summary>
	/// <returns>Returns a sorted list of all the scores.</returns>
	public string PrintScores() { //read the saved scores from a file
		string line;
		List<Score> scores = new List<Score> (); //create a new list that saves Score class objects
		using (StreamReader readtext = new StreamReader ("scores.txt")) { //Read file "scores.txt"
			while ((line = readtext.ReadLine ()) != null) { //read as long as the file has lines
				//split the line that was read from the file into a string array of size 2 (using - as the splitting point (name-score))
				string[] scoreSplit = line.Split(new char[]{'-'},2); 
				//create a new score object of class Score, parameters are name and result string converted into double and that is converted into float
				Score score = new Score(scoreSplit[0], (float) Convert.ToDouble(scoreSplit[1])); 
				scores.Add(score); //add the score to the scores list
			}
		}
		scores.Sort (); //sort using the CompareTo method inside Score class so that the lowest score (fastest time) is first
		int positionOnTheLeaderboard = 1; //variable to show what the position of the player is on the leaderboard
		StringBuilder scoreBoard = new StringBuilder(); //create a string that can have all the scores appended into it
		foreach (Score score in scores) { //iterate through the scores list
			if (positionOnTheLeaderboard > 10) { //max 10 results
				break;
			} else if (positionOnTheLeaderboard == 10) { //if the 10th result then don't add the empty spaces "  " to the start of the string to make the leaderboard look better
				//Create a string of the player name and the time
				scoreBoard.Append (positionOnTheLeaderboard).Append (". ").Append (score.GetName ()).Append (" - ").Append (score.GetResult ().ToString("0.00")).Append ("s\n");
				break;
			}
			//Create a string of the player name and the time
			scoreBoard.Append ("  ").Append (positionOnTheLeaderboard).Append (". ").Append (score.GetName ()).Append (" - ").Append (score.GetResult ().ToString("0.00")).Append ("s\n");
			positionOnTheLeaderboard++;
		}
		while (positionOnTheLeaderboard < 10) {//make the list always 10 results long to make it look better
			if (positionOnTheLeaderboard == 9) { //if the 10th result then don't add the empty spaces "  " to the start of the string to make the leaderboard look better
				scoreBoard.Append (positionOnTheLeaderboard).Append (".                    \n");
				break;
			}
			scoreBoard.Append ("  ").Append (positionOnTheLeaderboard).Append (".                    \n");
			positionOnTheLeaderboard++;
		}
		return scoreBoard.ToString (); //return the stringbuilder converted into a string
	}
}
