using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoatController : MonoBehaviour
{
	[SerializeField] private float      _speed = 1f;
	[SerializeField] private GameObject _cone;

	private Rigidbody2D _rigidbody2D;

	public GameObject Cone => _cone;

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

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		_cone.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
	}
}