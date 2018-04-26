using System;

/// <summary>
/// Mine class for minesweeper.
/// </summary>
public class Mine
{
	private int x;
	private int y;

	/// <summary>
	/// Initializes a new instance of the <see cref="Mine"/> class.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Mine (int x, int y)
	{
		this.x = x; //random number from 0 to max size of the game
		this.y = y;
	}

	/// <summary>
	/// Gets the x.
	/// </summary>
	/// <returns>x coord</returns>
	public int GetX() {
		return this.x;
	}

	/// <summary>
	/// Gets the y.
	/// </summary>
	/// <returns>y coordinate</returns>
	public int GetY() {
		return this.y;
	}
}

