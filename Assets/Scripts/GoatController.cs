using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoatController : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rigidbody2D;
	[SerializeField] private float _speed = 1f;
	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		_rigidbody2D.velocity = direction.normalized * _speed;
	}
}
