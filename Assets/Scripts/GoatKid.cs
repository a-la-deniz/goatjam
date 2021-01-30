using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class GoatKid : MonoBehaviour
{
	public enum GoatKidState
	{
		Hiding,
		JumpingOut,
		Out,
		OutHadNoSpace,
		JumpingOnMother,
		OnMother,
		OnPen
	}

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

	private Bush _bush;
	private float _bushOffsetZ = 2f;
	private float _bushJumoOffsetY = 2f;

	private GoatController _mamaGoat;
	private GoatKidState _state = GoatKidState.Hiding;

	private object _tweenId = new object();

	private void Awake()
	{
		_collider = GetComponent<Collider2D>();
	}

	private void OnDestroy()
	{
		_mamaGoat = null;
		DOTween.Kill(_tweenId);
		_tweenId = null;
	}

	/// <summary>
	/// Respond by screaming
	/// </summary>AS
	public void RespondToParent(GoatController mamaGoat, Plane[] cameraFrustum, Camera mainCamera)
	{
		if (_state != GoatKidState.Hiding && _state != GoatKidState.OutHadNoSpace) return;

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
			var responseOnViewport = new Vector3(responseOnClipX.Remap(-1, 1, 0, 1),
					responseOnClipY.Remap(-1, 1, 0, 1),
					kidOnViewport.z);

			var responseOnWorld = mainCamera.ViewportToWorldPoint(responseOnViewport);

			// TODO: visualize response on responseOnWorld position
		}

		// Propagate Visual
	}


	/// <summary>
	/// Stop hiding behind bush, etc.
	/// </summary>
	public void Appear(GoatController mamaGoat)
	{
		_mamaGoat = mamaGoat;

		if (_state == GoatKidState.Hiding)
		{
			Debug.Log($"{this} jump from bush", this);
			
			// Play animation
			JumpFromBush();
		}
		else if (_state == GoatKidState.OutHadNoSpace)
		{
			JumpOnMother();
		}
	}

	public void Detach(GoatPen pen)
	{
		// TODO Move to certain defined positions on the pen or something
		var randomOffset = new Vector3(Random.Range(1f, 3f), Random.Range(1f, 3f), 0f);
		transform.position = pen.transform.position + randomOffset;
		transform.parent = null;

		pen.ReturnGoat();
		_state = GoatKidState.OnPen;
	}

	public void HideInBush(Bush bush)
	{
		_bush = bush;
		var pos = transform.position;
		pos.z = _bushOffsetZ + _bushOffsetZ;
		transform.position = pos;
	}

	public void JumpFromBush()
	{
		_state = GoatKidState.JumpingOut;

		DOTween.Kill(_tweenId);
		DOTween.Sequence()
			.Append(transform.DOMoveY(_bushJumoOffsetY, 0.2f).SetRelative())
			.AppendCallback(() =>
			{
				var pos = transform.position;
				pos.z = _bushOffsetZ - _bushOffsetZ;
				transform.position = pos;
			})
			.Append(transform.DOMoveY(-_bushJumoOffsetY, 0.2f).SetRelative())
			.OnComplete(OnOutsideBush)
			.SetId(_tweenId);
	}

	private void OnOutsideBush()
	{
		Debug.Log($"{this} outside the bush", this);

		_bush = null;
		_state = GoatKidState.Out;

		JumpOnMother();
	}

	private void JumpOnMother()
	{
		if (_state != GoatKidState.Out) return;

		// Jump on parent
		if (!_mamaGoat.Back.HasSpace)
		{
			_state = GoatKidState.OutHadNoSpace;
			Debug.Log($"{this} can't jump on mama goat, no room", this);
			return;
		}

		_state = GoatKidState.JumpingOnMother;

		transform.position = _mamaGoat.Back.TopAttachPoint.position;
		transform.parent = _mamaGoat.transform;

		DOTween.Kill(_tweenId);
		DOTween.Sequence()
			.Append(transform.DOMove(_mamaGoat.Back.TopAttachPoint.position, 0.25f))
			.OnComplete(OnMother)
			.SetId(_tweenId);

		_mamaGoat.Back.Attach(this);
		_mamaGoat = null;
		_collider.enabled = false;
	}

	private void OnMother()
	{
		Debug.Log($"{this} on the mother", this);
		_state = GoatKidState.OnMother;
	}
}