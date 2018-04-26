using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explosion sound controller
/// </summary>
public class ExplosionSound : MonoBehaviour {
	public AudioClip crashSoft; //audio clip chosen in unity
	public AudioClip crashHard;
	private AudioSource source;
	private float lowPitchRange = .75F; 
	private float highPitchRange = 1.5F;
	private float velToVol = .2F;
	private float velocityClipSplit = 10F;

	/// <summary>
	/// Ran when the gameobject is created.
	/// </summary>
	void Awake () {
		source = GetComponent<AudioSource>();
	}

	/// <summary>
	/// On collision plays a sound.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter (Collision coll) {
		source.pitch = Random.Range (lowPitchRange,highPitchRange); //randomly changes the pitch of the audio
		float hitVol = coll.relativeVelocity.magnitude * velToVol;
		if (coll.relativeVelocity.magnitude < velocityClipSplit)
			source.PlayOneShot(crashSoft,hitVol);
		else 
			source.PlayOneShot(crashHard,hitVol);
	}

}