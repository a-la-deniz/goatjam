using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatKid : MonoBehaviour
{
	/// <summary>
	/// Respond by screaming
	/// </summary>
	public void RespondToParent()
	{
		// Play SFX
		Debug.Log($"{this} should respond to parent now", this);

		// Propagate Visual
	}

	/// <summary>
	/// Stop hiding behind bush, etc.
	/// </summary>
	public void Appear()
	{
		Debug.Log($"{this} should appear now", this);
		// Play animation

		// Jump on parent
	}
}
