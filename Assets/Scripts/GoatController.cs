using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GoatBack))]
public class GoatController : MonoBehaviour
{
	[SerializeField] private float          _speed = 1f;
	[SerializeField] private GameObject     _coneParent;
	[SerializeField] private Cone           _closeCone;
	[SerializeField] private Cone           _farCone;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Animator       _animator;


	[Header("Sfx")]
	[SerializeField] private AudioClip _scream;
	[SerializeField] private AudioSource _source;


	private Rigidbody2D _rigidbody2D;

	private List<Collider2D> _closeScreamResults = new List<Collider2D>();
	private List<Collider2D> _farScreamResults   = new List<Collider2D>();

	private                 bool _previousScream;
	private static readonly int  BHolding = Animator.StringToHash("bHolding");
	private static readonly int BScreaming = Animator.StringToHash("bScreaming");

	public GoatBack Back { get; private set; }

	private void Awake()
	{
		Back = GetComponent<GoatBack>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		_rigidbody2D.velocity = direction.normalized * _speed;

		var mainCamera = Camera.main;
		var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		var diff = transform.position - mousePos;

		float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		var rotation = Quaternion.Euler(0f, 0f, rotZ + 90);

		_coneParent.transform.rotation = rotation;

		var hold = Input.GetAxis("Fire1") > 0;

		// start holding
		if (!_previousScream && hold && !_source.isPlaying)
		{
			_animator.SetBool(BHolding, true);
		}

		// let go
		if (_previousScream && !hold && _animator.GetBool(BHolding))
		{
			DoScream(mainCamera);
		}

		if (!hold)
		{
			_animator.SetBool(BHolding, false);
		}
		if (!_source.isPlaying)
		{
			_animator.SetBool(BScreaming, false);
		}

		_previousScream = hold;

		_spriteRenderer.sortingOrder = (int) (_spriteRenderer.bounds.min).y * -1;
		foreach (var backGoatKid in Back.GoatKids)
		{
			backGoatKid.SpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder;
		}
	}

	private void DoScream(Camera mainCamera)
	{
		_animator.SetBool(BScreaming, true);

		var camFrustum = GeometryUtility.CalculateFrustumPlanes(mainCamera);
		_closeCone.GetOverlap(_closeScreamResults);
		_farCone.GetOverlap(_farScreamResults);

		var goatKidsToAppear = _closeScreamResults.Select(r => r.gameObject.GetComponent<GoatKid>())
												  .Where(g => g != null);

		var goatKidsToRespond = _farScreamResults.Select(r => r.gameObject.GetComponent<GoatKid>())
												 .Where(g => g != null)
												 .Except(goatKidsToAppear);

		foreach (var goatKid in goatKidsToAppear)
		{
			goatKid.Appear(this);
		}

		foreach (var goatKid in goatKidsToRespond)
		{
			goatKid.RespondToParent(this, camFrustum, mainCamera);
		}

		PlayScream();
	}

	private void PlayScream()
	{
		_source.Stop();
		_source.clip = _scream;
		_source.Play();
	}
}