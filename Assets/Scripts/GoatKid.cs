using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GoatKid : MonoBehaviour
{
    /// <summary>
    /// Transform where other goats can pile up
    /// </summary>
    [SerializeField] private Transform _attachmentPoint;

    public Transform AttachmentPoint => _attachmentPoint;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Respond by screaming
    /// </summary>
    public void RespondToParent(GoatController mamaGoat)
	{
		// Play SFX
		Debug.Log($"{this} should respond to parent now", this);

		// Propagate Visual
	}

	/// <summary>
	/// Stop hiding behind bush, etc.
	/// </summary>
	public void Appear(GoatController mamaGoat)
	{
		Debug.Log($"{this} should appear now", this);
		// Play animation

		// Jump on parent
        var back = mamaGoat.GetComponent<GoatBack>();

        // Do as corroutine, tween or something
        // Animate and block mama goat movement until complete
        transform.position = back.TopAttachPoint.position;
        transform.parent = mamaGoat.transform;

        back.Attach(this);
        _collider.enabled = false;
    }

    public void Detach()
    {
        // TODO Move to certain defined positions on the pen or something
        var randomOffset = new Vector3(Random.Range(1f, 3f), Random.Range(1f, 3f), 0f);
        transform.position = transform.parent.position + randomOffset;
        transform.parent = null;
        _collider.enabled = true;
    }
}
