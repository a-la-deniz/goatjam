using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GoatBack))]
public class GoatController : MonoBehaviour
{
	[SerializeField] private float _speed = 1f;
	[SerializeField] private Cone  _closeCone;
	[SerializeField] private Cone  _farCone;

	private Rigidbody2D _rigidbody2D;

	private List<Collider2D> _closeScreamResults = new List<Collider2D>();
	private List<Collider2D> _farScreamResults   = new List<Collider2D>();

	private bool _previousScream;

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

		_closeCone.transform.rotation = rotation;
		_farCone.transform.rotation = rotation;

		var scream = Input.GetAxis("Fire1") > 0;
		if (!_previousScream && scream)
		{
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
		}

		_previousScream = scream;
	}
}