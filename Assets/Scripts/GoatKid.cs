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
    [SerializeField] private SpriteRenderer _spriteRenderer;

	public Transform AttachmentPoint => _attachmentPoint;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

	/// <summary>
	/// Respond by screaming
	/// </summary>AS
	public void RespondToParent(GoatController mamaGoat, Plane[] cameraFrustum)
	{
		// Play SFX
		if (GeometryUtility.TestPlanesAABB(cameraFrustum, _spriteRenderer.bounds))
		{
			Debug.Log($"{this} should respond to parent now ON SCREEN", this);
		}
		else
		{
			Debug.Log($"{this} should respond to parent now OFF SCREEN", this);
		}

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

    public void Detach(GoatPen pen)
    {
        // TODO Move to certain defined positions on the pen or something
        var randomOffset = new Vector3(Random.Range(1f, 3f), Random.Range(1f, 3f), 0f);
        transform.position = pen.transform.position + randomOffset;
        transform.parent = null;

        pen.ReturnGoat();
    }
}
