using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

[RequireComponent(typeof(Collider2D))]
public class GoatKid : MonoBehaviour
{
	/// <summary>
	/// Transform where other goats can pile up
	/// </summary>
	[SerializeField] private Transform _attachmentPoint;
	[SerializeField] private SpriteRenderer _spriteRenderer;

	[Header("SFX")]
	[SerializeField] private AudioClip _scream;
	[SerializeField] private AudioSource _source;

	public Transform AttachmentPoint => _attachmentPoint;

	private Collider2D _collider;

	private void Awake()
	{
		_collider = GetComponent<Collider2D>();
	}

	/// <summary>
	/// Respond by screaming
	/// </summary>AS
	public void RespondToParent(GoatController mamaGoat, Plane[] cameraFrustum, Camera mainCamera)
	{
		// Play SFX
		_source.PlayOneShot(_scream);
		if (GeometryUtility.TestPlanesAABB(cameraFrustum, _spriteRenderer.bounds))
		{
			Debug.Log($"{this} should respond to parent now ON SCREEN", this);
		}
		else
		{
			Debug.Log($"{this} should respond to parent now OFF SCREEN", this);
			var kidTransform = transform;
			var kidPosition = kidTransform.position;
			var kidOnViewport = mainCamera.WorldToViewportPoint(kidPosition);
			var kidOnClipX = kidOnViewport.x.Remap(0, 1, -1, 1);
			var kidOnClipY = kidOnViewport.y.Remap(0, 1, -1, 1);
			var edgeScale = 1.0f / Mathf.Max(Mathf.Abs(kidOnClipX), Mathf.Abs(kidOnClipY));
			var responseOnClipX = kidOnClipX * edgeScale;
			var responseOnClipY = kidOnClipY * edgeScale;
			
			ScreamVisualsProvider.CreateScream(responseOnClipX, responseOnClipY, kidOnViewport.z);
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
		if (!back.HasSpace)
		{
			Debug.Log($"{this} can't jump on mama goat, no room", this);
			return;
		}

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