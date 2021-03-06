﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(GoatBack))]
public class GoatController : MonoBehaviour
{
	[SerializeField] private float          _speed = 1f;
	[SerializeField] private GameObject     _coneParent;
	[SerializeField] private Collider2D     _closeCollider;
	[SerializeField] private Cone           _farCone;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Animator       _animator;

	[SerializeField, Range(0, 1)] private float _rareChance;


	[Header("Sfx")]
	[SerializeField] private List<AudioClip> _screamClips;
	[SerializeField] private List<AudioClip> _rareScreamClips;
	[SerializeField] private AudioSource     _source;


	private Rigidbody2D _rigidbody2D;

	private List<Collider2D> _closeScreamResults = new List<Collider2D>();
	private List<Collider2D> _farScreamResults   = new List<Collider2D>();

	private bool _previousScream;

	private readonly HashSet<GoatKid> _responded = new HashSet<GoatKid>();

	private static readonly int BHolding   = Animator.StringToHash("bHolding");
	private static readonly int BScreaming = Animator.StringToHash("bScreaming");

	private static readonly ContactFilter2D _contactFilter2D = new ContactFilter2D()
															   {
																	   useTriggers = true
															   };

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
		var rotation = _coneParent.transform.rotation;

		var hold = false;
		var pitch = Input.GetAxisRaw("Pitch");
		var roll = Input.GetAxisRaw("Roll");
		var mousePos = Vector3.zero;
		if (Mathf.Abs(pitch) > 0 || Mathf.Abs(roll) > 0)
		{
			var waveDirection = new Vector2(roll, pitch);
			
			mousePos = mainCamera.ViewportToWorldPoint(new Vector3(
					roll.Remap(-1,1, 1, 0), 
					pitch.Remap(-1, 1, 1, 0)));
			Debug.Log(new Vector3(roll, pitch, waveDirection.magnitude));
			Debug.Log(mousePos);

			hold = waveDirection.magnitude > 0.9f;
		}
		else
		{
			mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

			hold = Input.GetAxis("Fire1") > 0;
		}

		var diff = transform.position - mousePos;

		float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		rotation = Quaternion.Euler(0f, 0f, rotZ + 90);

		var animatorState = _animator.GetCurrentAnimatorStateInfo(0);
		var canScream = !_source.isPlaying && (animatorState.IsName("Idle") || _animator.GetBool(BHolding));

		if (canScream) _coneParent.transform.rotation = rotation;

		// start holding
		if (!_previousScream && hold && canScream)
		{
			_responded.Clear();
			_animator.SetBool(BHolding, true);
		}

		// let go
		if (_previousScream && !hold && _animator.GetBool(BHolding))
		{
			DoScream();
		}

		if (!hold)
		{
			_animator.SetBool(BHolding, false);
		}

		if (!_source.isPlaying)
		{
			_animator.SetBool(BScreaming, false);
		}
		else
		{
			// new kids coming in can respond while scream is going on

			var camFrustum = GeometryUtility.CalculateFrustumPlanes(mainCamera);
			Physics2D.OverlapCollider(_closeCollider, _contactFilter2D, _closeScreamResults);
			_farCone.GetOverlap(_farScreamResults);

			var goatKidsToAppear = _closeScreamResults.Select(r => r.gameObject.GetComponent<GoatKid>())
													  .Where(g => g != null)
													  .Except(_responded);

			var goatKidsToRespond = _farScreamResults.Select(r => r.gameObject.GetComponent<GoatKid>())
													 .Where(g => g != null)
													 .Except(_responded)
													 .Except(goatKidsToAppear);

			foreach (var goatKid in goatKidsToAppear)
			{
				_responded.Add(goatKid);
				goatKid.Appear(this);
			}

			foreach (var goatKid in goatKidsToRespond)
			{
				_responded.Add(goatKid);
				goatKid.RespondToParent(this, camFrustum, mainCamera);
			}
		}

		_previousScream = hold;

		_spriteRenderer.sortingOrder = (int) (_spriteRenderer.bounds.min).y * -1;
		for (int i = 0; i < Back.GoatKids.Count; i++)
		{
			var kid = Back.GoatKids.ElementAt(i);
			kid.SpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder + i + 1;
		}
	}

	private void DoScream()
	{
		_animator.SetBool(BScreaming, true);
		PlayScream();
	}

	private void PlayScream()
	{
		_source.Stop();
		var rare = Random.value > (1 - _rareChance);
		var randomList = rare ? _rareScreamClips : _screamClips;
		var i = Random.Range(0, randomList.Count);
		var clip = randomList[i];
		_source.clip = clip;
		_source.Play();
	}
}