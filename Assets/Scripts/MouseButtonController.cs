using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Mouse button controller that checks if the mouse was held down. When the mouse button is held for 20 frames in minesweeper, a mine is marked.
/// </summary>
public class MouseButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private bool buttonPressed = false;
	private int buttonTimer = 0;

	/// <summary>
	/// Called once per frame. Counts frames.
	/// </summary>
	void Update() {
		if (buttonPressed) {
			buttonTimer++; //count every frame the button is held down
		}
	}

	/// <summary>
	/// Raises the pointer down event. Starts counting frames.
	/// </summary>
	/// <param name="e">mouse pointer event e (press down)</param>
	public void OnPointerDown(PointerEventData e) {
		if (!buttonPressed) {
			buttonTimer = 0; //if a new button click the timer is set to 0
		}
		buttonPressed = true;
	}

	/// <summary>
	/// Raises the pointer up event. Stops counting frames.
	/// </summary>
	/// <param name="e">mouse pointer event e (raise the mouse button)</param>
	public void OnPointerUp(PointerEventData e) {
		buttonPressed = false;
	}

	/// <summary>
	/// Gets the button timer.
	/// </summary>
	/// <returns>The button timer.</returns>
	public int GetButtonTimer() {
		return buttonTimer;
	}
}
