using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Planet class that keeps track of the name of the planet and the planets next to it.
/// </summary>
public class Planet {
	private string planetName;
	private string planetPictureName;
	private Planet nextPlanet;
	private Planet previousPlanet;

	/// <summary>
	/// Initializes a new instance of the <see cref="Planet"/> class.
	/// </summary>
	/// <param name="planetName">Planet name.</param>
	/// <param name="planetPictureName">Planet picture name.</param>
	public Planet(string planetName, string planetPictureName) {
		this.planetName = planetName;
		this.planetPictureName = planetPictureName;
	}

	/// <summary>
	/// Sets the next planet.
	/// </summary>
	/// <param name="nextPlanet">Next planet.</param>
	public void SetNextPlanet(Planet nextPlanet){
		this.nextPlanet = nextPlanet;
	}

	/// <summary>
	/// Sets the previous planet.
	/// </summary>
	/// <param name="previousPlanet">Previous planet.</param>
	public void SetPreviousPlanet(Planet previousPlanet){
		this.previousPlanet = previousPlanet;
	}

	/// <summary>
	/// Gets the name of the planet.
	/// </summary>
	/// <returns>The planet name.</returns>
	public string GetPlanetName(){
		return this.planetName;
	}

	/// <summary>
	/// Gets the name of the planet picture.
	/// </summary>
	/// <returns>The planet picture name.</returns>
	public string GetPlanetPictureName(){
		return this.planetPictureName;
	}

	/// <summary>
	/// Gets the next planet.
	/// </summary>
	/// <returns>The next planet.</returns>
	public Planet GetNextPlanet(){
		return this.nextPlanet;
	}

	/// <summary>
	/// Gets the previous planet.
	/// </summary>
	/// <returns>The previous planet.</returns>
	public Planet GetPreviousPlanet(){
		return this.previousPlanet;
	}

}
