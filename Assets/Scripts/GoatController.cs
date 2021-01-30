﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoatController : MonoBehaviour
{
	[SerializeField] private float _speed = 1f;
	[SerializeField] private Cone  _closeCone;
	[SerializeField] private Cone  _farCone;

	private Rigidbody2D _rigidbody2D;

	private List<Collider2D> _closeScreamResults = new List<Collider2D>();
	private List<Collider2D> _farScreamResults = new List<Collider2D>();

	private bool _previousScream;

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		_rigidbody2D.velocity = direction.normalized * _speed;

		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var diff = transform.position - mousePos;

		float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		var rotation = Quaternion.Euler(0f, 0f, rotZ + 90);

		_closeCone.transform.rotation = rotation;
		_farCone.transform.rotation = rotation;

		var scream = Input.GetAxis("Fire1")> 0;
		if (!_previousScream && scream)
		{
			_closeCone.GetOverlap(_closeScreamResults);
			_farCone.GetOverlap(_farScreamResults);

			Debug.Log("Things in close range:");
			foreach (var closeScreamResult in _closeScreamResults)
			{
				Debug.Log(closeScreamResult.name, closeScreamResult.gameObject);
			}

			Debug.Log("Things in far range:");
			foreach (var farScreamResult in _farScreamResults)
			{
				Debug.Log(farScreamResult.name, farScreamResult.gameObject);
			}
		}

		_previousScream = scream;
	}
}